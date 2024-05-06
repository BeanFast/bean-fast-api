using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
{
    public OrderDetailRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}