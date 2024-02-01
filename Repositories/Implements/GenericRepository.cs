﻿using DataTransferObjects.Core.Pagination;
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

        private IQueryable<T> buildQuery(
            BaseEntityStatus status,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null
            )
        {
            IQueryable<T> query = buildQuery(predicate, orderBy, include);
            query.Where(e => e.Status == ((int)status));
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
        public async Task<T?> FirstOrDefaultAsync(BaseEntityStatus status, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, predicate, orderBy, include);
            return await query.AsNoTracking().FirstOrDefaultAsync();
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
        public async Task<TResult?> FirstOrDefaultAsync<TResult>(BaseEntityStatus status, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, predicate, orderBy, include);

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
        public async Task<ICollection<T>> GetListAsync(BaseEntityStatus status, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, predicate, orderBy, include);
            return await query.AsNoTracking().ToListAsync();
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
            return await query.Select(selector).ToListAsync();
        }
        public async Task<ICollection<TResult>> GetListAsync<TResult>(BaseEntityStatus status, Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, predicate, orderBy, include);
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

        public Task<IPaginable<T>> GetPageAsync(BaseEntityStatus status, PaginationRequest paginationRequest, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, predicate, orderBy, include);
            return query.ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
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
            IQueryable<T> query = buildQuery(predicate, orderBy, include);
            return query.AsNoTracking().Select(selector).ToPaginableAsync(request.Page, request.Size, 1);
        }
        public Task<IPaginable<TResult>> GetPageAsync<TResult>(BaseEntityStatus status, Expression<Func<T, TResult>> selector, PaginationRequest paginationRequest, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = buildQuery(status, predicate, orderBy, include);
            return query.AsNoTracking().Select(selector).ToPaginableAsync(paginationRequest.Page, paginationRequest.Size, 1);
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task InsertAsync(T entity)
        {
            if (entity == null) return;
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        
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

    }
}
