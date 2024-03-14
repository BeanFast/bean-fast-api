﻿using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.SessionDetail.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISessionDetailService
    {
        Task<SessionDetail> GetByIdAsync(Guid id);
        Task<GetSessionDetailResponse> GetSessionDetailResponseByIdAsync(Guid id);
    }
}