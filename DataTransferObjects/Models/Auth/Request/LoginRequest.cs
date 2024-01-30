using System.ComponentModel.DataAnnotations;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Auth.Response
{
    //[PhoneOrEmailRequired]
    public class LoginRequest
    {
        [RegularExpression(RegexContants.PhoneRegex, ErrorMessage = MessageContants.Login.InvalidPhoneNumber)]
        public string? Phone { get; set; }
        //[RegularExpression(RegexContants.PasswordRegex, ErrorMessage = MessageContants.Login.InvalidPassword)]
        public string Password { get; set; } = null!;
        [RegularExpression(RegexContants.EmailRegex, ErrorMessage = MessageContants.Login.InvalidEmail)]
        //[CustomEmailAddress]
        public string? Email { get; set; }
        public RoleName Role { get; set; }
    }
}
