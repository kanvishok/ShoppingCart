using System;
using Moq;
using NUnit.Framework;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Infrastructure.Dependencies;

namespace ShoppingCart.Query.Test
{
    [TestFixture]
    public class BusTest
    {
        [Test]
        public void Should_throw_an_exception_when_command_is_null()
        {
            var resolverMock = new Mock<IResolver>();

            var bus = new Bus(resolverMock.Object);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await bus.SendAsync<ICommand, IAggregateRoot>(null));
        }

        [Test]
        public void Should_throw_an_exception_when_command_handler_is_not_found()
        {
            var resolverMock = new Mock<IResolver>();

            var bus = new Bus(resolverMock.Object);

            Assert.ThrowsAsync<NullReferenceException>(async () => await  bus.SendAsync<ICommand, IAggregateRoot>(new FakeCommand()));
        }

        private class FakeCommand : ICommand { }
    }
}
