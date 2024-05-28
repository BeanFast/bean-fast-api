using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;
using Utilities.Utils;

namespace Repositories.Implements;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {

    }
    private List<Expression<Func<Order, bool>>> GetFiltersFromOrderRequest(OrderFilterRequest request)
    {
        List<Expression<Func<Order, bool>>> filters = new();
        if (request.Status != null)
        {
            if (request.Status == 6)
            {
                filters.Add(o => o.Status == OrderStatus.Cancelled || o.Status == OrderStatus.CancelledByCustomer);
            }
            else
            {
                filters.Add(o => o.Status == request.Status);
            }
        }
        if (request.SchoolId != null)
        {
            filters.Add(o => o.SessionDetail!.Location!.SchoolId == request.SchoolId);
        }
        if (request.SessionDetailId != null)
        {
            filters.Add(o => o.SessionDetailId == request.SessionDetailId);
        }
        if (request.SessionId != null)
        {
            filters.Add(o => o.SessionDetail!.SessionId == request.SessionId);
        }
        return filters;
    }
    public async Task<ICollection<Order>> GetOrdersAsync(DateTime startDate, DateTime endDate, int? status = null)
    {
        var filters = new List<Expression<Func<Order, bool>>>
            {
                    order => order.PaymentDate.Date >= startDate.Date && order.PaymentDate.Date <= endDate.Date
            };
        if (status is not null)
        {
            filters.Add(order => order.Status == status);
        }
        var orders = await GetListAsync(
            filters: filters
        );
        return orders;
    }
    public async Task<ICollection<GetDelivererIdAndOrderCountBySessionDetailIdResponse>> GetDelivererIdAndOrderCountBySessionDetailId(Guid sessionDetailId)
    {
        var filters = new List<Expression<Func<Order, bool>>>
        {
                order => order.SessionDetailId == sessionDetailId
        };
        var orders = await GetListAsync(
                       filters: filters,
                       include: queryable => queryable.Include(o => o.Profile!));
        var exchangeGift = await _dbContext.ExchangeGifts.Where(
                ex => ex.SessionDetailId == sessionDetailId 
                && ex.Status != ExchangeGiftStatus.Cancelled && ex.Status != ExchangeGiftStatus.CancelledByCustomer
                && ex.DelivererId != null && ex.DelivererId != Guid.Empty
            ).Include(ex => ex.Profile).ToListAsync();
        var result = orders.GroupBy(o => o.DelivererId)
            .Select(g => new GetDelivererIdAndOrderCountBySessionDetailIdResponse
            {
                DelivererId = g.Key,
                OrderCount = g.Count(),
                CustomerIds = g.Select(o => o.Profile!.UserId).ToHashSet()
            }).ToList();
        result.AddRange(exchangeGift.GroupBy(ex => ex.DelivererId).Select(ex => new GetDelivererIdAndOrderCountBySessionDetailIdResponse
        {
            DelivererId = ex.Key.Value,
            OrderCount = ex.Count(),
            CustomerIds = ex.Select(o => o.Profile!.UserId).ToHashSet()
        }));
        return result;
    }
    public async Task<ICollection<Order>> GetCompletedOrderIncludeKitchenAsync()
    {
        var filters = new List<Expression<Func<Order, bool>>>
            {
                order => order.Status == OrderStatus.Completed
            };
        var orders = await GetListAsync(
            filters: filters,
            include: queryable => queryable
                .Include(o => o.OrderDetails!)
                        .ThenInclude(od => od.Food!)
                        .ThenInclude(f => f.MenuDetails!)
                        .ThenInclude(md => md.Menu!)
                        .ThenInclude(m => m.Kitchen!)
            );
        return orders;
    }
    public async Task<ICollection<Order>> GetCompletedOrderIncludeSchoolAsync()
    {
        var filters = new List<Expression<Func<Order, bool>>>
            {
                order => order.Status == OrderStatus.Completed
            };
        var orders = await GetListAsync(
            filters: filters,
            include: queryable => queryable
                .Include(o => o.SessionDetail!)
                    .ThenInclude(sd => sd.Location!)
                    .ThenInclude(l => l.School!)
                .Include(o => o.OrderDetails!)
        );
        return orders;
    }
    public async Task<ICollection<Order>> GetDeliveringOrdersByDelivererIdAndCustomerIdAsync(Guid delivererId, Guid customerId)
    {
        List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.SessionDetail!.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == delivererId)
                && order.Profile!.UserId == customerId
                && order.Status == OrderStatus.Delivering
            };
        var orders = await GetListAsync(
            filters: filters,
            include: queryable => queryable
            .Include(o => o.SessionDetail!)
                .ThenInclude(sd => sd.Session!)
            .Include(o => o.OrderDetails!)
            .Include(o => o.Profile!)
                .ThenInclude(p => p.User!)
        );
        return orders!;
    }
    public async Task<ICollection<GetOrderResponse>> GetOrdersByStatusAsync(int status)
    {
        List<Expression<Func<Order, bool>>> filters = new()
        {

        };
        if (status == OrderStatus.Cancelled)
        {
            filters.Add(o => o.Status == OrderStatus.Cancelled || o.Status == OrderStatus.CancelledByCustomer);
        }
        else
        {
            filters.Add(o => o.Status == status);
        }
        var orders = await GetListAsync(filters: filters,
            include: queryable => queryable.Include(o => o.Profile!).Include(o => o.SessionDetail!));

        return _mapper.Map<ICollection<GetOrderResponse>>(orders);
    }
    public async Task<Order> GetByIdAsync(Guid id)
    {
        List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.Id == id && order.Status != BaseEntityStatus.Deleted
            };
        var order = await FirstOrDefaultAsync(
            filters: filters, include: queryable =>
            queryable
            .Include(o => o.Profile!)
                .ThenInclude(p => p.User!)
                .ThenInclude(p => p.Wallets)
            .Include(o => o.Profile!)
                .ThenInclude(p => p.User!)
            .Include(o => o.SessionDetail!)
                .ThenInclude(o => o.Session!)
            .Include(o => o.OrderDetails!)
            .Include(o => o.OrderActivities!))

            ?? throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.OrderNotFound(id));
        return order!;
    }
    public async Task<ICollection<GetOrderResponse>> GetAllAsync(OrderFilterRequest request, User user)
    {
        var filters = GetFiltersFromOrderRequest(request);
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = (o) => o.Include(o => o.Profile!).Include(o => o.SessionDetail!);

        if (RoleName.MANAGER.ToString().Equals(user.Role!.EnglishName))
        {
            //filters.Add()
        }
        else if (RoleName.CUSTOMER.ToString().Equals(user.Role!.EnglishName))
        {
            filters.Add(o => o.Profile!.UserId == user.Id);
        }
        else if (RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName))
        {
            filters.Add(o => o.SessionDetail!.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == user.Id));
        }

        return await GetListAsync<GetOrderResponse>(filters: filters, include: include, orderBy: o => o.OrderByDescending(order => order.PaymentDate));

    }
    public async Task<IPaginable<GetOrderResponse>> GetPageAsync(PaginationRequest paginationRequest, OrderFilterRequest request, User user)
    {
        var filters = GetFiltersFromOrderRequest(request);
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = (o) => o.Include(o => o.Profile!).Include(o => o.SessionDetail!);
        
        if (RoleName.MANAGER.ToString().Equals(user.Role!.EnglishName))
        {
            //filters.Add()
        }
        else if (RoleName.CUSTOMER.ToString().Equals(user.Role!.EnglishName))
        {
            filters.Add(o => o.Profile!.UserId == user.Id);
        }
        else if (RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName))
        {
            filters.Add(o => o.SessionDetail!.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == user.Id));
        }
        return await GetPageAsync<GetOrderResponse>(filters: filters, include: include, paginationRequest: paginationRequest, orderBy: o => o.OrderByDescending(order => order.PaymentDate));
    }
    public async Task<IPaginable<GetOrderResponse>> GetPageAsync(string? userRole, PaginationRequest request)
    {
        Expression<Func<Order, GetOrderResponse>> selector = (o => _mapper.Map<GetOrderResponse>(o));
        Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = o => o.OrderBy(o => o.DeliveryDate);
        IPaginable<GetOrderResponse>? page = null;
        if (RoleName.ADMIN.ToString().Equals(userRole))
        {
            page = await GetPageAsync<GetOrderResponse>(
                paginationRequest: request, orderBy: orderBy);
        }
        else
        {
            page = await GetPageAsync<GetOrderResponse>(
                paginationRequest: request, orderBy: orderBy);
        }
        return page;
    }
    public async Task<GetOrderResponse> GetOderResponseByIdAsync(Guid id)
    {
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include =
            (o) => o.Include(o => o.Profile!)
            .Include(o => o.SessionDetail!)
            .ThenInclude(sd => sd.Session!)
            .Include(o => o.SessionDetail!)
            .ThenInclude(sd => sd.Location!)
            .ThenInclude(l => l.School!)
            .ThenInclude(school => school.Area!);
        List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.Id == id,
                //(order) => order.Status == BaseEntityStatus.Active
            };
        var result = await FirstOrDefaultAsync<GetOrderResponse>(
            filters: filters, include: include)
            ?? throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.OrderNotFound(id));
        return result!;
    }


}