﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DataTransferObjects.Core.Pagination;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        #region Read
        Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<TResult> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>?orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<T>> GetPagingListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int page = 1,
            int size = 10);

        Task<IPaginable<TResult>> GetPagingListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int page = 1,
            int size = 10);

        #endregion

        #region Insert

        Task InsertAsync(T entity);

        Task InsertRangeAsync(IEnumerable<T> entities);

        #endregion

        #region Update

        void UpdateAsync(T entity);

        void UpdateRange(IEnumerable<T> entities);

        #endregion

        void DeleteAsync(T entity);
        void DeleteRangeAsync(IEnumerable<T> entities);
    }
}
