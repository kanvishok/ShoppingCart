using System.Threading.Tasks;
using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Domain.ShoppingCart.Commands;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Query.Models;
using ShoppingCart.Query.Queries;

namespace ShoppingCart.Application.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBus _bus;
        private readonly IQueryDispatcher _queryDispatcher;


        public ShoppingCartService(IQueryDispatcher queryDispatcher, IBus bus)
        {
            _queryDispatcher = queryDispatcher;
            _bus = bus;
        }

        public async Task<Items> ListItems()
        {
            var query = new ListItemsQuery();
            return await _queryDispatcher.DispatchAsync<ListItemsQuery, Items>(query);
        }
        public async Task AddToBasket(int customerId, int productId, int quantity)
        {
          
            var addToBasketCommand = new AddToBasketCommand(customerId, productId, quantity);
            await  _bus.SendAsync<AddToBasketCommand, Domain.ShoppingCart.Basket>(addToBasketCommand);
        }

        public async Task EditBasketItemQuantity(int customerId, int productId, int quantity)
        {
            var editBasketItemQuantityCommand = new EditBasketItemQuantityCommand(customerId, productId, quantity);
            await _bus.SendAsync<EditBasketItemQuantityCommand, Basket>(editBasketItemQuantityCommand);
        }

        public async Task<Basket> GetBasket(int customerId)
        {
            var query = new GetBasketQuery(customerId);
            return await _queryDispatcher.DispatchAsync<GetBasketQuery, Basket>(query);
        }

        public async Task Checkout(int customerId)
        {
            var command = new CheckoutCommand(customerId);
            await _bus.SendAsync<CheckoutCommand, Checkout>(command);

        }
    }
}