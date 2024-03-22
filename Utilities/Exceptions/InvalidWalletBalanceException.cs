using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;

namespace Utilities.Exceptions
{
    public class InvalidWalletBalanceException : BeanFastApplicationException
    {
        public InvalidWalletBalanceException(string message) : base(message, System.Net.HttpStatusCode.BadRequest)
        {
            
        }
    }
}
