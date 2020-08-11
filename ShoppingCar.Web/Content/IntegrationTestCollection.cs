using Xunit;

namespace Awarious.Core.Repository.Mongo.Test
{
    [CollectionDefinition("Integration test collection")]
    public class IntegrationTestCollection : ICollectionFixture<TestFixture>
    {
    }
}
