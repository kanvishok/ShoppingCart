using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Query.Queries
{
    public class GetChckoutQuery:IQuery<Checkout>
    {
        public int CustomerId { get; private set; }
        public GetChckoutQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}