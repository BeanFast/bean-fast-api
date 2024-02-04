﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DataTransferObjects.Core.Pagination;
using Utilities.Enums;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        
        #region Read
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<T?> FirstOrDefaultAsync(
            BaseEntityStatus status,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);


        Task<TResult?> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<TResult?> FirstOrDefaultAsync<TResult>(
            BaseEntityStatus status,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<T>> GetListAsync(
            BaseEntityStatus status,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<ICollection<TResult>> GetListAsync<TResult>(
            BaseEntityStatus status,
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<T>> GetPageAsync(
            BaseEntityStatus status,
            PaginationRequest paginationRequest,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<IPaginable<T>> GetPageAsync(
            PaginationRequest paginationRequest,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        //Task<IPaginable<T>> GetPageAsync(
        //    BaseEntityStatus status,
        //    PaginationRequest paginationRequest,
        //    Expression<Func<T, bool>>? predicate = null,
        //    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        //    Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<TResult>> GetPageAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            PaginationRequest paginationRequest,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<TResult>> GetPageAsync<TResult>(
            BaseEntityStatus status,
            Expression<Func<T, TResult>> selector,
            PaginationRequest paginationRequest,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);


        #endregion

        #region Insert

        Task InsertAsync(T entity);

        Task InsertRangeAsync(IEnumerable<T> entities);

        #endregion

        #region Update

        Task UpdateAsync(T entity);

        void UpdateRange(IEnumerable<T> entities);

        #endregion

        void DeleteAsync(T entity);
        void DeleteRangeAsync(IEnumerable<T> entities);
    }
}
