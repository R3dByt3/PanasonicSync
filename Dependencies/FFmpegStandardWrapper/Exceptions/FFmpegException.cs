using System;
using System.Runtime.Serialization;

namespace FFmpegStandardWrapper.Exceptions
{
    [Serializable]
    internal class FFmpegException : Exception
    {
        public FFmpegException()
        {
        }

        public FFmpegException(string message) : base(message)
        {
        }

        public FFmpegException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FFmpegException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}