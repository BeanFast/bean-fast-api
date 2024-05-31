using BusinessObjects.Models;
using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using DataTransferObjects.Models.SmsOtp;
using DataTransferObjects.Models.User.Request;
using DataTransferObjects.Models.User.Response;

namespace Services.Interfaces
{
    public interface IUserService : IBaseService
    {
        Task<User> GetByIdAsync(Guid userId);
        Task<User> GetManagerByIdAsync(Guid managerId);
        Task<GetUserResponse> GetUserResponseByIdAsync(Guid userId);
        Task<ICollection<GetUserResponse>> GetAllAsync(UserFilterRequest request);
        Task<ICollection<GetUserResponse>> GetKitchenManagerHasNoKitchen();
        //Task<ICollection<GetDelivererResponse>> GetAvailableDeliverersAsync(Guid sessionId);
        Task<ICollection<GetDelivererResponse>> GetDeliverersExcludeAsync(List<Guid> excludeDelivererIds);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<LoginResponse> RefreshTokenAsync(string refreshToken, User user);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task SendOtpAsync(string phone);
        Task UpdateCustomerAsync(UpdateCustomerRequest request, User user);
        Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request);
        Task CreateUserAsync(CreateUserRequest request);
        Task<GetCurrentUserResponse> GetCurrentUserAsync(Guid userId);
        Task<ICollection<GetDelivererResponse>> GetDeliverersAsync();
        Task<GenerateQrCodeResponse> GenerateQrCodeAsync(User user);
        Task<User> GetCustomerByQrCodeAsync(string qrCode);
        Task UpdateUserStatusAsync(Guid id, UpdateUserStatusRequest request);
    }
}
