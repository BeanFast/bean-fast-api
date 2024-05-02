using BeanFastApi.Validators;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Transaction.Request;
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
            var param = queryParams.FirstOrDefault(i => i.Key.Equals("vnp_OrderInfo"));
            if (param.Value != null)
            {
                var walletId = param.Value.Split(":")[1];
                var amount = queryParams.FirstOrDefault(i => i.Key.Equals("vnp_Amount")).Value;
                await _transactionService.CreateTopUpTransactionAsync(walletId, amount);
                Console.WriteLine(walletId);
            }
            await Console.Out.WriteLineAsync("12312323");
            return SuccessResult<object>(null);
        }
        [HttpGet("profiles/{profileId}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> GetCurrentProfileTransaction(
            [FromRoute] Guid profileId, 
            [FromQuery] PaginationRequest paginationRequest,
            [FromQuery] TransactionFilterRequest filterRequest)
        {
            object? transactions = default;
            transactions = await _transactionService.GetTransactionPageByProfileIdAndCurrentUser(profileId, paginationRequest, filterRequest, await GetUserAsync());
            return SuccessResult(transactions);
        }
        [HttpPost("games")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreatGameTransaction([FromBody] CreateGameTransactionRequest request)
        {
            await _transactionService.CreateGameTransactionAsync(request, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpGet("games/count/profiles/{profileId}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CountGameTransactionByCurrentUser([FromRoute] Guid profileId)
        {
            var gameTransactionCount = await _transactionService.GetPlayedGameCount(await GetUserAsync(), profileId);
            return SuccessResult<object>(gameTransactionCount);
        }

        [HttpGet()]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> GetCurrentAccountTransaction(
            [FromQuery] PaginationRequest paginationRequest,
            [FromQuery] TransactionFilterRequest filterRequest)
        {
            object? transactions = default;
            transactions = await _transactionService.GetMoneyTransactionPageByCurrentUser(paginationRequest, filterRequest, await GetUserAsync());
            return SuccessResult(transactions);
        }
        [HttpGet("countByMonth")]
        [Authorize(RoleName.ADMIN, RoleName.MANAGER)]
        public async Task<IActionResult> GetTransactionsForDashBoard([FromQuery] GetTransactionsForDashBoardRequest request)
        {
            var transactions = await _transactionService.GetTransactionsForDashBoard(request);
            return SuccessResult(transactions);
        }
    }
}
