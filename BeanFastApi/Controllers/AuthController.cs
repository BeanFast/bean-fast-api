﻿using BeanFastApi.Validators;
using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;
using DataTransferObjects.Models.SmsOtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Settings;

namespace BeanFastApi.Controllers;

public class AuthController : BaseController
{

    public AuthController(IUserService userService) : base(userService)
    {

    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        LoginResponse loginResponse = await _userService.LoginAsync(loginRequest);
        return SuccessResult(loginResponse);
    }
    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        LoginResponse loginResponse = await _userService.RefreshTokenAsync(request.RefreshToken, await GetUserAsync());
        return SuccessResult(loginResponse);
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest registerRequest)
    {
        RegisterResponse registerResponse = await _userService.RegisterAsync(registerRequest);
        return SuccessResult(registerResponse);
    }
    [HttpPost("otp")]
    //[EnableRateLimiting("otpRateLimit")]
    public async Task<IActionResult> SendOtpAsync([FromBody] SendOtpRequest request)
    {
        await _userService.SendOtpAsync(request.Phone);
        return SuccessResult("OK");
    }
    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var userId = GetUserId();
        return SuccessResult(await _userService.GetCurrentUserAsync(userId));
    }
    [HttpPost("otp/verify")]
    //[EnableRateLimiting("otpRateLimit")]
    public async Task<IActionResult> VerifyOtpAsync([FromBody] SmsOtpVerificationRequest request)
    {
        await _userService.VerifyOtpAsync(request);
        return SuccessResult("OK");
    }
    //public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    //{
    //    RegisterResponse registerResponse = await _userService.Register(registerRequest);
    //    return SuccessResult(registerResponse);
    //}
}