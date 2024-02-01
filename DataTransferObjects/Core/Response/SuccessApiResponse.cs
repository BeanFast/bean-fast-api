using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataTransferObjects.Core.Response
{
    
    public class SuccessApiResponse<T> : BaseApiResponse
    {
        public SuccessApiResponse(HttpStatusCode statusCode) : base(statusCode)
        {
            
        }
        public SuccessApiResponse()
        {
            
        }
        public T? Data { get; set; }
    }
}