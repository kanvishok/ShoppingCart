using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using HomeCinema.Data.Infrastructure;
using Moq;
using NUnit.Framework;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Domain.ShoppingCart.CommandHandlers;
using ShoppingCart.Domain.ShoppingCart.Commands;

namespace ShoppingCart.Query.Test
{
    [TestFixture()]
    public class AddToBasketCommandHandlerTest
    {

        private Mock<IValidator<AddToBasketCommand>> _addToBasketValidatorMock = new Mock<IValidator<AddToBasketCommand>>();
        private Mock<IGenericRepository<Basket>> _basketRepository = new Mock<IGenericRepository<Basket>>();
        private Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private Mock<IGenericRepository<Stock>> _stockRepository = new Mock<IGenericRepository<Stock>>();
        private AddToBasketCommand _command = new AddToBasketCommand(1, 1, 1);
        [SetUp]
        public void Init()
        {
            var basketData = new FakeDbSet<Basket>();
            var stockData = new FakeDbSet<Stock>();

            _basketRepository.Setup(x => x.Add(It.IsAny<Basket>())).Returns(It.IsAny<int>);
            _basketRepository.Setup(x => x.Edit(It.IsAny<Basket>())).Returns(It.IsAny<int>);
            _basketRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Basket, bool>>>())).Returns(basketData);
            _stockRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Stock, bool>>>())).Returns(stockData);
            _stockRepository.Setup(x => x.Edit(It.IsAny<Stock>())).Returns(It.IsAny<int>);
            _addToBasketValidatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _unitOfWork.Setup(x => x.Commit());
        }

        [Test]
        public void Should_throw_an_exception_when_validation_fails()
        {
            _addToBasketValidatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("MustHaveQuantity", "No Products to add") }));

            var handler = new AddToBasketCommandHandler(_addToBasketValidatorMock.Object, _basketRepository.Object, _unitOfWork.Object, _stockRepository.Object);

            Assert.ThrowsAsync<ValidationException>(async () => await handler.HandleAsync(_command));
        }

        [Test]
        public async Task Should_Add_Item_To_Basket()
        {
            var handler = new AddToBasketCommandHandler(_addToBasketValidatorMock.Object, _basketRepository.Object, _unitOfWork.Object, _stockRepository.Object);

            await handler.HandleAsync(_command);

            _basketRepository.Verify(x => x.Add(It.IsAny<Basket>()));
        }
    }
}
