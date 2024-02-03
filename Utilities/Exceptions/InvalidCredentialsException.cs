using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class InvalidCredentialsException : BeanFastApplicationException
    {
        public InvalidCredentialsException(string message = MessageContants.Login.InvalidCredentials, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
        {

        }

        
    }
}
