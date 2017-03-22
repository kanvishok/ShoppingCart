using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart.Commands;

namespace ShoppingCart.Domain.ShoppingCart.Validators
{
    public class EditBasketQuantityValidator : AbstractValidator<EditBasketItemQuantityCommand>
    {
        private readonly IGenericRepository<Stock> _stockRepository;
        private readonly IGenericRepository<Basket> _basketRepository;

        public EditBasketQuantityValidator(IGenericRepository<Stock> stockRepository, IGenericRepository<Basket> basketRepository)
        {
            _stockRepository = stockRepository;
            _basketRepository = basketRepository;

            RuleFor(b => b)
              .Must(HaveQuantity)
              .WithMessage("No Products to edit");

            RuleFor(b => b)
                .MustAsync(ItemInTheHaveBasket)
                .WithMessage("You need to add the item before update the quantity. Please use add to basket");

            RuleFor(b => b)
                .MustAsync(HaveStock)
                .WithMessage("Sorry ..! We do not have sufficient stock for this product");
        }

        private async Task<bool> ItemInTheHaveBasket(EditBasketItemQuantityCommand command, CancellationToken cancellationToken)
        {
            var basket =
                await _basketRepository.FindBy(b => b.CustomerId == command.CustomerId && b.IsActive)
                    .FirstOrDefaultAsync(cancellationToken);
            bool isValid = command.BasketId != 0;
            if (isValid && basket?.Items != null)
                isValid = basket.Items.Any(i => i.ProductId == command.ProductId);
            return isValid;
        }

        private bool HaveQuantity(EditBasketItemQuantityCommand command)
        {
            var isvalid = command.Quantity > 0;
            return isvalid;
        }
        private async Task<bool> HaveStock(EditBasketItemQuantityCommand command, CancellationToken cancellationToken)
        {
            var stock =
                await _stockRepository.FindBy(s => s.ProductId == command.ProductId)
                    .FirstOrDefaultAsync(cancellationToken);

            return stock != null && (stock.EstimatedStock - command.Quantity >= 0);
        }

       
    }
}