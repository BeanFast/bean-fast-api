using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Settings;

namespace BeanFastApi.Controllers;

public class AuthController : BaseController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost(ApiEndpointConstants.Auth.Login)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        LoginResponse loginResponse = await _userService.LoginAsync(loginRequest);
        return SuccessResult(loginResponse);
    }
    [HttpPost(ApiEndpointConstants.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        RegisterResponse registerResponse = await _userService.RegisterAsync(registerRequest);
        return SuccessResult(registerResponse);
    }
    //public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    //{
    //    RegisterResponse registerResponse = await _userService.Register(registerRequest);
    //    return SuccessResult(registerResponse);
    //}
}