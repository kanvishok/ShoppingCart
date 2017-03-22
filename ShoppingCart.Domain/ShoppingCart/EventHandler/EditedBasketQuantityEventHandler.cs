using System;
using System.Threading.Tasks;
using ShoppingCart.Domain.ShoppingCart.Events;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart.EventHandler
{
    public class EditedBasketQuantityEventHandler:IEventHandler<EditiedBasketItemQuantity>
    {
        public Task HandleAsync(EditiedBasketItemQuantity @event)
        {
            throw new NotImplementedException();
        }
    }
}
