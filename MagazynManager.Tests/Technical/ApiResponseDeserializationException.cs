using System;

namespace MagazynManager.Tests.Technical
{
    public class ApiResponseDeserializationException : Exception
    {
        public ApiResponseDeserializationException(string message) : base(message)
        {
        }

        public ApiResponseDeserializationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ApiResponseDeserializationException()
        {
        }
    }
}