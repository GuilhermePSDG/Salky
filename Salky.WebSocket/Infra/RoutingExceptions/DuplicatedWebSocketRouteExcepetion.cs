using System.Runtime.Serialization;

namespace Salky.WebSocket.Infra.RoutingExceptions
{
    [Serializable]
    internal class DuplicatedWebSocketRouteExcepetion : Exception
    {
        public DuplicatedWebSocketRouteExcepetion()
        {
        }

        public DuplicatedWebSocketRouteExcepetion(string? message) : base(message)
        {
        }

        public DuplicatedWebSocketRouteExcepetion(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicatedWebSocketRouteExcepetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}