using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class InvalidOtpException : BeanFastApplicationException
    {
        public InvalidOtpException() : base(statusCode: System.Net.HttpStatusCode.BadRequest, message: MessageConstants.AuthorizationMessageConstrant.InvalidSmsOtp)
        {
                
        }
    }
}
