using System;
using System.Runtime.Serialization;

namespace MagazynManager.Application
{
    public class BussinessException : Exception
    {
        public BussinessException(string message) : base(message)
        {
        }

        public BussinessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BussinessException()
        {
        }

        protected BussinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}