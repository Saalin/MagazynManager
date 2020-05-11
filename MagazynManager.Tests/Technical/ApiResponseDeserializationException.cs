using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static MagazynManager.Tests.Technical.JsonSerializerUtils;

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