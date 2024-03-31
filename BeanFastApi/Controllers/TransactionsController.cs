﻿using BeanFastApi.Validators;
using DataTransferObjects.Models.VnPay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using Utilities.Enums;
using Utilities.Settings;

namespace BeanFastApi.Controllers
{

    public class TransactionsController : BaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly IVnPayService _vnPayService;
        public TransactionsController(IUserService userService, ITransactionService transactionService, IVnPayService vnPayService) : base(userService)
        {
            _transactionService = transactionService;
            _vnPayService = vnPayService;
        }
        [HttpPost("payment")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateVnPayPaymentRequest([FromQuery] int amount)
        {
            var user = await GetUserAsync();
            return SuccessResult(_transactionService.CreateVnPayPaymentRequest(user, amount, HttpContext));
        }
        [HttpGet("ipn")]
        public async Task<IActionResult> VnpayIpnEntry([FromQuery] Dictionary<string, string> queryParams)
        {
            queryParams.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(Console.WriteLine);
            await Console.Out.WriteLineAsync("12312323");
            return SuccessResult<object>(null);
        }
    }
}
