using Salky.API.WebSocketRoutes.Models;
using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;
using Salky.WebSocket.Infra;
using Salky.WebSocket.Infra.Interfaces;

namespace Salky.API.Handlers.GroupHandlers
{
    public class GroupMemberRemovedHandler : IHandler<GroupMemberRemoved>
    {
        public GroupMemberRemovedHandler(IPoolMannager connectionMannager)
        {
            ConnectionMannager = connectionMannager;
        }

        public IPoolMannager ConnectionMannager { get; }

        public async void Handle(GroupMemberRemoved args)
        {
            try
            {
                if (!ConnectionMannager.TryGet(args.groupMember.UserId.ToString(), out var memberSock)) return;
                if (!memberSock.Storage.TryGet<GroupMemberCall>(out var usrCall)) return;
                if (!(usrCall.IsInCall && usrCall.PoolPath != null && usrCall.PoolPath.Contains(args.groupMember.GroupId.ToString()))) return;
                await ConnectionMannager.SendToAllInPool(usrCall.PoolPath,$"group/call", Method.DELETE, usrCall);
                usrCall.ZeroCallProperties();
            }
            catch
            {

            }
        }
    }
}
