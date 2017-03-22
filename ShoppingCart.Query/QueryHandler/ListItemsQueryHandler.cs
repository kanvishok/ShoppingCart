using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Query.Models;
using ShoppingCart.Query.Queries;

namespace ShoppingCart.Query.QueryHandler
{
    public class ListItemsQueryHandler:IQueryHandler<ListItemsQuery,Items>
    {
        private readonly IGenericRepository<Product> _productRepository;
        public ListItemsQueryHandler(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Items> RetrieveAsync(ListItemsQuery query)
        {
            var products =
                await _productRepository.GetAll()
                    .SelectMany(p => p.Stocks.Where(s => s.EstimatedStock > 0))
                    .ToListAsync();

            var items = new Items() {AllItems = products.Select(x=>x.Products)};

            return items;
        }
    }
}