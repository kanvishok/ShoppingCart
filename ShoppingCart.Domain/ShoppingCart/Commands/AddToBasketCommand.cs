using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart.Commands
{
    public class AddToBasketCommand : ICommand
    {
        public AddToBasketCommand(  int customerId, int productId, int quantity)
        {
            
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }

        public int BasketId { get; set; }
        public int ProductId { get; private set; }
        public int CustomerId { get; private set; }
        public int Quantity { get; private set; }
       
        
    }
}
