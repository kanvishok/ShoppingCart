using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    class CheckoutCommandHandler:ICommandHandler<CheckoutCommand>
    {
        private readonly IValidator<CheckoutCommand> _validator;

        private readonly IGenericRepository<Basket> _basketRepository;
        private readonly IGenericRepository<Checkout> _checkOutRepository;
        private readonly IGenericRepository<Stock> _stockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutCommandHandler(IValidator<CheckoutCommand> validator, IGenericRepository<Basket> basketRepository, IGenericRepository<Checkout> checkOutRepository, IUnitOfWork unitOfWork, IGenericRepository<Stock> stockRepository)
        {
            _validator = validator;
            _basketRepository = basketRepository;
            _checkOutRepository = checkOutRepository;
            _unitOfWork = unitOfWork;
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(CheckoutCommand command)
        {
            _validator.ValidateCommand(command);
            var basket = await _basketRepository.FindBy(b => b.CustomerId == command.CustomerId && b.IsActive).FirstOrDefaultAsync();
            Basket.DisableBasket(basket);
            var checkout = new Checkout(basket);
            _checkOutRepository.Add(checkout);

            //This can be improved instead of taking all the stock just get items in the basket
            var stocks = await _stockRepository.GetAll().ToListAsync();
            foreach (var item in basket.Items)
            {
                stocks.ForEach(s =>
                {
                    s.AvailableStock = s.EstimatedStock;
                    _stockRepository.Edit(s);
                });
            }

            _basketRepository.Edit(basket);
            _unitOfWork.Commit();

            return new List<IEvent>
            {
                new CheckedoutEvent()
            };
        }
    }
}
