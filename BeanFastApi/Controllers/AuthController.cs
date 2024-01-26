using DataTransferObjects.Account.Request;
using DataTransferObjects.Account.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers;

public class AuthController : BaseController
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        LoginResponse loginResponse = await _userService.Login(loginRequest);
        return SuccessResult(loginResponse);
    }
}