using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repositories.Interfaces
{
	public interface IUnitOfWork : IGenericRepositoryFactory, IDisposable
	{
		int Commit();

		Task<int> CommitAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
	}

	public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
	{
		TContext Context { get; }
	}
}
