using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.OrderActivity.Response;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Repositories.Implements;

public class OrderActivityRepository : GenericRepository<OrderActivity>, IOrderActivityRepository
{
    public OrderActivityRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<OrderActivity> GetByIdAsync(Guid id)
    {
        List<Expression<Func<OrderActivity, bool>>> filters = new()
            {
                (orderActivity) => orderActivity.Id == id,

            };
        var orderActivity = await FirstOrDefaultAsync(
            filters: filters)
            ?? throw new EntityNotFoundException(MessageConstants.OrderActivityMessageConstrant.OrderActivityNotFound(id));
        return orderActivity;
    }


    public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user)
    {
        var roleName = user.Role!.EnglishName;
        List<Expression<Func<OrderActivity, bool>>> filters = new();
        if (RoleName.CUSTOMER.ToString().Equals(roleName))
        {
            filters.Add(oa => oa.Order!.Profile!.UserId == user.Id);
        }
        filters.Add(oa => oa.OrderId == orderId);
        var result = await GetListAsync<GetOrderActivityResponse>(filters: filters);
        return result;
        //await GetListAsync()
    }

    public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user)
    {
        var roleName = user.Role!.EnglishName;
        List<Expression<Func<OrderActivity, bool>>> filters = new();
        if (RoleName.CUSTOMER.ToString().Equals(roleName))
        {
            filters.Add(oa => oa.Order!.Profile!.UserId == user.Id);
        }
        filters.Add(oa => oa.ExchangeGiftId == exchangeGiftId);
        var result = await GetListAsync<GetOrderActivityResponse>(filters: filters);
        return result;
    }
}