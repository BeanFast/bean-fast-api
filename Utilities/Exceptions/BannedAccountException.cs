using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class BannedAccountException : BeanFastApplicationException
    {
        public BannedAccountException() : base(MessageConstants.AuthorizationMessageConstrant.BannedAccount, System.Net.HttpStatusCode.Forbidden)
        {
        
        }
    }
}
