using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Awarious.Core.Infrastructure;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Xunit;

namespace Awarious.Core.Repository.Mongo.Test
{
	[Collection("Integration test collection")]
	public class RepositoryServiceMongoTest
	{
		private readonly TestFixture _fixture;
		private readonly string _testCollectionName;
		public RepositoryServiceMongoTest(TestFixture fixture)
		{
			_testCollectionName = typeof(TestDocument).Name.ToLower();
			_fixture = fixture;

			if (!BsonClassMap.IsClassMapRegistered(typeof(TestDocument)))
			{
				BsonClassMap.RegisterClassMap<TestDocument>(map =>
				{
					map.AutoMap();
					map.MapIdProperty(x => x.TestDocumentId);
				});
			}
		}
		
		[Fact]
		public void Should_Create_RepositoryService_Instance()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			Assert.NotNull(mongoRepo);
		}

		[Fact]
		public async Task Add_Should_Add_New_Document_To_Collection()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			mongoRepo.Add(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});

			var documents = await mongoRepo.GetAllAsync();
			Assert.NotNull(documents);
			Assert.Single(documents);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task AddAsync_Should_Add_New_Document_To_Collection()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});

			var documents = await mongoRepo.GetAllAsync();
			Assert.NotNull(documents);
			Assert.Single(documents);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task Inserted_UTC_DateTime_Should_Be_Set_Automatically_For_New_Document()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});

			var documents = await mongoRepo.GetAllAsync();
			Assert.NotNull(documents);
			Assert.Single(documents);
			Assert.NotEqual(default(DateTime), documents.First().InsertedDate);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task AddAll_Should_Add_Multiple_Documents_To_Collection()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			mongoRepo.AddAll(new List<TestDocument>{
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName1"
				},
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName2"
				}
			});

			var documents = await mongoRepo.GetAllAsync();
			Assert.NotNull(documents);
			Assert.Equal(2, documents.Count);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task AddAllAsync_Should_Add_Multiple_Documents_To_Collection()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			await mongoRepo.AddAllAsync(new List<TestDocument>{
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName1"
				},
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName2"
				}
			});

			var documents = await mongoRepo.GetAllAsync();
			Assert.NotNull(documents);
			Assert.Equal(2, documents.Count);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task Inserted_UTC_DateTime_Should_Be_Set_For_All_Newly_Added_Documents()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			await mongoRepo.AddAllAsync(new List<TestDocument>{
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName1"
				},
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName2"
				}
			});

			var documents = await mongoRepo.GetAllAsync();
			Assert.NotNull(documents);
			Assert.Equal(2, documents.Count);
			Assert.NotEqual(default(DateTime), documents.First().InsertedDate);
			Assert.NotEqual(default(DateTime), documents.Last().InsertedDate);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task Update_Should_Update_Document()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			var document = new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			};

			await mongoRepo.AddAsync(document);
			
			document.FirstName = "FName1Updated";

			mongoRepo.Update(document);

			var documentFromDb = await mongoRepo.FindFirstAsync(x => x.TestDocumentId == document.TestDocumentId);

			Assert.NotNull(documentFromDb);
			Assert.Equal("FName1Updated", documentFromDb.FirstName);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task UpdateAsync_Should_Update_Document()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			var document = new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			};

			await mongoRepo.AddAsync(document);

			document.FirstName = "FName1Updated";

			await mongoRepo.UpdateAsync(document);

			var documentFromDb = await mongoRepo.FindFirstAsync(x => x.TestDocumentId == document.TestDocumentId);

			Assert.NotNull(documentFromDb);
			Assert.Equal("FName1Updated", documentFromDb.FirstName);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task Updated_UTC_DateTime_Should_Be_Set_For_Updated_Document()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			var document = new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			};

			await mongoRepo.AddAsync(document);

			document.FirstName = "FName1Updated";

			await mongoRepo.UpdateAsync(document);

			var documentFromDb = await mongoRepo.FindFirstAsync(x => x.TestDocumentId == document.TestDocumentId);

			Assert.NotNull(documentFromDb);
			Assert.NotNull(documentFromDb.UpdatedDate);
			Assert.NotEqual(default(DateTime), documentFromDb.UpdatedDate);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task Update_Should_Update_Document_Based_On_The_Update_Definition()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			var newDocumentId = Utility.NewSequentialGuid();
			var document = new TestDocument
			{
				TestDocumentId = newDocumentId,
				FirstName = "FName1"
			};

			await mongoRepo.AddAsync(document);

			var updateDefinition = Builders<TestDocument>.Update.Set(x => x.FirstName, "FName1Updated");

			mongoRepo.Update(newDocumentId, updateDefinition);

			var documentFromDb = await mongoRepo.FindFirstAsync(x => x.TestDocumentId == document.TestDocumentId);

			Assert.NotNull(documentFromDb);
			Assert.Equal("FName1Updated", documentFromDb.FirstName);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task UpdateAsync_Should_Update_Document_Based_On_The_Update_Definition()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			var newDocumentId = Utility.NewSequentialGuid();
			var document = new TestDocument
			{
				TestDocumentId = newDocumentId,
				FirstName = "FName1"
			};

			await mongoRepo.AddAsync(document);

			var updateDefinition = Builders<TestDocument>.Update.Set(x => x.FirstName, "FName1Updated");

			await mongoRepo.UpdateAsync(newDocumentId, updateDefinition);

			var documentFromDb = await mongoRepo.FindFirstAsync(x => x.TestDocumentId == document.TestDocumentId);

			Assert.NotNull(documentFromDb);
			Assert.Equal("FName1Updated", documentFromDb.FirstName);
			Assert.NotEqual(default(DateTime), documentFromDb.UpdatedDate);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task UpdateAsync_Should_Update_Document_Based_On_The_Expression()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});
			

			var updateDefinition = Builders<TestDocument>.Update.Set(x => x.FirstName, "FName1Updated");

			await mongoRepo.UpdateAsync(x => x.FirstName == "FName1", updateDefinition);

			var documentFromDb = await mongoRepo.GetAsync(x => x.FirstName == "FName1Updated");

			Assert.NotNull(documentFromDb);
			Assert.True(documentFromDb.Count == 1);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}
		[Fact]
		public async Task UpdateAsync_Should_Update_Multiple_Documents_Based_On_The_Expression()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});
			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});
			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});
			await mongoRepo.AddAsync(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});

			var updateDefinition = Builders<TestDocument>.Update.Set(x => x.FirstName, "FName1Updated");

			await mongoRepo.UpdateAsync(x => x.FirstName == "FName1", updateDefinition);

			var documentFromDb = await mongoRepo.GetAsync(x => x.FirstName == "FName1Updated");

			Assert.NotNull(documentFromDb);
			Assert.True(documentFromDb.Count == 4);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}

		[Fact]
		public async Task UpdateAsync_Should_Update_All_Documents()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);

			await mongoRepo.AddAllAsync(new List<TestDocument>{
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName1"
				},
				new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName2"
				}
			});

			var documents = await mongoRepo.GetAllAsync();

			documents.First().FirstName = "FName1Updated";
			documents.Last().FirstName = "FName2Updated";

			await mongoRepo.UpdateAllAsync(documents);

			var documentFromDb = await mongoRepo.GetAllAsync();

			Assert.NotNull(documentFromDb);
			Assert.Equal("FName1Updated", documentFromDb.First().FirstName);
			Assert.Equal("FName2Updated", documentFromDb.Last().FirstName);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
		}
	}
}
