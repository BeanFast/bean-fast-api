using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class InvalidSchoolException : BeanFastApplicationException
    {
        public InvalidSchoolException() : base(MessageConstants.SessionDetailMessageConstrant.InvalidSchoolLocation, System.Net.HttpStatusCode.BadRequest)
        {
            
        }
    }
}
