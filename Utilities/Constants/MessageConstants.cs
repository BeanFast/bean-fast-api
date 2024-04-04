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
            public static readonly string NoPermission = "Bạn không có quyền hạn để thực hiện tính năng này";

            public static string TooManyRequest(int seconds) => $"Bạn đã gửi đi quá nhiều yêu cầu, xin hãy chờ trong {seconds} giây";

        }
        public static class PaginationMessageConstrant
        {
            public const string PageRequired = "Số trang là bắt buộc";

            public const string SizeRequired = "Kích thước trang là bắt buộc";
        }
        public static class AuthorizationMessageConstrant
        {
            public static string NotAllowed = "Bạn không được phép sử dụng tính năng này";
            public static string NotLoggedInOrInvalidToken = "Bạn chưa đăng nhập hoặc access token không hợp lệ";
            public static string BannedAccount = "Tài khoản của bạn đã bị khóa!!";
            public static string DupplicatedPhone = "Số điện thoại đã tồn tại";
            public static string DupplicatedEmail = "Email đã tồn tại";
            public static string PhoneNotFound = "Không tìm thấy số điện thoại này!";
            public static string InvalidSmsOtp = "Mã xác nhận OTP không hợp lệ";
            public static string RoleNotFound = "Không tìm thấy role này";

        }

        public static class LoginMessageConstrant
        {
            #region validation messasges

            public const string PhoneOrEmailRequired = "Số điện thoại hoặc email là bắt buộc";

            public const string PhoneRequired = "Số điện thoại là bắt buộc";

            public const string EmailRequired = "Email là bắt buộc";

            public const string InvalidPhoneNumber = "Số điện thoại không hợp lệ";

            public const string InvalidEmail = "Email không hợp lệ";

            public const string InvalidPassword = "Mật khẩu phải từ 8-20 ký tự";

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

        public static class UserMessageConstrant
        {
            public static string UserNotFound(Guid guid) => $"Người dùng với id: {guid} không tồn tại";

            public const string UserEmailRequired = "Email là bắt buộc";

            public const string UserPhoneRequired = "Số điện thoại là bắt buộc";

            public const string UserPasswordRequired = "Mật khẩu là bắt buộc";

            public const string UserFullNameRequired = "Họ và tên là bắt buộc";

            public const string UserFullNameLength = "Họ và tên phải từ {2} đến {1} ký tự";

            public const string UserAddressRequired = "Địa chỉ là bắt buộc";

            public const string UserAddressLength = "Địa chỉ phải từ {2} đến {1} ký tự";

            public const string UserDobRequired = "Ngày sinh là bắt buộc";

            public const string UserDobInvalid = "Ngày sinh không hợp lệ";

            public const string UserGenderRequired = "Giới tính là bắt buộc";

            public const string UserGenderInvalid = "Giới tính không hợp lệ";
        }

        public static class AreaMessageConstrant
        {
            public static string AreaNotFound(Guid guid) => $"Khu vực với id: {guid} không tồn tại";

            //public const string AreaCodeRequird = "Mã khu vực là bắt buộc";
            //public const string AreaCodeLength = "Mã khu vực phải từ {2} đến {1} ký tự";
            public const string AreaCityRequired = "Thành phố là bắt buộc";
            public const string AreaCityLength = "Tên thành phố phải từ {2} đến {1} ký tự";
            public const string AreaDistrictRequired = "Quận/Huyện là bắt buộc";
            public const string AreaDistrictLength = "Tên quận/huyện phải từ {2} đến {1} ký tự";
            public const string AreaWardRequired = "Phường/Xã là bắt buộc";
            public const string AreaWardLength = "Tên phường/xã phải từ {2} đến {1} ký tự";

        }

        public class MenuMessageConstrant
        {
            public static string MenuNotFound(Guid guid) => $"Thực đơn với id: {guid} không tồn tại";
        }

        public class MenuDetailMessageConstrant
        {
            public static string MenuDetailNotFound(Guid guid) => $"Chi tiết thực đơn với id: {guid} không tồn tại";
        }

        public static class CategoryMessageConstrant
        {
            public const string CategoryNameExisted = "Danh mục đã tồn tại";

            public const string CategoryNotFound = "Danh mục không tồn tại";

            public const string CategoryCreateSucess = "Đã tạo danh mục thành công!";

            public const string CategoryNameRequired = "Tên danh mục là bắt buộc";
            public const string CategoryCodeRequired = "Mã danh mục là bắt buộc";
            public const string CategoryCodeLength = "Tên danh mục phải từ {2} đến {1} ký tự";
            public const string CategoryNameLength = "Mã danh mục phải từ {2} đến {1} ký tự";

        }

        public class FoodMessageConstrant
        {
            public static string FoodNotFound(Guid guid) => $"Đồ ăn với id: {guid} không tồn tại";

            public const string FoodNameRequired = "Tên đồ ăn là bắt buộc";
            public const string FoodNameLength = "Tên đồ ăn phải từ {2} đến {1} ký tự";
            public const string FoodPriceRequired = "Giá đồ ăn là bắt buộc";
            public const string FoodPriceRange = "Giá đồ ăn phải trong khoảng từ {1} đến {2}";
            public const string FoodDescriptionRequired = "Mô tả đồ ăn là bắt buộc";
            public const string FoodCategoryIdRequired = "Hãy chọn loại danh mục cho đồ ăn";
            public const string FoodQuantityRequired = "Hãy nhập số lượng đồ ăn có trong combo";
            public const string FoodQuantityRange = "Số lượng đồ ăn trong combo phải trong khoảng từ {1} đến {2}";
            public const string FoodCodeLength = "Mã đồ ăn phải từ {2} đến {1} ký tự";

        }

        public class KitchenMessageConstrant
        {
            public static string KitchenNotFound(Guid guid) => $"Bếp ăn với id: {guid} không tồn tại";
            public const string KitchenNameRequired = "Tên bếp ăn là bắt buộc";
            public const string KitchenNameLength = "Tên bếp ăn phải từ {2} đến {1} ký tự";
            public const string KitchenCodeRequired = "Mã bếp ăn là bắt buộc";
            public const string KitchenCodeLength = "Mã bếp ăn phải từ {2} đến {1} ký tự";
            public const string KitchenAddressRequired = "Đại chỉ bếp ăn là bắt buộc";
            public const string KitchenAddressLength = "Địa chỉ bếp ăn phải từ {2} đến {1} ký tự";
        }

        public class SchoolMessageConstrant
        {
            public static string SchoolAlreadyExists() => $"Đã tồn tại trường học ở địa chỉ này";

            public static string SchoolNotFound(Guid guid) => $"Trường học với id: {guid} không tồn tại";

            public const string SchoolCodeRequired = "Mã trường học là bắt buộc";
            public const string SchoolCodeLength = "Mã trường học phải từ {2} đến {1} ký tự";
            public const string SchoolNameRequired = "Tên trường học là bắt buộc";
            public const string SchoolNameLength = "Tên trường học phải từ {2} đến {1} ký tự";
            public const string SchoolAddressRequired = "Địa chỉ trường học là bắt buộc";
            public const string SchoolAddressLength = "Địa chỉ trường học tối đa 500 ký tự";

        }

        public class SessionMessageConstrant
        {
            public static string SessionNotFound(Guid guid) => $"Buổi với id: {guid} không tồn tại";
            public const string DuplicateLocationInSession = "Địa điểm đã tồn tại trong buổi này";
            public const string SessionDeliveryClosed = "Thời gian giao hàng đã đóng, không thể thực hiện thao tác này";


        }

        public class SessionDetailMessageConstrant
        {
            public const string SessionOrderClosed = "Thời gian đặt hàng đã đóng, không thể thực hiện thao tác này";
            public const string InvalidSchoolLocation = "Địa điểm không thuộc trường học mà bạn đã đăng ký";
            public static string SessionDetailNotFound(Guid guid) => $"Buổi chi tiết với id: {guid} không tồn tại";
        }

        public class LocationMessageConstrant
        {
            public static string LocationAlreadyExists() => $"Địa điểm này đã tồn tại";
            public static string LocationlNotFound(Guid guid) => $"Địa điểm với id: {guid} không tồn tại";

            public const string LocationNameRequired = "Tên địa điểm là bắt buộc";
            public const string LocationNameLength = "Tên địa điểm phải từ {2} đến {1} ký tự";
            public const string LocationDescriptionRequired = "Mô tả địa điểm là bắt buộc";

        }

        public class LoyaltyCardMessageConstrant
        {
            public static string LoyaltyCardNotFound(Guid guid) => $"Thẻ thành viên với id: {guid} không tồn tại";
            public const string QRCodeNotFound = "Không tìm thấy mã QR";
            public const string InvalidLoyaltyCard = "Thẻ thành viên không hợp lệ";
        }
        public class CardTypeMessageConstrant
        {
            public static string CardTypeNotFound(Guid guid) => $"Loại thẻ với id: {guid} không tồn tại";

            public const string CardTypeNameRequired = "Tên loại thẻ là bắt buộc";
            public const string CardTypeNameLength = "Tên loại thẻ phải từ {2} đến {1} ký tự";
            public const string CardTypeHeightRequired = "Chiều cao của thẻ là bắt buộc";
            public const string CardTypeHeightRange = "Chiều cao của thẻ phải trong khoảng từ {1} đến {2}";
            public const string CardTypeWidthRequired = "Chiều rộng của thẻ là bắt buộc";
            public const string CardTypeWidthRange = "Chiều rộng của thẻ phải trong khoảng từ {1} đến {2}";
        }
        public class FileMessageConstrant
        {
            public static string FileExtensionsOnlyAccept(IEnumerable<string> extensions) => $"Chỉ chấp nhận file có đuôi: {string.Join(", ", extensions)}";
        }
        public class GiftMessageConstrant
        {
            public static string GiftNotFound(Guid guid) => $"Quà tặng với id: {guid} không tồn tại";
            public const string GiftNameRequired = "Tên quà tặng là bắt buộc";
            public const string GiftCodeRequired = "Mã quà tặng là bắt buộc";
            public const string GiftNameLength = "Tên quà tặng phải từ {2} đến {1} ký tự";
            public const string GiftCodeLength = "Mã quà tặng phải từ {2} đến {1} ký tự";
            public const string GiftPointsRange = "Số điểm quà tặng phải lớn hơn 0";
            public const string GiftInStockRequired = "Số lượng quà tặng là bắt buộc";
            public const string GiftInStockRange = "Số lượng quà tặng phải lớn hơn 0";
        }
        public class MenuMessageContrant
        {
            public static string MenuNotFound(Guid guid) => $"Menu với id: {guid} không tồn tại";
            public const string MenuCodeRequired = "Mã menu là bắt buộc";
            public const string MenuCodeLength = "Mã menu phải từ {2} đến {1} ký tự";
            public const string MenuKitchenIdRequired = "Bếp ăn là bắt buộc";
            public const string MenuCreateDateInvalid = "Ngày tạo menu không hợp lệ";
            public const string MenuUpdateDateInvalid = "Ngày cập nhật menu không hợp lệ";
        }

        public class OrderMessageConstrant
        {
            public static string OrderNotFound(Guid guid) => $"Đơn hàng với id: {guid} không tồn tại";

            public const string OrderTotalPriceRequired = "Tổng giá đơn hàng là bắt buộc";
            public const string OrderTotalPriceRange = "Tổng giá đơn hàng phải ít nhất 1000 VNĐ";
            public const string OrderCreateDateInvalid = "Ngày tạo đơn hàng không hợp lệ";
            public const string OrderDeliveryDateInvalid = "Ngày giao đơn hàng không hợp lệ";
            public const string OrderIdRequired= "Order id là bắt buộc";
            public const string NoDeliveryOrders = "Bạn không có đơn hàng nào để giao cho khách hàng này";
        }

        public class OrderActivityMessageConstrant
        {
            public static string OrderActivityNotFound(Guid id) => $"Theo dõi hoạt động đơn hàng với id: {id} không tồn tại";
            public const string OrderCreatedActivityName = "Đơn hàng được tạo thành công";
            public const string OrderCookingActivityName = "Đơn hàng đã được nhận và đang trong quá trình chế biến";
            public const string OrderDeliveringActivityName = "Đơn hàng đang được giao";
            public const string OrderCompletedActivityName = "Đơn hàng đã được giao hoàn thành";
            public const string OrderCanceledActivityName = "Đơn hàng đã bị hủy";

            public const string DefaultExchangeGiftCreatedActivityName = "Đang chuẩn bị quà";
        }

        public class ProfileMessageConstrant
        {
            //public static string ProfileNotFound(Guid guid) => $"Hồ sơ với id: {guid} không tồn tại";

            public const string ProfileFullNameRequired = "Họ và tên là bắt buộc";
            public const string ProfileFullNameLength = "Họ và tên phải chứa tối đa {1} ký tự";
            public const string ProfileNickNameLength = "Tên biệt danh chứa tối đa {1} ký tự";
            public const string ProfileDobRequired = "Ngày sinh là bắt buộc";
            public const string ProfileDobInvalid = "Ngày sinh không hợp lệ";
            public const string ProfileClassRequired = "Tên lớp học là bắt buộc";
            public const string ProfileClassLength = "Tên lớp học phải tối đa {1} ký tự";
            public const string ProfileHeightRange = "Chiều cao phải lớn hơn 0";
            public const string ProfileWeightRange = "Cân nặng phải lớn hơn 0";
            public const string ProfileAgeRange = "Tuổi phải lớn hơn 0";
            public const string ProfileDoesNotBelongToUser = "Thông tin đứa trẻ không thuộc về người dùng này";
            public const string ProfileNotFound = "Không tìm thấy thông tin cá nhân";
        }
        public class WalletMessageConstrant
        {
            public static string WalletNotFound(Guid guid) => $"Ví với id: {guid} không tồn tại";
            public const string NotEnoughPoints = "Quý khách không có đủ số điểm để đổi món quà này!";
            public const string NotEnoughMoney = "Quý khách không có đủ tiền để thực hiện giao dịch này!";
        }
        public class ExchangeGiftMessageConstrant
        {
            public const string ExchangeGiftIdRequired = "ExchangeGiftId là bắt buộc";
        }
    }
}
