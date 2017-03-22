using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Infrastructure.Dependencies;

namespace ShoppingCart.Query.Test
{
    [TestFixture]
    public class QueryDispatcherTest
    {
        [Test]
        public void Dispatch_should_throw_an_exception_when_query_is_null()
        {
            var resolverMock = new Mock<IResolver>();
            var queryDispatcher = new QueryDispatcher(resolverMock.Object);

            Assert.Throws<ArgumentNullException>(() => queryDispatcher.DispatchAsync<FakeQuery, int>(null));
        }

        [Test]
        public void DispatchAsync_should_throw_an_exception_when_query_is_null()
        {
            var resolverMock = new Mock<IResolver>();
            var queryDispatcher = new QueryDispatcher(resolverMock.Object);

            Assert.Throws<ArgumentNullException>(() => queryDispatcher.DispatchAsync<FakeQuery, int>(null));
        }

        [Test]
        public void Dispatch_should_retrieve_query_result()
        {
            var resolverMock = new Mock<IResolver>();
            resolverMock.Setup(r => r.ResolveOptional(It.IsAny<Type>())).Returns(new FakeQueryHandler());
            var queryDispatcher = new QueryDispatcher(resolverMock.Object);

            Assert.AreEqual(100, queryDispatcher.DispatchAsync<FakeQuery, int>(new FakeQuery()).Result);
        }

        [Test]
        public void Dispatch_should_throw_an_exception_when_query_handler_is_not_found()
        {
            var resolverMock = new Mock<IResolver>();
            var queryDispatcher = new QueryDispatcher(resolverMock.Object);

            Assert.Throws<ArgumentException>(() => queryDispatcher.DispatchAsync<FakeQuery, int>(new FakeQuery()));
        }

        [Test]
        public void DispatchAsync_should_throw_an_exception_when_query_handler_is_not_found_()
        {
            var resolverMock = new Mock<IResolver>();
            var queryDispatcher = new QueryDispatcher(resolverMock.Object);

            Assert.Throws<ArgumentException>(() => queryDispatcher.DispatchAsync<FakeQuery, int>(new FakeQuery()));
        }

        [Test]
        public void DispatchAsync_should_retrieve_query_result()
        {
            var resolverMock = new Mock<IResolver>();
            resolverMock.Setup(r => r.ResolveOptional(It.IsAny<Type>())).Returns(new FakeQueryHandlerAsync());
            var queryDispatcher = new QueryDispatcher(resolverMock.Object);

            Assert.AreEqual(100, queryDispatcher.DispatchAsync<FakeQuery, int>(new FakeQuery()).Result);
        }

        private class FakeQuery : IQuery<int>
        { }

        private class FakeQueryHandler : IQueryHandler<FakeQuery, int>
        {
            public Task<int> RetrieveAsync(FakeQuery query)
            {
                return Task.Run(() => 100);
            }
        }

        private class FakeQueryHandlerAsync : IQueryHandler<FakeQuery, int>
        {
            public Task<int> RetrieveAsync(FakeQuery query)
            {
                return Task.FromResult(100);
            }
        }
    }
}
