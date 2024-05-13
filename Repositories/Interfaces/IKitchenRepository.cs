using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IKitchenRepository : IGenericRepository<Kitchen>
    {
        Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole);
        Task<Kitchen> GetByIdAsync(Guid id);
        Task<Kitchen> GetByIdAsync(int status, Guid id);
        Task<Kitchen> GetByIdIncludePrimarySchoolsAsync(Guid id);
        Task<int> CountSchoolByKitchenIdAsync(Guid kitchentId);
        Task<ICollection<GetKitchenResponse>> GetAllAsync(string? userRole, KitchenFilterRequest filterRequest);
        Task<Kitchen?> GetByManagerId(Guid managerId);
    }
}
