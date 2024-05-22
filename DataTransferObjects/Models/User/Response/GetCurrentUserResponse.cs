using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.User.Response
{
    public class GetCurrentUserResponse
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public double Balance { get; set; }
        public double Points { get; set; }

    }
}
