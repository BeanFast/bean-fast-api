using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Account.Response
{
    public class LoginResponse
    {
        public string? RefreshToken { get; set; }

        public string? AccessToken { get; set; }
    }
}
