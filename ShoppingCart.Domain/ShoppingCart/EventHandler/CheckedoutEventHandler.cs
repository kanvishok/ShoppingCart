using System;
using System.Threading.Tasks;
using ShoppingCart.Domain.ShoppingCart.Events;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart.EventHandler
{
    public class CheckedoutEventHandler:IEventHandler<CheckedoutEvent>
    {
        public Task HandleAsync(CheckedoutEvent @event)
        {
            //Send Invoice Email to Customer
            throw new NotImplementedException();
        }
    }
}
