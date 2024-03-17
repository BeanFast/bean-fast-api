using BusinessObjects.Models;
using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using DataTransferObjects.Models.User.Response;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(Guid userId);
        Task<GetDelivererResponse> GetDelivererResponseById(Guid id);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task SendOtpAsync(string phone);
    }
}
