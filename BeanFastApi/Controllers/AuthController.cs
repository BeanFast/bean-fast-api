using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Constants;

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
        LoginResponse loginResponse = await _userService.Login(loginRequest);
        return SuccessResult(loginResponse);
    }
    [HttpPost(ApiEndpointConstants.Auth.Register)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        RegisterResponse registerResponse = await _userService.Register(registerRequest);
        return SuccessResult(registerResponse);
    }
    //public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    //{
    //    RegisterResponse registerResponse = await _userService.Register(registerRequest);
    //    return SuccessResult(registerResponse);
    //}
}