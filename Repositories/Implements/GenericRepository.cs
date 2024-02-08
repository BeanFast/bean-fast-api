using DataTransferObjects.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Utilities.Enums;
using BusinessObjects.Models;
using System;
using System.Linq;
using Azure.Core;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
            _dbSet = context.Set<T>();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        private IQueryable<T> buildQuery(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            filters?.ForEach(filter => query = query.Where(filter));

            if (orderBy != null) query = orderBy(query);
            return query;
        }

        private IQueryable<T> buildQuery(
            BaseEntityStatus status,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            filters?.ForEach(filter => query = query.Where(filter));
            query.Where(e => e.Status.Equals((int)status));
            if (orderBy != null) query = orderBy(query);
            return query;
        }

        public virtual async Task<T?> FirstOrDefaultAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.AsNoTracking().FirstOrDefaultAsync();
            //return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(BaseEntityStatus status,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);

            return await query.AsNoTracking().Select(selector).FirstOrDefaultAsync();
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(BaseEntityStatus status,
            Expression<Func<T, TResult>> selector, List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);

            return await query.AsNoTracking().Select(selector).FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<T>> GetListAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.AsNoTracking().ToListAsync();

            //return await query.AsNoTracking().ToListAsync();
        }

        public async Task<ICollection<T>> GetListAsync(BaseEntityStatus status,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return await query.AsNoTracking().ToListAsync();
        }

        // public virtual async Task<ICollection<D>> GetMappedListAsync(
        //     Expression<Func<T, bool>> filters = null,
        //     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        // )
        // {
        //     IQueryable<T> query = _dbSet;
        //
        //     if (include != null) query = include(query);
        //
        //     if (filters != null) query = query.Where(filters);
        //
        //     if (orderBy != null) return await orderBy(query).ProjectTo<D>(_mapper.ConfigurationProvider).ToListAsync();
        //
        //     return await query.ProjectTo<D>(_mapper.ConfigurationProvider).ToListAsync();
        // }
        public virtual async Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.Select(selector).ToListAsync();
        }

        public async Task<ICollection<TResult>> GetListAsync<TResult>(BaseEntityStatus status,
            Expression<Func<T, TResult>> selector, List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
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

        public Task<IPaginable<T>> GetPageAsync(BaseEntityStatus status, PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return query.ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }

        // public Task<IPaginable<D>> GetMappedPageAsync(
        //     PaginationRequest request,
        //     Expression<Func<T, bool>> filters = null, 
        //     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
        //     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        //     )
        // {
        //     IQueryable<T> query = _dbSet;
        //     if (include != null) query = include(query);
        //     if (filters != null) query = query.Where(filters);
        //     if (orderBy != null) return orderBy(query).ProjectTo<D>(_mapper.ConfigurationProvider).ToPaginableAsync(request.Page, request.Size, 1);
        //     return query.ProjectTo<D>(_mapper.ConfigurationProvider).ToPaginableAsync(request.Page, request.Size, 1);
        // }


        public Task<IPaginable<TResult>> GetPageAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            PaginationRequest request,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return query.AsNoTracking().Select(selector).ToPaginableAsync(request.Page, request.Size, 1);
        }

        public Task<IPaginable<TResult>> GetPageAsync<TResult>(BaseEntityStatus status,
            Expression<Func<T, TResult>> selector, PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return query.AsNoTracking().Select(selector)
                .ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }


        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            entity.Status = (int)BaseEntityStatus.INACTIVE;
            await UpdateAsync(entity);
        }

        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}