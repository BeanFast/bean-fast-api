using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
<<<<<<< HEAD
using DataTransferObjects.Models.Transaction.Request;
=======
>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;
<<<<<<< HEAD
using Utilities.Statuses;

namespace Services.Implements
{
    public class TransactionService : BaseService<BusinessObjects.Models.Transaction>, ITransactionService
=======

namespace Services.Implements
{
    public class TransactionService : BaseService<Transaction>, ITransactionService
>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
    {
        public TransactionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }
<<<<<<< HEAD
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
=======

>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
    }
}
