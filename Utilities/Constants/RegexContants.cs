using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Constants
{
    public static class RegexContants
    {
        public const string PhoneRegex = @"/^((\+){0,1}((841[0-9]{9})|(849[0-9]{8})))$|^(09[0-9]{8})$|^(01[0-9]{9})$/gm";
        public const string EmailRegex = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
        public const string PasswordRegex = @"/^^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,20}$/gm";
    }
}
