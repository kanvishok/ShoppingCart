using System;
using System.Threading.Tasks;
using ShoppingCart.Infrastructure.Dependencies;

namespace ShoppingCart.Infrastructure.Core
{
    public class Bus : IBus
    {
        private readonly IResolver _resolver;
        public Bus(IResolver resolver)
        {
           
            _resolver = resolver;
        }

        public async Task SendAsync<TCommand, TAggregate>(TCommand command) where TCommand : ICommand where TAggregate : IAggregateRoot
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();

            if (commandHandler == null)
            {
                throw new NullReferenceException($"No handler found for the command '{command.GetType().Name}'");
            }

           var events = await commandHandler.HandleAsync(command);

            //We can publish the events to a even bus that can be subscribed by some one else.
        }

        
    }
}
