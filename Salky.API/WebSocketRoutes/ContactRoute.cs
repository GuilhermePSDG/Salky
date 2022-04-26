namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute]
    public class ContactRoute : WebSocketRouteBase
    {
        public ContactService contactService { get; }
        public ContactRoute(ContactService contactService)
        {
            this.contactService = contactService;
        }

        [WsGet]
        public async void GetContacts(SalkyWebSocket ws)
        {
            try
            {
                var usrId = ws.user.Id;
                var usrContacs = await contactService.GetAllAsync(usrId);
                await SendBack(ws, usrContacs, "contact", Method.POST);
            }
            catch (Exception ex)
            {
                await SendErrorBack(ws, ex);
            }
        }
    }
}
