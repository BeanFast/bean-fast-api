using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class TransactionRepository : GenericRepository<Transaction>,ITransactionRepository
{
    public TransactionRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}