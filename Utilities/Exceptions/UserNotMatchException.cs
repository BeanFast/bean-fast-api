using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class UserNotMatchException : BeanFastApplicationException
    {
        public UserNotMatchException() : base(MessageConstants.AuthorizationMessageConstrant.NotAllowed, HttpStatusCode.BadRequest)
        {
        }
    }
}
