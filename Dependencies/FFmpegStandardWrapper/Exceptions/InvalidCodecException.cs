using System;
using System.Runtime.Serialization;

namespace FFmpegStandardWrapper.Exceptions
{
    [Serializable]
    internal class InvalidCodecException : Exception
    {
        public InvalidCodecException()
        {
        }

        public InvalidCodecException(string message) : base(message)
        {
        }

        public InvalidCodecException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCodecException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}