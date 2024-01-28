using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataTransferObjects.Core.Response
{
    public class BaseApiResponse
    {
        public BaseApiResponse(HttpStatusCode httpStatusCode)
        {
            StatusCode = (int)httpStatusCode;
        }
        public BaseApiResponse()
        {
            
        }


        public string? Message { get; set; }
       
        public string? Code { get; set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }

        public void SetStatusCode (HttpStatusCode httpStatusCode)
        {
            StatusCode = (int)httpStatusCode;
        }
        
        public DateTime Time { get; set; } = DateTime.UtcNow;


    }
}
