using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{
    public class InvalidRequestException : BeanFastApplicationException
    {
        public InvalidRequestException(string message) : base(message, System.Net.HttpStatusCode.BadRequest)
        {
            
        }
    }
}
