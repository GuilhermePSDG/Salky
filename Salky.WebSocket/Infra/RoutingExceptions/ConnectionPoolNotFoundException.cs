using System.Runtime.Serialization;

namespace Salky.WebSocket.Infra.RoutingExceptions
{
    [Serializable]
    internal class ConnectionPoolNotFoundException : Exception
    {
        public ConnectionPoolNotFoundException()
        {
        }

        public ConnectionPoolNotFoundException(string? message) : base(message)
        {
        }

        public ConnectionPoolNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ConnectionPoolNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}