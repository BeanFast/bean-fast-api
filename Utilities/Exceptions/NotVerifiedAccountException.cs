using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class NotVerifiedAccountException : BeanFastApplicationException
    {
        public NotVerifiedAccountException() : base(MessageConstants.AuthorizationMessageConstrant.NotVerifiedAccount, System.Net.HttpStatusCode.Forbidden)
        {
            
        }
    }
}
