﻿using DataTransferObjects.Models.Auth.Request;
using DataTransferObjects.Models.Auth.Response;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task SendOtpAsync(string phone);
        Task VerifyOtpAsync(string phone, string otp);
    }
}
