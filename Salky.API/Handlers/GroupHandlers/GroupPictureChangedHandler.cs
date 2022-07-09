using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;

namespace Salky.API.Handlers
{
    public class GroupPictureChangedHandler : IHandler<GroupPictureChanged>
    {
        private readonly IConnectionPoolMannager connectionMannager;
        public GroupPictureChangedHandler(IConnectionPoolMannager connectionMannager)
        {
            this.connectionMannager = connectionMannager;
        }

        public async void Handle(GroupPictureChanged args)
        {
            await this.connectionMannager
                .SendToAll(args.Id.ToString(), new MessageServer("group/change/picture", Method.PUT, Status.Success, args));
        }
    }
}
