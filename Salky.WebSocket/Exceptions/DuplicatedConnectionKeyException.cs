using System.Runtime.Serialization;

namespace Salky.WebSocket.Exceptions
{
    [Serializable]
    internal class DuplicatedConnectionKeyException : Exception
    {
        public DuplicatedConnectionKeyException()
        {
        }

        public DuplicatedConnectionKeyException(string? message) : base(message)
        {
        }

        public DuplicatedConnectionKeyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicatedConnectionKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}