using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles.Request;
using DataTransferObjects.Models.Profiles.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProfileService : IBaseService
    {
        Task CreateProfileAsync(CreateProfileRequest request, Guid userId);
        Task DeleteProfileAsync(Guid id);
        Task UpdateProfileAsync(Guid id, UpdateProfileRequest request);
        Task<ICollection<GetProfileResponse>> GetProfilesByCustomerIdAsync(Guid customerId);
        Task<Profile> GetByIdAsync(Guid id);
        Task<GetProfileResponse> GetProfileResponseByIdAsync(Guid id, User user);
        Task<Profile> GetProfileByIdAndCurrentCustomerIdAsync(Guid profileId, Guid customerId);
    }
}
