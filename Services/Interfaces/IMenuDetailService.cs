using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMenuDetailService : IBaseService
    {
        Task<MenuDetail> GetByIdAsync(Guid id);
        Task HardDeleteAsync(List<MenuDetail> menuDetails);
        Task InsertRangeAsync(List<MenuDetail> menuDetails);
    }
}
