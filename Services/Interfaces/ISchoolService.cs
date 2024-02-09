﻿using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISchoolService
    {
        public Task<IPaginable<GetSchoolResponse>> GetSchoolPage(PaginationRequest paginationRequest, SchoolFilterRequest filterRequest);
    }
}
