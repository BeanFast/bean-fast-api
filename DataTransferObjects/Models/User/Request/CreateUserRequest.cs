using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.User.Request
{
    public class CreateUserRequest
    {
        [RequiredGuid]
        public Guid RoleId { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        //public string Phone { get; set; }
        public string Email { get; set; }
        [RequiredFileExtensions(Utilities.Enums.AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
    }
}
