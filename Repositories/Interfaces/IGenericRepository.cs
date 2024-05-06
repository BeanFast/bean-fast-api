using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DataTransferObjects.Core.Pagination;
using Utilities.Enums;
using BusinessObjects.Models;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {

        Task<int> CountAsync();
        Task<int> CountAsync(List<Expression<Func<T, bool>>>? filters = null);
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
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<TResult?> FirstOrDefaultAsync<TResult>(
            int status,
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
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Expression<Func<T, object>>? groupBy = null);


        Task<ICollection<TResult>> GetListAsync<TResult>(
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<ICollection<TResult>> GetListAsync<TResult>(
            int status,
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
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

        Task<IPaginable<TResult>> GetPageAsync<TResult>(
            int status,
            PaginationRequest paginationRequest,
            List<Expression<Func<T, bool>>>? filters = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);



        Task InsertAsync(T entity);
        Task InsertAsync(T entity, User inserter);
        Task InsertRangeAsync(IEnumerable<T> entities);


        Task UpdateAsync(T entity);
        Task UpdateAsync(T entity, User updater);

        void UpdateRange(IEnumerable<T> entities);


        Task DeleteAsync(T entity);
        Task DeleteAsync(T entity, User deleter);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        Task HardDeleteAsync(T entity);
        Task HardDeleteRangeAsync(IEnumerable<T> entities);
    }
}
