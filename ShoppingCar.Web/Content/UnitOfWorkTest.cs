using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Awarious.Core.Infrastructure;
using MongoDB.Bson.Serialization;
using Xunit;

namespace Awarious.Core.Repository.Mongo.Test
{
	[Collection("Integration test collection")]
	public class UnitOfWorkTest
	{
		private readonly TestFixture _fixture;
		private readonly string _testCollectionName;

		public UnitOfWorkTest(TestFixture fixture)
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
		public async Task Should_Save_MultipleDocument_Within_OneTransaction()
		{
			var mongoRepo = new RepositoryServiceMongo<TestDocument>(_fixture.MongoDbContext);
			await _fixture.MongoDbContext.Database.DropCollectionAsync(mongoRepo.CollectionName);
			mongoRepo.Add(new TestDocument
			{
				TestDocumentId = Utility.NewSequentialGuid(),
				FirstName = "FName1"
			});

			using (var repositories = new UnitOfWorkMongo(_fixture.MongoDbContext))
			{

				var testDocumentRepo = repositories.GetRepositoryServiceMongo<TestDocument>();
				await testDocumentRepo.AddAsync(new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName2"
				});
				await testDocumentRepo.AddAsync(new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName3"
				});

				await repositories.SaveChangesAsync();

				await testDocumentRepo.AddAsync(new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName2"
				});
				await testDocumentRepo.AddAsync(new TestDocument
				{
					TestDocumentId = Utility.NewSequentialGuid(),
					FirstName = "FName3"
				});

				await repositories.SaveChangesAsync();

				var documents = await mongoRepo.GetAllAsync();
				Assert.NotNull(documents);
				Assert.True(documents.Count == 5);
			}
		}
	}
}
