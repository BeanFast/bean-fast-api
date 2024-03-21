using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class ClosedSessionException : BeanFastApplicationException
    {
        public ClosedSessionException() : base(
            MessageConstants.SessionDetailMessageConstrant.SessionOrderClosed, 
            System.Net.HttpStatusCode.BadRequest
            )
        {

        }
    }
}
