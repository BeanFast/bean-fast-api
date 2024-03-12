using DataTransferObjects.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProfileService
    {
        Task CreateProfileAsync(CreateProfileRequest request, Guid userId);
        Task DeleteProfileAsync(Guid id);
        Task UpdateProfileAsync(Guid id, UpdateProfileRequest request);
    }
}
