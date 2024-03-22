using BusinessObjects.Models;
using DataTransferObjects.Models.Transaction.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITransactionService : IBaseService
    {
        Task CreateTransactionAsync(Transaction transaction);
        Task CreateTransactionListAsync(List<Transaction> transactions);
    }
}
