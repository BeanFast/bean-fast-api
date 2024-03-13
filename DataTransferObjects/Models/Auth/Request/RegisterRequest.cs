using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Auth.Request
{
    public class RegisterRequest
    {
        public string? FullName { get; set; }
        //public string Phone { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
        //public string AvatarPath { get; set; }
    }
}
