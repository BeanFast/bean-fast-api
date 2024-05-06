using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {

    }
}