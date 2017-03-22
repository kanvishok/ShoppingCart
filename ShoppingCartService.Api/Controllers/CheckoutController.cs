using System.Threading.Tasks;
using System.Web.Http;
using ShoppingCart.Api.Models;
using ShoppingCart.Application.Service;

namespace ShoppingCart.Api.Controllers
{
    [RoutePrefix("api/checkout")]
    public class CheckoutController : ApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        
        public CheckoutController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Route("checkout-basket")]
        public async Task CheckoutBasket([FromBody] CheckoutModel checkoutModel )
        {
            await _shoppingCartService.Checkout(checkoutModel.CustomerId);
        }

    }
}
