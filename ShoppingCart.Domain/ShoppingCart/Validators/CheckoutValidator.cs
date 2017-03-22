using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart.Commands;

namespace ShoppingCart.Domain.ShoppingCart.Validators
{
    public class CheckoutValidator : AbstractValidator<CheckoutCommand>
    {
        private readonly IGenericRepository<Basket> _basketRepository;

        public CheckoutValidator(IGenericRepository<Basket> basketRepository)
        {
            _basketRepository = basketRepository;

            RuleFor(c => c)
                .MustAsync(HaveItemsToCheckout)
                .WithMessage("There is no items to checkout in your basket");
        }

        private async Task<bool> HaveItemsToCheckout(CheckoutCommand command, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.FindBy(b => b.CustomerId == command.CustomerId && b.IsActive).FirstOrDefaultAsync(cancellationToken);

            return basket != null &&  basket.Items.Count > 0;
        }
    }
}
