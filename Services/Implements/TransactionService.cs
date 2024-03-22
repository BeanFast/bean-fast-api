using AutoMapper;
using BusinessObjects;
using DataTransferObjects.Models.Transaction.Request;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;

namespace Services.Implements
{
    public class TransactionService : BaseService<BusinessObjects.Models.Transaction>, ITransactionService
    {
        public TransactionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public Task CreateTransactionAsync(CreateTransactionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
