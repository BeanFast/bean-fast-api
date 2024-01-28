using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Exceptions
{
    public class EntityNotFoundException : BeanFastApplicationException
    {
        public EntityNotFoundException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.NotFound) : base(message, httpStatusCode)
        {
            
        }
    }
}
