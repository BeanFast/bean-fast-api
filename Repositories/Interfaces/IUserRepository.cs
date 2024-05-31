using BusinessObjects.Models;
using DataTransferObjects.Models.Auth.Response;
using DataTransferObjects.Models.User.Request;
using DataTransferObjects.Models.User.Response;

namespace Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetByIdAsync(Guid userId);
    Task<User> GetCustomerByQrCodeAsync(string qrCode);
    Task<ICollection<GetDelivererResponse>> GetDeliverersExcludeAsync(List<Guid> excludeDelivererIds);
    Task<ICollection<GetDelivererResponse>> GetDeliverersAsync();
    Task<User> LoginAsync(LoginRequest loginRequest);
    Task<User> GetUserByIdIncludeWallet(Guid userId);
    Task<ICollection<GetUserResponse>> GetAllAsync(UserFilterRequest request); 
    Task<GetUserResponse> GetUserResponseByIdAsync(Guid userId);
    Task<User> FindNotVerifiedUserByPhone(string phone);
    Task<User?> FindUserByPhone(string phone);
    Task<User> GetManagerByIdAsync(Guid managerId);
    Task<ICollection<GetUserResponse>> GetKitchenManagerHasNoKitchen();
}