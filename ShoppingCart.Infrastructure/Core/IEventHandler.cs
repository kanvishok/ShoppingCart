using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Core
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}