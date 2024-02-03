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
        public HttpStatusCode StatusCode { get; set; }

        public BeanFastApplicationException(HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base()
        {
            StatusCode = statusCode;
        }
        public BeanFastApplicationException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            StatusCode = statusCode;
        }

    }
}
