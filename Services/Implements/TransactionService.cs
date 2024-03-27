using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.VnPay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{

    public class TransactionService : BaseService<Transaction>, ITransactionService
    {
        private readonly IVnPayService _vnPayService;
        public TransactionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IVnPayService vnPayService) : base(unitOfWork, mapper, appSettings)
        {
            _vnPayService = vnPayService;
        }
        public async Task CreateTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            transaction.Status = OrderActivityStatus.Active;
            transaction.Id = Guid.NewGuid();
            await _repository.InsertAsync(transaction);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateTransactionListAsync(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                await CreateTransactionAsync(transaction);
            }
        }

        public string CreateVnPayPaymentRequest(User user, int amount, HttpContext context)
        {
            var wallet = user.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type));
            var vnPayEntity = new VnPayRequest
            {
                WalletId = wallet!.Id,
                Description = "Nap tien",
                Amount = amount,
                CreatedDate = DateTime.Now,
                FullName = user.FullName!,
            };
            return _vnPayService.CreatePaymentUrl(context, vnPayEntity);
        }
        public Task CreateMoneyTransactionAsync()
        {

        }
    }
}
