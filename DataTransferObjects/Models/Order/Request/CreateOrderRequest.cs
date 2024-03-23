
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Order.Request
{
    public class CreateOrderRequest
    {
        [RequiredGuid]
        public Guid SessionDetailId { get; set; }
        [RequiredGuid]
        public Guid ProfileId { get; set; }

        public ICollection<OrderDetailOfCreateOrderRequest>? OrderDetails { get; set; }

        public class OrderDetailOfCreateOrderRequest
        {
            [Required]
            public int Quantity { get; set; }

            public string? Note { get; set; }
            public Guid MenuDetailId { get; set; }

        }
    }
}
