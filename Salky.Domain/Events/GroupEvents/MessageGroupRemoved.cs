using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class MessageGroupRemoved : IDomainEvent
    {
        internal MessageGroupRemoved(MessageGroup messageGroup)
        {
            MessageGroup = messageGroup;
        }

        public MessageGroup MessageGroup { get; }
    }
}