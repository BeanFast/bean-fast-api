using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;
using Utilities.Utils;

namespace Repositories.Implements
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        private List<Expression<Func<Session, bool>>> getFiltersFromSessionFilterRequest(SessionFilterRequest request, string userRole)
        {
            List<Expression<Func<Session, bool>>> filters = new List<Expression<Func<Session, bool>>>();
            //Expression<Func<Session, bool>> orFilter = (s) =>
            //{
            //    bool filter = false;
            //    if (request.Expired)
            //    {
            //        filter = filter || s.OrderEndTime < TimeUtil.GetCurrentVietNamTime();
            //    }
            //    if (request.Incomming)
            //    {
            //        filter = filter || s.OrderStartTime > TimeUtil.GetCurrentVietNamTime();
            //    }
            //    if (request.Orderable)
            //    {
            //        filter = filter || (s.OrderStartTime <= TimeUtil.GetCurrentVietNamTime() && s.OrderEndTime > TimeUtil.GetCurrentVietNamTime());
            //    }
            //    return filter;
            //};
            //filters.Add(orFilter);
            var currentVietNamTime = TimeUtil.GetCurrentVietNamTime();
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {

                if (request.Orderable)
                {
                    filters.Add(s => s.OrderStartTime <= currentVietNamTime && s.OrderEndTime > currentVietNamTime);
                }
            }
            else
            {
                if (request.Orderable)
                {
                    filters.Add(s => s.OrderStartTime <= currentVietNamTime && s.OrderEndTime > currentVietNamTime);
                }
                filters.Add(s => s.Status != BaseEntityStatus.Deleted);
            }
            if (request.Expired)
            {
                filters.Add(s => s.OrderEndTime < currentVietNamTime);
            }
            if (request.Incomming)
            {
                filters.Add(s => s.OrderStartTime > currentVietNamTime);
            }
            if (request.MenuId != Guid.Empty)
            {
                filters.Add(s => s.MenuId == request.MenuId);
            }
            if (request.SchoolId != null && request.SchoolId != Guid.Empty)
            {
                filters.Add(s => s.SessionDetails!.Where(sd => sd.Location!.SchoolId == request.SchoolId && sd.Status == BaseEntityStatus.Active).Any());
            }
            if (request.OrderStartTime != null)
            {
                filters.Add(s => s.OrderStartTime.Date.Equals(request.OrderStartTime.Value.Date));
            }
            if (request.OrderTime != null)
            {
                filters.Add(s => s.OrderStartTime.Date <= request.OrderTime.Value.Date && s.OrderEndTime.Date >= request.OrderTime.Value.Date);
            }
            if (request.DeliveryTime != null)
            {
                filters.Add(s => s.DeliveryStartTime.Date <= request.DeliveryTime.Value.Date && s.DeliveryEndTime.Date >= request.DeliveryTime.Value.Date);
            }
            //if(request.DeliveryEndTime)
            return filters;
        }

        public async Task<Session?> GetSessionByMenuDetailIdAndProfileIdAndSessionIdAsync(Guid menuDetailId, Guid profileId, Guid sessionId)
        {
            var filters = new List<Expression<Func<Session, bool>>>()
            {
                s => s.Id == sessionId,
                s => s.Status != BaseEntityStatus.Deleted,
                s => s.Menu!.MenuDetails!.Any(md => md.Id == menuDetailId),
            };

            var session = await FirstOrDefaultAsync(
                filters: filters,
                include: i =>
                    i.Include(s => s.SessionDetails!)
                        .ThenInclude(sd => sd.Location!)
                        .ThenInclude(l => l.School!)
                        .ThenInclude(s => s.Profiles!.Where(p => p.Id == profileId && p.Status != BaseEntityStatus.Deleted))
                    .Include(s => s.Menu!)
                        .ThenInclude(m => m.MenuDetails!.Where(md => md.Id == menuDetailId))
            );
            return session;
        }
        public async Task<Session?> GetByIdForDelete(Guid id)
        {
            return await FirstOrDefaultAsync(filters: new()
            {
                session => session.Id == id,
                session => session.Status == BaseEntityStatus.Active
            }, include: i => i
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.Orders!)
                .ThenInclude(s => s.OrderActivities!)
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.Orders!)
                .ThenInclude(o => o.Profile!)
                .ThenInclude(p => p.Wallets!));
        }
        public async Task<ICollection<Session>> GetOverlappedDeliveryTimeSessions(DateTime deliveryStartTime, DateTime deliveryEndTime)
        {
            return await GetListAsync(filters: new()
            {
                s => s.DeliveryStartTime < deliveryEndTime && s.DeliveryEndTime >deliveryStartTime
            }, include: i => i.Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.SessionDetailDeliverers!)
                .ThenInclude(sdd => sdd.Deliverer!));
        }
        public async Task<GetSessionForDeliveryResponse> GetSessionForDeliveryResponseByIdAsync(Guid id, SessionFilterRequest request, string? userRole)
        {
            var filters = getFiltersFromSessionFilterRequest(request, userRole!);
            filters.Add((session) => session.Id == id && session.Status != BaseEntityStatus.Deleted);
            var result = await FirstOrDefaultAsync<GetSessionForDeliveryResponse>(filters: filters)
                 ?? throw new EntityNotFoundException(MessageConstants.SessionMessageConstrant.SessionNotFound(id));
            return result!;
        }
       
        public async Task<ICollection<GetSessionForDeliveryResponse>> GetAllAsync(string? userRole, SessionFilterRequest filterRequest)
        {
            var filters = getFiltersFromSessionFilterRequest(filterRequest, userRole!);
            return await GetListAsync<GetSessionForDeliveryResponse>(filters: filters);
        }

        public async Task<Session> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Session, bool>>> filters = new()
            {
                (session) => session.Id == id,
                (session) => session.Status != BaseEntityStatus.Deleted
            };
            var session = await FirstOrDefaultAsync(
                filters: filters,
                include: i => i.Include(s => s.SessionDetails!))
                ?? throw new EntityNotFoundException(MessageConstants.SessionMessageConstrant.SessionNotFound(id));
            return session;
        }
        public async Task<Session> GetBySessionDetailIdAsync(Guid sesionDetailId)
        {
            List<Expression<Func<Session, bool>>> filters = new()
            {
                (session) => session.SessionDetails!.Where(sd => sd.Id == sesionDetailId).Any(),
                (session) => session.Status != BaseEntityStatus.Deleted
            };
            var session = await FirstOrDefaultAsync(
                filters: filters,
                include: i => i.Include(s => s.SessionDetails!).ThenInclude(sd => sd.SessionDetailDeliverers!))
                ?? throw new EntityNotFoundException(MessageConstants.SessionDetailMessageConstrant.SessionDetailNotFound(sesionDetailId));
            return session;
        }
    }
}
