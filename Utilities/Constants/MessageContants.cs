using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Constants
{
    public static class MessageContants
    {
        public static class DefaultApiMessage
        {
            public static readonly string ApiSuccess = "Thành công";

            public static readonly string ApiError = "Có lỗi xảy ra";

        }
        public static class Login
        {
            #region validation messasges

            public const string PhoneOrEmailRequired = "Số điện thoại hoặc email là bắt buộc";

            public const string PhoneRequired = "Số điện thoại là bắt buộc";

            public const string EmailRequired = "Email là bắt buộc";

            public const string InvalidPhoneNumber = "Số điện thoại không hợp lệ";

            public const string InvalidEmail = "Email không hợp lệ";

            public const string InvalidPassword = "Mật khẩu phải từ 8-20 ký tự, có ";

            #endregion

            #region error messages

            public const string InvalidCredentials = "Tài khoản hoặc mật khẩu không đúng";

            public const string InvalidRole = "Role của người dùng không hợp lệ";

            #endregion
        }
    }
}
