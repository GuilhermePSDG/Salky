using Microsoft.Extensions.DependencyInjection;
using Salky.Domain.Contracts;

namespace Salky.Domain
{
    public class Dispatcher : IDispatcher
    {

        private IServiceProvider provider;
        public Dispatcher(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public void Raise<Event>(Event @event) where Event : IDomainEvent
        {
            foreach (var handler in provider.GetServices<IHandler<Event>>())
            {
                handler.Handle(@event);
            }
        }
    }


}
