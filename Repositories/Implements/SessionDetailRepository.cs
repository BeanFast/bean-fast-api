using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.SessionDetail.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;
using Utilities.Utils;

namespace Repositories.Implements;

public class SessionDetailRepository : GenericRepository<SessionDetail>, ISessionDetailRepository
{
    public SessionDetailRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<SessionDetail> GetByIdAsync(Guid id)
    {
        List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.Id == id && sessionDetail.Status != BaseEntityStatus.Deleted
            };
        var sessionDetail = await FirstOrDefaultAsync(
                filters: filters,
                include: queryable => queryable.Include(sd => sd.SessionDetailDeliverers!).ThenInclude(sdd => sdd.Deliverer).Include(sd => sd.Location!).Include(sd => sd.Session!))
            ?? throw new EntityNotFoundException(MessageConstants.SessionDetailMessageConstrant.SessionDetailNotFound(id));
        return sessionDetail;
    }
    public async Task<ICollection<GetSessionDetailResponse>> GetSessionDetailByDelivererIdAsync(User user)
    {

        List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == user.Id),
                (sessionDetail) => sessionDetail.Status != BaseEntityStatus.Deleted,
                (sessionDetail) => sessionDetail.Session!.DeliveryStartTime >= TimeUtil.GetCurrentVietNamTime()
            };

        var sessionDetails = await GetListAsync<GetSessionDetailResponse>(
            filters: filters
            , include: queryable => queryable
            .Include(sd => sd.Location!)
                .ThenInclude(l => l.School!)
                .ThenInclude(s => s.Area!)
            .Include(sd => sd.Session!)
            .Include(sd => sd.Orders!)
                .ThenInclude(o => o.OrderDetails!)
            );
        foreach (var item in sessionDetails)
        {
            item.Orders = item.Orders!.Where(o => o.Status == OrderStatus.Delivering).ToList();
            item.ExchangeGifts = item.ExchangeGifts!.Where(eg => eg.Status == ExchangeGiftStatus.Delivering).ToList();

        }

        return sessionDetails;
    }
    public async Task<ICollection<GetSessionDetailResponse>> GetIncommingDeliveringSessionDetailsAsync(User user)
    {
        List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.Session!.DeliveryStartTime > TimeUtil.GetCurrentVietNamTime(),
                (sessionDetail) => sessionDetail.Status != BaseEntityStatus.Deleted,
            };
        var sessionDetails = await GetListAsync<GetSessionDetailResponse>(
            filters: filters);
        return sessionDetails;
    }
}