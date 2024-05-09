using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles.Response;

namespace Repositories.Interfaces;

public interface IProfileRepository : IGenericRepository<Profile>
{
    Task<Profile> GetProfileByIdAndCurrentCustomerIdAsync(Guid profileId, Guid customerId);
    Task<GetProfileResponse> GetProfileResponseByIdAsync(Guid id, User user);
    Task<Profile> GetByIdAsync(Guid id);
    Task<ICollection<GetProfileResponse>> GetProfilesByCustomerIdAsync(Guid customerId);
    Task<Profile> GetProfileByIdForUpdateProfileAsync(Guid id);
    Task<Profile> GetProfileByIdAsync(Guid id);
    Task<Profile> GetProfileByIdAsync(int status, Guid id);
}