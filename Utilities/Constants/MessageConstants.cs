using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Constants
{
    public static class MessageConstants
    {
        public static class DefaultMessageConstrant
        {
            public static readonly string ApiSuccess = "Thành công";

            public static readonly string ApiError = "Có lỗi xảy ra";

        }
        public static class PaginationMessageConstrant
        {
            public const string PageRequired = "Số trang là bắt buộc";

            public const string SizeRequired = "Kích thước trang là bắt buộc";
        }
        public static class LoginMessageConstrant
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

        public static class GuidMessageConstrant
        {
            public const string GuidRequired = "Guid là bắt buộc";

            public const string GuidNotValid = "Guid không hợp lệ";
        }
        public static class AreaMessageConstrant
        {
            public static string AreaNotFound(Guid guid) => $"Khu vực với id: {guid} không tồn tại";
        }

        public static class CategoryMessageConstrant
        {
            public const string CategoryNameExisted = "Danh mục đã tồn tại";

            public const string CategoryNotFound = "Danh mục không tồn tại";

            public const string CategoryNameRequired = "Tên danh mục là bắt buộc";

            public const string CategoryCodeOrNameExisted = "Mã danh mục hoặc tên danh mục đã tồn tại";

            public const string CategoryCreateSucess = "Đã tạo danh mục thành công!";
        }

        public class FoodMessageConstrant
        {
            public static string FoodNotFound (Guid guid) => $"Đồ ăn với id: {guid} không tồn tại";
        }

        public class KitchenMessageConstrant
        {
            public static string KitchenNotFound(Guid guid) => $"Bếp ăn với id: {guid} không tồn tại";
        }

        public class SchoolMessageConstrant
        {
            public static string SchoolAlreadyExists() => $"Đã tồn tại trường học ở địa chỉ này";

            public static string SchoolNotFound(Guid guid) => $"Trường học với id: {guid} không tồn tại";
        }
    }
}
