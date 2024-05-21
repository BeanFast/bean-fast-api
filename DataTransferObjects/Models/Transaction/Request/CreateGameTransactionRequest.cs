using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Transaction.Request
{
    public class CreateGameTransactionRequest
    {
        [RequiredGuid]
        public Guid GameId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = MessageConstants.TransactionMessageConstrant.PointMustBeGreaterThanZero)]
        public int Points { get; set; }
    }
}
