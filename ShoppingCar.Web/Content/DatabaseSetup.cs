using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mongo2Go;
using MongoDB.Driver;

namespace Awarious.Core.Repository.Mongo.Test
{
    public class DatabaseSetup : IDisposable
    {
	    internal static MongoDbRunner Runner;
		private static readonly string DbName = "MongoDbRepository_IntegrationTest";

	    public IMongoDbContext MongoDbContext { get; }

		public DatabaseSetup()
        {
	        Runner = MongoDbRunner.Start(singleNodeReplSet: true);
	        MongoDbContext = new MongoDbContext(Runner.ConnectionString, DbName);
        }

        public virtual void Dispose()
        {
            Runner.Dispose();
        }
    }
}
