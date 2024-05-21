using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.Transaction.Response;

namespace Repositories.Interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<int> GetPlayedGameCountAsync(User user);
    Task<ICollection<GetTransactionsForDashBoardResponse>> GetTransactionsForDashBoardAsync(GetTransactionsForDashBoardRequest request);
    Task<IPaginable<GetTransactionPageByCurrentUserResponse>> GetMoneyTransactionPageByUserAsync(
        PaginationRequest paginationRequest,
        TransactionFilterRequest filterRequest,
        User user);
    Task<IPaginable<GetTransactionPageByProfileIdAndCurrentUserResponse>> GetTransactionPageByProfileIdAndUserAsync(
        Guid profileId,
        PaginationRequest paginationRequest,
        TransactionFilterRequest filterRequest,
        User user);
    
    }