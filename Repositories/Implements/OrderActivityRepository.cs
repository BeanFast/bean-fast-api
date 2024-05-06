using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class OrderActivityRepository : GenericRepository<OrderActivity>, IOrderActivityRepository
{
    public OrderActivityRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}