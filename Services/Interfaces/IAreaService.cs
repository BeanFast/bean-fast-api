using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Services.Interfaces
{
    public interface IAreaService
    {
        Task<Area> GetAreaByIdAsync(Guid id);
        Task<Area> GetAreaByIdAsync(int status, Guid id);
    }
}
