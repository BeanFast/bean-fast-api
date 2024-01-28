using System.Net;

namespace DataTransferObjects.Core.Response
{
    public class ErrorApiResponse : BaseApiResponse
    {

        public ErrorApiResponse(HttpStatusCode statusCode) : base(statusCode) 
        {
            
        }
        public ErrorApiResponse()
        {
            
        }


        //public string Description { get; set; }
    }
}
