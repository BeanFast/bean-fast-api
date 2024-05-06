using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        
    }
}
