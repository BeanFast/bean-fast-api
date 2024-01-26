using DataTransferObjects.Account.Request;
using DataTransferObjects.Account.Response;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
