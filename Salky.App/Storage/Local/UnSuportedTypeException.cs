using System.Runtime.Serialization;

namespace Salky.WebSockets.Exensions
{
    [Serializable]
    internal class UnSuportedTypeException : Exception
    {
        public UnSuportedTypeException()
        {
        }

        public UnSuportedTypeException(string? message) : base(message)
        {
        }

        public UnSuportedTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnSuportedTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}