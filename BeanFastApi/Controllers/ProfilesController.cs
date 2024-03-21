using BeanFastApi.Validators;
using DataTransferObjects.Models.Profiles.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class ProfilesController : BaseController
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService, IUserService userService) : base(userService)
        {
            _profileService = profileService;
        }

        [HttpPost]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateProfileAsync([FromForm] CreateProfileRequest request)
        {
            var uid = GetUserId();

            await _profileService.CreateProfileAsync(request, uid);
            return SuccessResult<object>(statusCode: System.Net.HttpStatusCode.Created);
        }
        [HttpGet]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> GetProfilesByCurrentCustomerAsync()
        {
            var uid = GetUserId();
            var profiles = await _profileService.GetProfilesByCustomerIdAsync(uid);
            return SuccessResult(profiles);
        }
        [HttpPut("{id}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> UpdateProfileAsync([FromRoute] Guid id, [FromForm] UpdateProfileRequest request)
        {
            await _profileService.UpdateProfileAsync(id, request);
            return SuccessResult<object>();
        }
        [HttpDelete("{id}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> DeleteProfileAsync([FromRoute] Guid id)
        {
            await _profileService.DeleteProfileAsync(id);
            return SuccessResult<object>();
        }
    }
}
