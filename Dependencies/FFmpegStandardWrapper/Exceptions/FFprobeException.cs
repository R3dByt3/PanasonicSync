using System;
using System.Runtime.Serialization;

namespace FFmpegStandardWrapper.Exceptions
{
    [Serializable]
    internal class FFprobeException : Exception
    {
        public FFprobeException()
        {
        }

        public FFprobeException(string message) : base(message)
        {
        }

        public FFprobeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FFprobeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}