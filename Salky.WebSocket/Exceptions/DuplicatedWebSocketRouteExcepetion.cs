using System.Runtime.Serialization;

namespace Salky.WebSocket.Infra.RoutingExceptions
{
    [Serializable]
    internal class DuplicatedRouteKeyException : Exception
    {
        public DuplicatedRouteKeyException()
        {
        }

        public DuplicatedRouteKeyException(string? message) : base(message)
        {
        }

        public DuplicatedRouteKeyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicatedRouteKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}