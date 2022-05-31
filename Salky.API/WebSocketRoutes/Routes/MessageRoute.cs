using Salky.App.Dtos.Message;
using Salky.App.Services.Group;

namespace Salky.API.WebSocketRoutes.Routes
{
    [WebSocketRoute("group/message")]
    public class MessageRoute : WebSocketRouteBase
    {
        private GroupMessageService messageService;
        private readonly GroupService groupService;

        public MessageRoute(GroupMessageService messageService, GroupService groupService)
        {
            this.messageService = messageService;
            this.groupService = groupService;
        }

        [WsRedirect]
        public async Task SendMessage(MessageAddDTO msg)
        {
            try
            {
                var currentUsrId = Claims.GetUserId();
                var msgResult = await messageService.AddMessage(currentUsrId, msg.GroupId, msg.Content);
                if (msgResult == null)
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel adicionar a mensagem");
                    return;
                }
                var pool = GetPool(msgResult.GroupId.ToString());
                if (pool != null)
                {
                    await pool.SendToAll(CurrentPath, Method.POST, msgResult);
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao fazer o envio da mensagem");
            }
        }


        [WsDelete]
        public async void RemoveMessage(Guid messageId)
        {
            try
            {
                var removedMsg = await messageService.RemoveMessage(Claims.GetUserId(), messageId);
                if (removedMsg != null)
                {
                    var pool = GetPool(removedMsg.GroupId.ToString());
                    await pool.SendToAll(CurrentPath, Method.DELETE, new
                    {
                        removedMsg.GroupId,
                        MessageId = removedMsg.Id
                    });
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel remover a mensagem");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao remover a mensagem");
            }
        }





    }
}
