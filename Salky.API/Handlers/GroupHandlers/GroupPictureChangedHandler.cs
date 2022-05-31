using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;
using Salky.WebSocket.Infra.Interfaces;


namespace Salky.API.Handlers
{
    public class GroupPictureChangedHandler : IHandler<GroupPictureChanged>
    {
        public IConnectionManager ConnectionMannager { get; }
        public GroupPictureChangedHandler(IConnectionManager connectionMannager)
        {
            ConnectionMannager = connectionMannager;
        }

        public async void Handle(GroupPictureChanged args)
        {
            if (ConnectionMannager.TryGetConnectionPool(args.Id.ToString(), out var pool))
            {
                await pool.SendToAll("group/change/picture", Method.PUT, args);
            }
        }
    }
}
