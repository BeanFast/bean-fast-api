using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.Transaction.Response;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Utils;

namespace Repositories.Implements;

public class TransactionRepository : GenericRepository<Transaction>,ITransactionRepository
{
    public TransactionRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    private List<Expression<Func<Transaction, bool>>> GetTransactionFilterFromTransactionFilterRequest(TransactionFilterRequest filterRequest)
    {
        List<Expression<Func<Transaction, bool>>> filters = new List<Expression<Func<Transaction, bool>>>();
        if (!filterRequest.Type.IsNullOrEmpty())
        {
            if (filterRequest.Type == "money")
            {
                filters.Add(t => WalletType.Money.ToString().Equals(t.Wallet!.Type));
            }
            else if (filterRequest.Type == "points")
            {
                filters.Add(t => WalletType.Points.ToString().Equals(t.Wallet!.Type));
            }
        }
        //if(!filterRequest.)
        return filters;
    }


    public async Task<IPaginable<GetTransactionPageByProfileIdAndCurrentUserResponse>> GetTransactionPageByProfileIdAndUserAsync(
        Guid profileId,
        PaginationRequest paginationRequest,
        TransactionFilterRequest filterRequest,
        User user)
    {
        var filters = GetTransactionFilterFromTransactionFilterRequest(filterRequest);
        filters.Add(
            t =>  t.Wallet!.UserId == user.Id
        );
        var transactionPage = await GetPageAsync<GetTransactionPageByProfileIdAndCurrentUserResponse>(
            paginationRequest, filters,
            orderBy: o => o.OrderByDescending(t => t.Time));
        return transactionPage;
    }
    public async Task<IPaginable<GetTransactionPageByCurrentUserResponse>> GetMoneyTransactionPageByUserAsync(
        PaginationRequest paginationRequest,
        TransactionFilterRequest filterRequest,
        User user)
    {
        var filters = GetTransactionFilterFromTransactionFilterRequest(filterRequest);
        filters.Add(t => t.Wallet!.UserId == user.Id);
        var transactionPage = await GetPageAsync<GetTransactionPageByCurrentUserResponse>(paginationRequest, filters,
            orderBy: o => o.OrderByDescending(t => t.Time));
        return transactionPage;
    }

    public async Task<ICollection<GetTransactionsForDashBoardResponse>> GetTransactionsForDashBoardAsync(GetTransactionsForDashBoardRequest request)
    {
        var filters = new List<Expression<Func<Transaction, bool>>>();
        if (request.Type == "money")
        {
            filters.Add(t => t.OrderId == null && t.GameId == null && t.ExchangeGiftId == null);
        }
        if (request.StartDate != DateTime.MinValue)
        {
            filters.Add(t => t.Time.Date >= request.StartDate.Date && t.Time.Date <= request.EndDate.Date);
        }
        var transactions = await GetListAsync(filters);
        return transactions.GroupBy(t => t.Time.Month)
            .OrderBy(group => group.Key)
            .Select(group => new GetTransactionsForDashBoardResponse
            {
                Count = group.Count(),
                Month = TimeUtil.GetMonthName(group.Key),
                Value = (int)group.Sum(t => t.Value)
            }).ToList();
    }
    public async Task<int> GetPlayedGameCountAsync(User user)
    {
        var currentVietnamDate = TimeUtil.GetCurrentVietNamTime();
        List<Expression<Func<Transaction, bool>>> filters = new List<Expression<Func<Transaction, bool>>>()
            {
                t => WalletType.Points.ToString().Equals(t.Wallet!.Type) && t.Wallet.UserId == user.Id,
                t => t.GameId != null && t.OrderId == null && t.ExchangeGiftId == null && t.Value >= 0,
                t => t.Time.Date == currentVietnamDate.Date
            };
        var playedGameTransactions = await GetListAsync(filters: filters);
        return playedGameTransactions.Count;
    }
    
   
}