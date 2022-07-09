using Salky.API.Models;
using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;
using Salky.WebSockets.Contracts;

namespace Salky.API.Handlers.GroupHandlers
{
    public class GroupMemberRemovedHandler : IHandler<GroupMemberRemoved>
    {
        public GroupMemberRemovedHandler(ILogger<GroupMemberRemovedHandler> logger, IConnectionPoolMannager connectionMannager)
        {
            this.logger = logger;
            this.connectionMannager = connectionMannager;
        }

        private readonly ILogger<GroupMemberRemovedHandler> logger;
        private readonly IConnectionPoolMannager connectionMannager;

        public void Handle(GroupMemberRemoved args)
        {
            try
            {
                this.logger.LogError(new NotImplementedException(), "");

                //if (!ConnectionMannager.TryGetSocket(args.groupMember.UserId.ToString(), out var memberSock)) return;
                //if (!memberSock.Storage.TryGet<GroupMemberCall>(out var usrCall)) return;
                //if (!(usrCall.IsInCall && usrCall.PoolPath != null && usrCall.PoolPath.Contains(args.groupMember.GroupId.ToString()))) return;
                //await ConnectionMannager.SendToAllInPool(usrCall.PoolPath,new MessageServer($"group/call", Method.DELETE,Status.Success, usrCall));
                //usrCall.ZeroCallProperties();
            }
            catch
            {
            }
        }
    }
}
