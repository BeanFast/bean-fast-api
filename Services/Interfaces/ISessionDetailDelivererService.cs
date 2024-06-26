﻿using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISessionDetailDelivererService
    {
        Task HardDeleteAsync(List<SessionDetailDeliverer> sessionDetailDeliverers);
        Task InsertListAsync(List<SessionDetailDeliverer> sessionDetailDeliverers);
        Task<ICollection<SessionDetailDeliverer>> GetBySessionDetailId(Guid sessionDetailId);
    }
}
