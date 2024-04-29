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
            var user = await GetUserAsync();

            await _profileService.CreateProfileAsync(request, user.Id , user);
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
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProfileByIdAsync([FromRoute] Guid id)
        {
            var user = await GetUserAsync();    
            var profile = await _profileService.GetProfileResponseByIdAsync(id, user);
            return SuccessResult(profile);
        }
        [HttpPut("{id}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> UpdateProfileAsync([FromRoute] Guid id, [FromForm] UpdateProfileRequest request)
        {
            await _profileService.UpdateProfileAsync(id, request, await GetUserAsync());
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
