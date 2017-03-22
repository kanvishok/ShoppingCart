using System.Data.Entity;
using System.Threading.Tasks;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Query.Queries;

namespace ShoppingCart.Query.QueryHandler
{
    public class CheckoutQueryHandler
    {
        private readonly IGenericRepository<Checkout> _checkoutRepository;

        public CheckoutQueryHandler(IGenericRepository<Checkout> checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<Checkout> RetrieveAsync(GetBasketQuery query)
        {
            return await _checkoutRepository.FindBy(b => b.CustomerId == query.CustomerId).FirstOrDefaultAsync();
        }
    }
}
