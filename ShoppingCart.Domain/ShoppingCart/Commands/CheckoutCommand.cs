using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart.Commands
{
    public class CheckoutCommand : ICommand
    {
        public CheckoutCommand(int customerId)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; private set; }
    }
}
