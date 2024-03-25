using BeanFastApi.Validators;
using BusinessObjects.Models;
using DataTransferObjects.Models.Notification.Request;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.VnPay.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Utils;

namespace BeanFastApi.Controllers
{
    public class TestsController : BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IVnPayService _vnPayService;
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;
        public TestsController(IUserService userService,
            INotificationService notificationService,
            IVnPayService vnPayService,
            IWalletService walletService,
            ITransactionService transactionService) : base(userService)
        {
            _notificationService = notificationService;
            _vnPayService = vnPayService;
            _walletService = walletService;
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationAsync([FromBody] CreateNotificationRequest request)
        {
            await _notificationService.SendNotificationAsync(request);
            return SuccessResult(new object());
        }

        [HttpPost("payment")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> topUp([FromQuery] double amount)
        {
           var user = await GetUserAsync();
            var wallet = user.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type));
            var vnPayEntity = new VnPayRequest
            {
                WalletId = wallet!.Id,
                Description = "Nap tien",
                Amount = amount,
                CreatedDate = DateTime.Now,
                FullName = user.FullName!,
            };
            //wallet.Balance += amount;

            //var transaction = new CreateTransactionRequest
            //{
            //    OrderId = null,
            //    ExchangeGiftId = null,
            //    WalletId = wallet.Id,
            //    Value = amount,
            //    Time = DateTime.Now
            //};
            return SuccessResult(_vnPayService.CreatePaymentUrl(HttpContext, vnPayEntity));
        }

    }
}
