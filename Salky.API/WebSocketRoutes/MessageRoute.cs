using Salky.App.Dtos.Message;
using Salky.App.Services;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;

namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute]
    public class MessageRoute : WebSocketRouteBase
    {
        private MessageService messageService;

        public MessageRoute(MessageService messageService)
        {
            this.messageService = messageService;
        }



        [WsGet]
        public async void GetMessages(SalkyWebSocket salkyWebSocket, Guid contactid)
        {
            try
            {
                var msgs = await messageService.GetMessagesByContactId(salkyWebSocket.user.Id, contactid);
                await SendBack(salkyWebSocket, msgs, "user", Method.POST);
            }
            catch (Exception ex)
            {
                await SendErrorBack(salkyWebSocket, ex);
            }
        }


        [WsRedirect]
        public async void SendMessage(SalkyWebSocket salkyWebSocket, MessageAddDTO msg)
        {
            try
            {
                var messages = await messageService.AdicionarMensagemParaAmbos(salkyWebSocket.user.Id, msg.ContactId, msg.Content);
                var res = await SendTo(salkyWebSocket, msg.UserContactId, messages.MessageSender,"message", Method.POST);
               
                await this.messageService.UpdateMessageStatus(messages.MessageReceiver.UserReceiverId , messages.MessageReceiver.Id ?? throw new("Message.Id Cannot be null"), res);
                await this.messageService.UpdateMessageStatus(salkyWebSocket.user.Id, messages.MessageSender.Id ?? throw new("Message.Id Cannot be null"), res);
               
                await SendBack(salkyWebSocket, messages.MessageReceiver, "message", Method.POST);
            }
            catch(Exception ex)
            {
                await SendErrorBack(salkyWebSocket, ex);
            }
        }

    }
}
