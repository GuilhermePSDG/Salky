﻿using Salky.App.Dtos.Message;
using Salky.App.Services.Group;
using Salky.WebSockets.Router.Routing;
using Salky.WebSockets.Router.Routing.Atributes;

namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute("group/message")]
    public class MessageRoute : WebSocketRouteBase
    {
        private GroupMessageService messageService;

        public MessageRoute(GroupMessageService messageService)
        {
            this.messageService = messageService;
        }

        [WsRedirect]
        public async Task SendMessage(MessageAddDTO msg)
        {
            try
            {
                var msgResult = await messageService.AddMessage(Guid.Parse(User.UserId), msg.GroupId, msg.Content);
                if (msgResult == null)
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel adicionar a mensagem");
                    return;
                }
                var n = await SendToAllInPool(msgResult.GroupId, CurrentPath, Method.POST, msgResult);
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
                    await SendToAllInPool(removedMsg.GroupId, CurrentPath, Method.DELETE, new
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
