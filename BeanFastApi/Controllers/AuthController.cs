﻿using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        LoginResponse loginResponse = await _userService.LoginAsync(loginRequest);
        return SuccessResult(loginResponse);
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest registerRequest)
    {
        RegisterResponse registerResponse = await _userService.RegisterAsync(registerRequest);
        return SuccessResult(registerResponse);
    }
    [HttpGet("otp")]
    [EnableRateLimiting("otpRateLimit")]
    public async Task<IActionResult> SendOtpAsync([FromBody] string phone)
    {
        await _userService.SendOtpAsync(phone);
        return SuccessResult("OK");
    }
    //public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    //{
    //    RegisterResponse registerResponse = await _userService.Register(registerRequest);
    //    return SuccessResult(registerResponse);
    //}
}