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
	public class ReadOnlyRepositorySerivceMongoTest
	{
		private readonly TestFixture _fixture;
		private readonly string _testCollectionName;
		public ReadOnlyRepositorySerivceMongoTest(TestFixture fixture)
		{
			_testCollectionName = typeof(TestDocument).Name;
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
		public void Should_Create_ReadOnlyRepository_Instance()
		{
			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			Assert.NotNull(readOnlyRepo);
		}

		[Fact]
		public async Task GetAllAsync_Should_Return_Only_Top_2_Documents()
		{
			//Arrange
			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext, "MyTestCollection");
			var collectionName = readOnlyRepo.CollectionName;

			await _fixture.MongoDbContext.Database.DropCollectionAsync(collectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(collectionName);
			
			await testDocsCollection
			.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3"
					}
				});


			
			var testDocs = await readOnlyRepo.GetAllAsync(2);
			Assert.NotNull(testDocs);
			Assert.Equal(3, testDocsCollection.AsQueryable().Count());
			Assert.Equal(2, testDocs.Count);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(collectionName);
		}

		[Fact]
		public async Task GetPagedListAsync_Should_Apply_DynamicPredicates()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var testDocs = await readOnlyRepo.GetPagedListAsync(1, 2, null, 
				new Dictionary<string, object[]>{{ "FirstName = @0 OR FirstName = @1", new object[] {"FName1", "FName2"}}});
			Assert.NotNull(testDocs);
			Assert.Equal(4, testDocsCollection.AsQueryable().Count());
			Assert.Equal(2, testDocs.Count);
			Assert.IsType<PagedList<TestDocument>>(testDocs);
			Assert.Equal(2, ((PagedList<TestDocument>) testDocs).TotalCount);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task GetPagedListAsync_Should_Apply_Predicates()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var testDocs = await readOnlyRepo.GetPagedListAsync(1, 2, x => x.FirstName == "FName1" || x.FirstName == "FName2", null);
			Assert.NotNull(testDocs);
			Assert.Equal(4, testDocsCollection.AsQueryable().Count());
			Assert.Equal(2, testDocs.Count);
			Assert.IsType<PagedList<TestDocument>>(testDocs);
			Assert.Equal(2, ((PagedList<TestDocument>)testDocs).TotalCount);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task GetPagedListAsync_Should_Return_Documents_Sorted()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1",
						Age = 45,
						Designation = "Developer"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2",
						Age = 23,
						Designation = "Developer"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3",
						Age = 72,
						Designation = "Tester"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4",
						Age = 56,
						Designation = "Developer"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var testDocs = await readOnlyRepo.GetPagedListAsync(1, 4, null, null, new Dictionary<string, ListSortDirection>()
			{
				{"Designation", ListSortDirection.Ascending},
				{"Age", ListSortDirection.Descending}
			});
			Assert.NotNull(testDocs);
			Assert.Equal(4, testDocsCollection.AsQueryable().Count());
			Assert.Equal("Developer", testDocs[0].Designation);
			Assert.Equal(56, testDocs[0].Age);
			Assert.Equal(23, testDocs[2].Age);
			Assert.Equal("Tester", testDocs[3].Designation);
			Assert.Equal(4, ((PagedList<TestDocument>)testDocs).TotalCount);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task GetPagedListAsync_Should_Return_Second_Page()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var testDocs = await readOnlyRepo.GetPagedListAsync(2, 2);
			Assert.NotNull(testDocs);
			Assert.Equal(4, testDocsCollection.AsQueryable().Count());
			Assert.Equal(2, testDocs.Count);
			Assert.IsType<PagedList<TestDocument>>(testDocs);
			Assert.Equal(4, ((PagedList<TestDocument>)testDocs).TotalCount);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task FindFirstAsync_Should_Return_First_Matching_Record()
		{
			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(readOnlyRepo.CollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(readOnlyRepo.CollectionName);

			var userIdToFind = Utility.NewSequentialGuid();
			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1",
						Age = 30
					},
					new TestDocument
					{
						TestDocumentId = userIdToFind,
						FirstName = "FName2",
						Age = 24
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3",
						Age = 42
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4",
						Age = 12
					}
				});

			var documentFromDb = await readOnlyRepo.FindFirstAsync(x => x.TestDocumentId == userIdToFind);
			Assert.NotNull(documentFromDb);
			Assert.Equal("FName2", documentFromDb.FirstName);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(readOnlyRepo.CollectionName);
		}

		[Fact]
		public async Task FindFirstAsync_Should_OrderByDescending_And_Return_FirstRecord()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1",
						Age = 30
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2",
						Age = 24
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3",
						Age = 42
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4",
						Age = 12
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var testDocs = await readOnlyRepo.FindFirstAsync(null, new Dictionary<string, ListSortDirection>()
			{
				{"Age", ListSortDirection.Descending},
				{"FirstName", ListSortDirection.Descending}
			});
			Assert.NotNull(testDocs);
			Assert.Equal("FName3", testDocs.FirstName);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task GetCountAsync_Should_Return_Total_Document_Count()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var docsCount = readOnlyRepo.GetCount();
			Assert.Equal(4, docsCount);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task GetMaxAsync_Should_Return_Maximum_Age()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1",
						Age = 35,
						Designation = "Developer"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2",
						Age = 74,
						Designation = "Tester"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3",
						Age = 22,
						Designation = "Developer"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4",
						Age = 53,
						Designation = "Developer"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var maxAge = await readOnlyRepo.GetMaxAsync(x => x.Age);
			Assert.Equal(74, maxAge);

			var maxDeveloperAge = await readOnlyRepo.GetMaxAsync(x => x.Age, x => x.Designation == "Developer");
			Assert.Equal(53, maxDeveloperAge);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}

		[Fact]
		public async Task GetMaxAsync_Should_Return_0_When_No_Sequence()
		{
			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);

			var testDocsCollection = _fixture.MongoDbContext.Database
				.GetCollection<TestDocument>(_testCollectionName);

			await testDocsCollection
				.InsertManyAsync(new List<TestDocument>
				{
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName1",
						Age = 35,
						Designation = "Developer"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName2",
						Age = 74,
						Designation = "Tester"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName3",
						Age = 22,
						Designation = "Developer"
					},
					new TestDocument
					{
						TestDocumentId = Utility.NewSequentialGuid(),
						FirstName = "FName4",
						Age = 53,
						Designation = "Developer"
					}
				});


			var readOnlyRepo = new ReadOnlyRepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			var maxAge = await readOnlyRepo.GetMaxAsync(x => x.Age);
			Assert.Equal(74, maxAge);

			var maxDeveloperAge = await readOnlyRepo.GetMaxAsync(x => x.Age, x => x.Designation == "Developer1");
			Assert.Equal(0, maxDeveloperAge);

			await _fixture.MongoDbContext.Database.DropCollectionAsync(_testCollectionName);
		}
	}
}
