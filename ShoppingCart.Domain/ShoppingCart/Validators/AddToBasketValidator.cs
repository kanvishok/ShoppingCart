using System.Linq;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart.Commands;

namespace ShoppingCart.Domain.ShoppingCart.Validators
{
    public class AddToBasketValidator : AbstractValidator<AddToBasketCommand>
    {
        private readonly IGenericRepository<Basket> _basketRepository;
        private readonly IGenericRepository<Stock> _stockRepository;

        public AddToBasketValidator(IGenericRepository<Basket> basketRepository, IGenericRepository<Stock> stockRepository)
        {
            _basketRepository = basketRepository;
            _stockRepository = stockRepository;

            RuleFor(b => b)
                .Must(HaveQuantity)
                .WithMessage("No Products to add");

            RuleFor(b => b)
                .MustAsync(BeUniqueProductId)
                .WithMessage("This product is already added to basket, you can edit the quantity");

            RuleFor(b => b)
              .MustAsync(HaveStock)
              .WithMessage("Sorry ..! We do not have sufficient stock for this product");
        }

        private bool HaveQuantity(AddToBasketCommand command)
        {
            var isvalid = command.Quantity > 0;
            return isvalid;
        }

        private async Task<Basket> GetBasket(AddToBasketCommand command, CancellationToken cancellationToken)
        {
            return await _basketRepository.FindBy(b => b.Id == command.BasketId && b.IsActive).FirstOrDefaultAsync(cancellationToken);
        }
        private async Task<bool> BeUniqueProductId(AddToBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await GetBasket(command, cancellationToken);
            bool isValid = true;
            if (basket != null && basket.Items != null)
                isValid = basket.Items.All(i => i.ProductId != command.ProductId);
            return isValid;
        }
        private async Task<bool> HaveStock(AddToBasketCommand command, CancellationToken cancellationToken)
        {
            var stock =
                await _stockRepository.FindBy(s => s.ProductId == command.ProductId)
                    .FirstOrDefaultAsync(cancellationToken);

            return stock != null && (stock.EstimatedStock - command.Quantity >= 0);
        }
    }
}
