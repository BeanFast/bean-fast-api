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
        Task<ICollection<GetDelivererResponse>> GetAvailableDeliverersAsync(Guid sessionId);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task SendOtpAsync(string phone);
        Task UpdateCustomerAsync(UpdateCustomerRequest request, User user);
        Task<bool> VerifyOtpAsync(SmsOtpVerificationRequest request);
        Task CreateUserAsync(CreateUserRequest request);
        Task<GetCurrentUserResponse> GetCurrentUserAsync(Guid userId);
    }
}
