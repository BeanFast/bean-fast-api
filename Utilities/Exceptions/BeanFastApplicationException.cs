using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{
    public class BeanFastApplicationException : Exception
    {
        public string? Code { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public BeanFastApplicationException(string? code = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base()
        {
            Code = code;
            StatusCode = statusCode;
        }
        public BeanFastApplicationException(string message, string? code = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

    }
}
