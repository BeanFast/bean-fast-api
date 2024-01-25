using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Account.Request
{
    public class LoginRequest
    {
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
}
