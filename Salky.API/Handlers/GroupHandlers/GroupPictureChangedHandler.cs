using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;
using Salky.WebSocket.Infra.Interfaces;


namespace Salky.API.Handlers
{
    public class GroupPictureChangedHandler : IHandler<GroupPictureChanged>
    {
        public IPoolMannager ConnectionMannager { get; }
        public GroupPictureChangedHandler(IPoolMannager connectionMannager)
        {
            ConnectionMannager = connectionMannager;
        }

        public async void Handle(GroupPictureChanged args)
        {
            await ConnectionMannager.SendToAllInPool(args.Id, new MessageServer("group/change/picture", Method.PUT, Status.Success,args));
        }
    }
}
