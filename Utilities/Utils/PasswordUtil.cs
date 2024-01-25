using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Utilities.Util
{
    public static class PasswordUtil
    {
        public static string HashPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }
        public static bool VerifyPassword(string rawPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hashPassword);
        }
    }
}
