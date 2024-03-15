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
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            filters?.ForEach(filter => query = query.Where(filter));

            if (orderBy != null) query = orderBy(query);
            return query;
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
            return query;
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
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);

            return await query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(int status,
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);

            return await query.Select(selector).FirstOrDefaultAsync();
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
        public async Task<ICollection<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> selector, List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return await query.Select(selector).ToListAsync();
        }

        public async Task<ICollection<TResult>> GetListAsync<TResult>(int status, Expression<Func<T, TResult>> selector, List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
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

        public Task<IPaginable<T>> GetPageAsync(int status, PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return query.ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }

        public Task<IPaginable<TResult>> GetPageAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            PaginationRequest request,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(filters, orderBy, include);
            return query.Select(selector).ToPaginableAsync(request.Page, request.Size, 1);
        }

        public Task<IPaginable<TResult>> GetPageAsync<TResult>(int status,
            Expression<Func<T, TResult>> selector, PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, filters, orderBy, include);
            return query.Select(selector)
                .ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }


        public async Task UpdateAsync(T entity)
        {
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