﻿using BeanFastApi.Validators;
using DataTransferObjects.Models.LoyaltyCard.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net;

namespace BeanFastApi.Controllers
{
    public class LoyaltyCardsController : BaseController
    {
        private readonly ILoyaltyCardService _loyaltyCardService;
        public LoyaltyCardsController(IUserService userService, ILoyaltyCardService loyaltyCardService) : base(userService)
        {
            _loyaltyCardService = loyaltyCardService;
        }
        [HttpPost]
        [Authorize(Utilities.Enums.RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateLoyaltyCardAsync(CreateLoyaltyCardRequest request)
        {
            await _loyaltyCardService.CreateLoyaltyCard(request, await GetUserAsync());
            return SuccessResult<object>(HttpStatusCode.Created);
        }
        //[HttpGet] async Task<IActionResult> GetLoyaltyCardAsync()
        //{

        //}
    }
}
