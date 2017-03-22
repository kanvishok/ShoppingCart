using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Core
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
         Task<IEnumerable<IEvent>> HandleAsync(TCommand command);
    }
}