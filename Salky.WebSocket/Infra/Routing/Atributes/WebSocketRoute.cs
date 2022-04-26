namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class WebSocketRoute : Attribute
    {
        public string? routeName = null;
        public WebSocketRoute() { }
        public WebSocketRoute(string routeName)
        {
            this.routeName = routeName;
        }
    }
}
