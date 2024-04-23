using BusinessObjects.Models;
using DataTransferObjects.Models.Transaction.Request;
using DataTransferObjects.Models.Transaction.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class TransactionMapper : AutoMapper.Profile
    {
        public TransactionMapper()
        {
            CreateMap<CreateTransactionRequest, Transaction>();
            CreateMap<Transaction, GetTransactionResponse>();
            CreateMap<Transaction, GetTransactionPageByProfileIdAndCurrentUserResponse>();
            CreateMap<Order, GetTransactionPageByProfileIdAndCurrentUserResponse.OrderOfGetTransactionPageByProfileIdAndCurrentUserResponse>();
            CreateMap<Game, GetTransactionPageByProfileIdAndCurrentUserResponse.GameOfGetTransactionPageByProfileIdAndCurrentUserResponse>();
            CreateMap<ExchangeGift, GetTransactionPageByProfileIdAndCurrentUserResponse.ExchangeGiftOfGetTransactionPageByProfileIdAndCurrentUserResponse>();

            CreateMap<Transaction, GetTransactionPageByCurrentUserResponse>();
            CreateMap<Order, GetTransactionPageByCurrentUserResponse.OrderOfGetTransactionPageByProfileIdAndCurrentUserResponse>();
        }
    }
}
