﻿using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMenuDetailService
    {
        Task<int> CountAsync();
        Task<MenuDetail> GetByIdAsync(Guid id);
    }
}
