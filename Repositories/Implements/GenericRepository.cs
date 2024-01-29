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

namespace Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
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

        #region Gett Async
        private IQueryable<T> buildQuery(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
            )
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);
            return query;
        }
        public virtual async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = buildQuery(predicate, orderBy, include);
            return await query.AsNoTracking().FirstOrDefaultAsync();

            //return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<TResult?> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> selector, 
            Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(predicate, orderBy, include);

            return await query.AsNoTracking().Select(selector).FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(predicate, orderBy, include);
            return await query.AsNoTracking().ToListAsync();

            //return await query.AsNoTracking().ToListAsync();
        }

        // public virtual async Task<ICollection<D>> GetMappedListAsync(
        //     Expression<Func<T, bool>> predicate = null,
        //     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        // )
        // {
        //     IQueryable<T> query = _dbSet;
        //
        //     if (include != null) query = include(query);
        //
        //     if (predicate != null) query = query.Where(predicate);
        //
        //     if (orderBy != null) return await orderBy(query).ProjectTo<D>(_mapper.ConfigurationProvider).ToListAsync();
        //
        //     return await query.ProjectTo<D>(_mapper.ConfigurationProvider).ToListAsync();
        // }
        public virtual async Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector, 
            Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(predicate, orderBy, include);
            //return.AsNoTracking().Select(selector).ToListAsync();

            return await query.Select(selector).ToListAsync();
        }

        public Task<IPaginable<T>> GetPageAsync(
            PaginationRequest request,
            Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
        )
        {
            IQueryable<T> query = buildQuery(predicate, orderBy, include);
            return query.ToPaginableAsync(request.Page, request.Size, 1);
        }
        
        // public Task<IPaginable<D>> GetMappedPageAsync(
        //     PaginationRequest request,
        //     Expression<Func<T, bool>> predicate = null, 
        //     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
        //     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        //     )
        // {
        //     IQueryable<T> query = _dbSet;
        //     if (include != null) query = include(query);
        //     if (predicate != null) query = query.Where(predicate);
        //     if (orderBy != null) return orderBy(query).ProjectTo<D>(_mapper.ConfigurationProvider).ToPaginableAsync(request.Page, request.Size, 1);
        //     return query.ProjectTo<D>(_mapper.ConfigurationProvider).ToPaginableAsync(request.Page, request.Size, 1);
        // }

        
        public Task<IPaginable<TResult>> GetPageAsync<TResult>(
            Expression<Func<T, TResult>> selector, 
            PaginationRequest request,
            Expression<Func<T, bool>>? predicate = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) return orderBy(query).Select(selector).ToPaginableAsync(request.Page, request.Size, 1);
            return query.AsNoTracking().Select(selector).ToPaginableAsync(request.Page, request.Size, 1);
        }

        #endregion

        #region Insert

        public async Task InsertAsync(T entity)
        {
            if (entity == null) return;
            var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _dbSet.AddAsync(entity);
                transaction.Commit();
                await _dbContext.SaveChangesAsync();
            }
            catch 
            {
                transaction.Rollback();
                throw;
            }
            
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Update
        public void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public Task<T?> FirstOrDefaultAsync(BaseEntityStatus status, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResult?> FirstOrDefaultAsync<TResult>(BaseEntityStatus status, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<T>> GetListAsync(BaseEntityStatus status, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TResult>> GetListAsync<TResult>(BaseEntityStatus status, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            throw new NotImplementedException();
        }

        public Task<IPaginable<T>> GetPageAsync(BaseEntityStatus status, PaginationRequest paginationRequest, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            throw new NotImplementedException();
        }

        public Task<IPaginable<TResult>> GetPageAsync<TResult>(BaseEntityStatus status, Expression<Func<T, TResult>> selector, PaginationRequest paginationRequest, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
