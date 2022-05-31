using Salky.API.WebSocketRoutes.Models;
using Salky.Domain.Contracts;
using Salky.Domain.Events.GroupEvents;
using Salky.WebSocket.Infra;
using Salky.WebSocket.Infra.Interfaces;

namespace Salky.API.Handlers.GroupHandlers
{
    public class GroupMemberRemovedHandler : IHandler<GroupMemberRemoved>
    {
        public GroupMemberRemovedHandler(IConnectionManager connectionMannager)
        {
            ConnectionMannager = connectionMannager;
        }

        public IConnectionManager ConnectionMannager { get; }

        public async void Handle(GroupMemberRemoved args)
        {
            try
            {
                if(ConnectionMannager.TryGetSocket(args.groupMember.UserId.ToString(), out var memberSock))
                {
                    if (memberSock.Storage.TryGet<GroupMemberCall>(out var usrCall))
                    {
                        if (usrCall.IsInCall && usrCall.CurrentCallPath != null)
                        {
                            if (ConnectionMannager.TryGetConnectionPool(usrCall.CurrentCallPath, out var pool))
                                await pool.SendToAll(
                                    path: $"group/call",
                                    method: Method.DELETE,
                                    data: usrCall
                                    );
                            usrCall.ZeroCallProperties();
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
