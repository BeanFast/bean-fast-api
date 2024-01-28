using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<RegisterResponse> Register(RegisterRequest registerRequest);
    }
}
