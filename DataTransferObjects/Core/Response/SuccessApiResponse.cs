using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DataTransferObjects.Core.Response
{
    
    public class SuccessApiResponse : BaseApiResponse
    {
        [DataMember(Order = -2)]
        public object? Data { get; set; }
    }
}