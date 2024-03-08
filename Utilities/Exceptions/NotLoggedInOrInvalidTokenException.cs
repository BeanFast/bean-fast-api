using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{
    public class NotLoggedInOrInvalidTokenException : BeanFastApplicationException
    {
        public NotLoggedInOrInvalidTokenException() : base(
            Constants.MessageConstants.AuthorizationMessageConstrant.NotLoggedInOrInvalidToken, 
            System.Net.HttpStatusCode.Unauthorized)
        {

        }
    }
}
