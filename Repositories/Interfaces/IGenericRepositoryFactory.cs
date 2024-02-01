using BusinessObjects.Models;

namespace Repositories.Interfaces
{
	public interface IGenericRepositoryFactory
	{
		IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

    }
}
