using BeanFastApi.Validators;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Notification.Request;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.VnPay.Request;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
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
        private readonly IUnitOfWork<BeanFastContext> unitOfWork;
        private readonly ISessionService _sessionService;
        public TestsController(IUserService userService,
            INotificationService notificationService,
            IVnPayService vnPayService,
            ISessionService sessionService,
            IUnitOfWork<BeanFastContext> unitOfWork) : base(userService)
        {
            _notificationService = notificationService;
            _vnPayService = vnPayService;
            _sessionService = sessionService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationAsync([FromBody] CreateNotificationRequest request)
        {
            await _notificationService.SendNotificationAsync(request);
            return SuccessResult(new object());
        }
        [HttpGet("delay")]
        public async Task<IActionResult> ChangeBackgroundJobDeplay([FromQuery] int delay)
        {
            BackgroundServiceConstrant.DelayedInMinutes = delay;
            return SuccessResult(new object());
        }
        [HttpGet("background-job")]
        public async Task<IActionResult> RunBackgroundJob()
        {
            await _sessionService.UpdateOrdersStatusAutoAsync();
            return SuccessResult(new object());
        }
        [HttpGet("test")]
        public async Task <IActionResult> Test()
        {
            object obj = new();
            obj = await unitOfWork.GetRepository<SessionDetailDeliverer>().CountAsync();
            return SuccessResult(obj);
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
