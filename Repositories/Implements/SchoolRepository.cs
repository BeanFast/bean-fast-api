using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class SchoolRepository : GenericRepository<School>, ISchoolRepository
{
    public SchoolRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}