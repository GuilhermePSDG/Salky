using System.Runtime.Serialization;

namespace Salky.Domain.Exceptions
{
    [Serializable]
    internal class UnableToSaveChangesException : Exception
    {
        public UnableToSaveChangesException()
        {
        }

        public UnableToSaveChangesException(string? message) : base(message)
        {
        }

        public UnableToSaveChangesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UnableToSaveChangesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}