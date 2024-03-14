using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class TooManyRequestException : BeanFastApplicationException
    {
        public TooManyRequestException(int seconds) : base(MessageConstants.DefaultMessageConstrant.TooManyRequest(seconds), System.Net.HttpStatusCode.TooManyRequests)
        {

        }
    }
}
