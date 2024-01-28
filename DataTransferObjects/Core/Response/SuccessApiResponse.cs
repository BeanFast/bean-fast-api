using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataTransferObjects.Core.Response
{
    
    public class SuccessApiResponse : BaseApiResponse
    {
        public SuccessApiResponse(HttpStatusCode statusCode) : base(statusCode)
        {
            
        }
        public SuccessApiResponse()
        {
            
        }
        [DataMember(Order = -2)]
        public object? Data { get; set; }
    }
}