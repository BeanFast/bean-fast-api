<<<<<<< HEAD
﻿using BusinessObjects.Models;
using DataTransferObjects.Models.Transaction.Request;
using System;
=======
﻿using System;
>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
<<<<<<< HEAD
    public interface ITransactionService
    {
        Task CreateTransactionAsync(Transaction transaction);
        Task CreateTransactionListAsync(List<Transaction> transactions);
=======
    public interface ITransactionService : IBaseService
    {
>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
    }
}
