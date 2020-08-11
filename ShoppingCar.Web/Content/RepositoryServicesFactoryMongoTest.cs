using System;
using System.Collections.Generic;
using System.Text;
using Awarious.Core.Infrastructure.Repository;
using MongoDB.Bson.Serialization;
using Xunit;

namespace Awarious.Core.Repository.Mongo.Test
{
	[Collection("Integration test collection")]
	public class RepositoryServicesFactoryMongoTest
	{
		private readonly TestFixture _fixture;
		private readonly string _testCollectionName;
		public RepositoryServicesFactoryMongoTest(TestFixture fixture)
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
		public void GetRepositoryService_OfT_Should_Return_RepositoryServiceMongo_Instance()
		{
			var mongoRepos = new RepositoryServicesFactoryMongo(_fixture.MongoDbContext);
			var testDocRepo = mongoRepos.GetRepositoryService<TestDocument>();
			Assert.NotNull(testDocRepo);
			Assert.IsAssignableFrom<IRepositoryServiceMongo<TestDocument>>(testDocRepo);
		}

		[Fact]
		public void GetRepositoryService_Should_Return_RepositoryServiceMongo_Instance()
		{
			var mongoRepos = new RepositoryServicesFactoryMongo(_fixture.MongoDbContext);
			var testDocRepo = mongoRepos.GetRepositoryService(typeof(TestDocument));
			Assert.NotNull(testDocRepo);
			Assert.IsAssignableFrom<IRepositoryServiceMongo<TestDocument>>(testDocRepo);
		}

		[Fact]
		public void GetRepositoryServiceMongo_OfT_Should_Return_RepositoryServiceMongo_Instance()
		{
			var mongoRepos = new RepositoryServicesFactoryMongo(_fixture.MongoDbContext);
			var testDocRepo = mongoRepos.GetRepositoryServiceMongo<TestDocument>();
			Assert.NotNull(testDocRepo);
			Assert.IsAssignableFrom<IRepositoryServiceMongo<TestDocument>>(testDocRepo);
		}
	}
}
