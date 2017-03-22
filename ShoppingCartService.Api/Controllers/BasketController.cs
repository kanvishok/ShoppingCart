using System.Threading.Tasks;
using System.Web.Http;
using ShoppingCart.Api.Models;
using ShoppingCart.Application.Service;
using ShoppingCart.Domain.ShoppingCart;
using ShoppingCart.Query.Models;

namespace ShoppingCart.Api.Controllers
{
    [RoutePrefix("api/basket")]
    public class BasketController : ApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        public BasketController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("list-items")]
        public async Task<Items> ListItems()
        {
            var items = await _shoppingCartService.ListItems();
            return items;
        }
        [HttpPost]

        [Route("add-to-basket")]
        public async Task<IHttpActionResult> AddToBasket([FromBody]BasketModel basketModel)
        {
            await _shoppingCartService.AddToBasket(basketModel.CustomerId, basketModel.ProductId, basketModel.Quantity);
            return Ok();
        }

        [HttpPost]
        [Route("edit-basket-item-quantity")]
        public async Task<IHttpActionResult> EditBasketItemQuantity([FromBody]BasketModel basketModel)
        {
            await _shoppingCartService.EditBasketItemQuantity(basketModel.CustomerId, basketModel.ProductId, basketModel.Quantity);
            return Ok();
        }

        [HttpGet]
        [Route("get-basket")]
        public async Task<Basket> GetBasketItems([FromUri] int customerId)
        {
            return await _shoppingCartService.GetBasket(customerId);

        }

    }
}