
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Core
{
    public interface IBus
    {
        Task SendAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : ICommand
            where TAggregate : IAggregateRoot;
    }
}
