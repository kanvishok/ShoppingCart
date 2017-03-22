using System.Data.Entity;
using System.Threading.Tasks;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Query.Queries;

namespace ShoppingCart.Query.QueryHandler
{
    public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, Basket>
    {
        private readonly IGenericRepository<Basket> _basketRepository;

        public GetBasketQueryHandler(IGenericRepository<Basket> basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Basket> RetrieveAsync(GetBasketQuery query)
        {
            return await _basketRepository.FindBy(b => b.CustomerId == query.CustomerId && b.IsActive == true).FirstOrDefaultAsync();
        }
    }
}