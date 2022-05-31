using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.Domain.Contracts
{
    public interface IDispatcher
    {
        public void Raise<Event>(Event @event) where Event : IDomainEvent;
    }
}
