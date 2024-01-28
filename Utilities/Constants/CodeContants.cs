using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Constants
{
    public static class CodeContants
    {
        public static class DefaultApiCodeContants
        {
            public static readonly string ApiSuccess = "200_OK";
        }
        public static class Login
        {
            public const string InvalidCredentials = "ERR_LOGIN_001";

            public const string InvalidRole = "ERR_LOGIN_002";
        }
    }
}
