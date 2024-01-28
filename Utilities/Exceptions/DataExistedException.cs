using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{
    public class DataExistedException : BeanFastApplicationException
    {
        public DataExistedException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : base(message, httpStatusCode)
        {
        }

    }
}
