﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class InvalidRoleException : BeanFastApplicationException
    {
        public InvalidRoleException() : base(
                MessageConstants.AuthorizationMessageConstrant.NotAllowed,
                System.Net.HttpStatusCode.Forbidden
            )
        {
            
        }

    }
}
