using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.Transaction.Response;
using Microsoft.AspNetCore.Http;
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
        string CreateVnPayPaymentRequest(User user, int amount, HttpContext context);
        Task CreateTopUpTransactionAsync(string walletId, string amount);
        Task<IPaginable<GetTransactionPageByProfileIdAndCurrentUserResponse>> GetTransactionPageByProfileIdAndCurrentUser(
            Guid profileId, 
            PaginationRequest paginationRequest,
            TransactionFilterRequest filterRequest,
            User user);
        Task<ICollection<GetTransactionsForDashBoardResponse>> GetTransactionsForDashBoard(GetTransactionsForDashBoardRequest request);
        Task<IPaginable<GetTransactionPageByCurrentUserResponse>> GetMoneyTransactionPageByCurrentUser(PaginationRequest paginationRequest, TransactionFilterRequest filterRequest, User user);
    }
}
