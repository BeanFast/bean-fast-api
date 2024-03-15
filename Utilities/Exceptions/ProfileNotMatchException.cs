﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Exceptions
{
    public class ProfileNotMatchException : BeanFastApplicationException
    {
        public ProfileNotMatchException(string message = MessageConstants.ProfileMessageConstrant.ProfileDoesNotBelongToUser, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
        {
        }
    }
}