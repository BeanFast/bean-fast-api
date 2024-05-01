using DataTransferObjects.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects.Models;
using System;
using System.Linq;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Storage;
using Utilities.Statuses;
using System.Net.NetworkInformation;
using Utilities.Utils;

namespace Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;
        public GenericRepository(DbContext context, IMapper mapper)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
            _mapper = mapper;
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        private IQueryable<T> buildQuery(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            filters?.ForEach(filter => query = query.Where(filter));

            if (orderBy != null) query = orderBy(query);
            return query.AsNoTracking();
        }

        private IQueryable<T> buildQuery(
            int status,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            filters?.ForEach(filter => query = query.Where(filter));
            query.Where(e => e.Status.Equals(status));
            if (orderBy != null) query = orderBy(query);
            return query.AsNoTracking();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(int status,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);

            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(int status,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);

            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<T>> GetListAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.ToListAsync();

            //return await query.ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(
            int status, List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return await query.ToListAsync();
        }
        public async Task<ICollection<TResult>> GetListAsync<TResult>(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<ICollection<TResult>> GetListAsync<TResult>(
            int status, List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>,
                IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>,
                IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return await query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }
        public async Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, object>>? groupBy = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            if(groupBy != null) query.GroupBy(groupBy);
            return await query.Select(selector).ToListAsync();
        }

        public Task<IPaginable<T>> GetPageAsync(
            PaginationRequest request,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return query.ToPaginableAsync(request.Page, request.Size, 1);
        }

        public Task<IPaginable<T>> GetPageAsync(int status, PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return query.ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }

        public Task<IPaginable<TResult>> GetPageAsync<TResult>(
            PaginationRequest request,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return query.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToPaginableAsync(request.Page, request.Size, 1);
        }

        public Task<IPaginable<TResult>> GetPageAsync<TResult>(int status,
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return query.ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }
        public async Task<int> CountAsync()
        {
            var result = await _dbSet.CountAsync();
            return result;
        }


        public async Task InsertAsync(T entity)
        {
            if(entity.Status == 0) entity.Status = BaseEntityStatus.Active;
            await _dbSet.AddAsync(entity);
        }
        public async Task InsertAsync(T entity, User? inserter)
        {
            if (entity.Status == 0) entity.Status = BaseEntityStatus.Active;
            if (entity is BaseAuditableEntity auditableEntity)
            {
                auditableEntity.CreatedDate = TimeUtil.GetCurrentVietNamTime();
                auditableEntity.UpdatedDate = TimeUtil.GetCurrentVietNamTime();
                auditableEntity.UpdaterId = inserter?.Id;
                auditableEntity.CreatorId = inserter?.Id;
            }
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }


        public async Task UpdateAsync(T entity)
        {
            _dbContext.ChangeTracker.Clear();
            //await _dbContext.SaveChangesAsync();
            if (entity is BaseAuditableEntity auditableEntity)
            {
                auditableEntity.UpdatedDate = TimeUtil.GetCurrentVietNamTime();
            }
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }
        public async Task UpdateAsync(T entity, User? updater)
        {
            _dbContext.ChangeTracker.Clear();
            //await _dbContext.SaveChangesAsync();
            if (entity is BaseAuditableEntity auditableEntity)
            {
                auditableEntity.UpdatedDate = TimeUtil.GetCurrentVietNamTime();
                auditableEntity.UpdaterId = updater?.Id;
            }
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            entity.Status = BaseEntityStatus.Deleted;
            await UpdateAsync(entity);
        }
        public async Task DeleteAsync(T entity, User deleter)
        {
            entity.Status = BaseEntityStatus.Deleted;
            await UpdateAsync(entity, deleter);
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task HardDeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task HardDeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }


    }
}