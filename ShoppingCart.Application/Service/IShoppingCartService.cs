using System.Threading.Tasks;
using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Query.Models;

namespace ShoppingCart.Application.Service
{
    public interface IShoppingCartService
    {
        Task<Items> ListItems();
        Task AddToBasket(int customerId, int productId, int quantity);
        Task EditBasketItemQuantity(int customerId, int productId, int quantity);
        Task<Basket> GetBasket(int customerId);
        Task Checkout(int customerId);

    }
}