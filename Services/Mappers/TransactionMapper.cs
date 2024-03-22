using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.Transaction.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Mappers
{
    public class TransactionMapper : AutoMapper.Profile
    {
        public TransactionMapper()
        {
            CreateMap<CreateTransactionRequest, Transaction>();
            CreateMap<Transaction, GetTransactionResponse>();
        }
    }
}
