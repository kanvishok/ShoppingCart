using System;
using System.Threading.Tasks;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart.Events
{
    public class EditiedBasketItemQuantity:IEvent
    {
        public Task HandleAsync(EditiedBasketItemQuantity @event)
        {
            throw new NotImplementedException();
        }
    }
}
