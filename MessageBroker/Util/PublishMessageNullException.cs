using System;
using System.Runtime.Serialization;

namespace MessageBroker.Util
{
    [Serializable]
    internal class PublishMessageNullException : Exception
    {
        private const string ParamName = "message";

        public PublishMessageNullException()
        {
            throw new ArgumentException("Message cannot be null", ParamName);
        }

        public PublishMessageNullException(string message) : base(message)
        {
        }

        public PublishMessageNullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PublishMessageNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}