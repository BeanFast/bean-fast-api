using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DataTransferObjects.Core.Pagination;
using Utilities.Enums;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        
        #region Read
        Task<T?> FirstOrDefaultAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<T?> FirstOrDefaultAsync(
            int status,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);


        Task<TResult?> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<TResult?> FirstOrDefaultAsync<TResult>(
            int status,
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<T>> GetListAsync(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<T>> GetListAsync(
            int status,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<ICollection<TResult>> GetListAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<ICollection<TResult>> GetListAsync<TResult>(
            int status,
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<T>> GetPageAsync(
            int status,
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<IPaginable<T>> GetPageAsync(
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        //Task<IPaginable<T>> GetPageAsync(
        //    int status,
        //    PaginationRequest paginationRequest,
        //    List<Expression<Func<T, bool>>>? filters = null,
        //    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        //    Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<TResult>> GetPageAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<TResult>> GetPageAsync<TResult>(
            int status,
            Expression<Func<T, TResult>> selector,
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null,
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

        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task HardDeleteAsync(T entity);

        Task HardDeleteRangeAsync(IEnumerable<T> entities);
    }
}
