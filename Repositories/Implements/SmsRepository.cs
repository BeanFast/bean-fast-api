using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class SmsRepository : GenericRepository<SmsOtp>, ISmsRepository
{
    public SmsRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}