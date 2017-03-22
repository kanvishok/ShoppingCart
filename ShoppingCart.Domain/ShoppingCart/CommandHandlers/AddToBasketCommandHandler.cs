using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using FluentValidation;
using HomeCinema.Data.Infrastructure;
using ShoppingCart.Domain.ShoppingCart.Commands;
using ShoppingCart.Domain.ShoppingCart.Events;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Domain.ShoppingCart.CommandHandlers
{
    public class AddToBasketCommandHandler : ICommandHandler<AddToBasketCommand>
    {
        private readonly IValidator<AddToBasketCommand> _validator;

        private readonly IGenericRepository<Basket> _basketRepository;
        private readonly IGenericRepository<Stock> _stockRepository;

        private readonly IUnitOfWork _unitOfWork;



        public AddToBasketCommandHandler(IValidator<AddToBasketCommand> validator, IGenericRepository<Basket> basketRepository, IUnitOfWork unitOfWork, IGenericRepository<Stock> stockRepository)
        {
            _validator = validator;
            _basketRepository = basketRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(AddToBasketCommand command)
        {

            var existingBasket = await _basketRepository.FindBy(b => b.CustomerId == command.CustomerId && b.IsActive).FirstOrDefaultAsync();
            if (existingBasket != null)
                command.BasketId = existingBasket.Id;

            _validator.ValidateCommand(command);

            var basket = Basket.AddToBasket(command, existingBasket);

            //we can use Entity framework addorupdate method to avoid this if condition
            if (existingBasket == null)
            {
                _basketRepository.Add(basket);
            }
            else
            {
                _basketRepository.Edit(basket);
            }

            // StockHelper.UpdateStock(_stockRepository, command.ProductId, command.Quantity);

            //Assuming that always there will be a stock entry for the product. 
            //Even the stock is not available then there will be stock entry with zero stock
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
                new AddedToBasketEvent()
            };
        }
    }
}
