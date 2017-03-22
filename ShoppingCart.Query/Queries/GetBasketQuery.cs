using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Query.Queries
{
    public class GetBasketQuery : IQuery<Basket>
    {
        public int CustomerId { get; private set; }
        public GetBasketQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
