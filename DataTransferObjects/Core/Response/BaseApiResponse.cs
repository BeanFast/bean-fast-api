using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataTransferObjects.Core.Response
{
    public class BaseApiResponse
    {
        
        public required string Message { get; set; }
       
        public required string Code { get; set; }
        
        public DateTime Time { get; set; } = DateTime.UtcNow;


    }
}
