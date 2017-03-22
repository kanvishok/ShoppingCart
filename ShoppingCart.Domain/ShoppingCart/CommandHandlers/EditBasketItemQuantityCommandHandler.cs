using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using FluentValidation;
using HomeCinema.Data.Infrastructure;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart.Commands;
using ShoppingCart.Domain.ShoppingCart.Events;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart.CommandHandlers
{
    public class EditBasketItemQuantityCommandHandler : ICommandHandler<EditBasketItemQuantityCommand>
    {
        private readonly IValidator<EditBasketItemQuantityCommand> _validator;

        private readonly IGenericRepository<Basket> _basketRepository;
        private readonly IGenericRepository<Stock> _stockRepository;
        private readonly IUnitOfWork _unitOfWork;


        public EditBasketItemQuantityCommandHandler(IValidator<EditBasketItemQuantityCommand> validator, IGenericRepository<Basket> basketRepository, IGenericRepository<Stock> stockRepository, IUnitOfWork unitOfWork)
        {
            _validator = validator;
            _basketRepository = basketRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(EditBasketItemQuantityCommand command)
        {
            var existingBasket = await _basketRepository.FindBy(b => b.CustomerId == command.CustomerId).FirstOrDefaultAsync();

            if (existingBasket != null)
                command.BasketId = existingBasket.Id;

            _validator.ValidateCommand(command);

            var basket = Basket.EditBasketItemQuantity(command, existingBasket);

            
            //StockHelper.UpdateStock(_stockRepository, command.ProductId,command.Quantity);

            ////Assuming that always there will be a stock entry for the product. 
            ////Even the stock is not available then there will be stock entry with zero stock
            var stock = await _stockRepository.FindBy(s => s.ProductId == command.ProductId).FirstOrDefaultAsync();

            if (stock != null)
            {
               // stock.AvailableStock = stock.AvailableStock - command.Quantity;
                stock.EstimatedStock = stock.AvailableStock - command.Quantity;
                _stockRepository.Edit(stock);
            }

            _unitOfWork.Commit();

            return new List<IEvent>
            {
                new EditiedBasketItemQuantity()
            };
        }
    }
}