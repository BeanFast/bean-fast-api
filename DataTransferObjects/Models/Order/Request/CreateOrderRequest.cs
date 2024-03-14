using BusinessObjects.Models;
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
        [Required(ErrorMessage = MessageConstants.OrderMessageConstrant.OrderTotalPriceRequired)]
        [Range(1000, double.MaxValue, ErrorMessage = MessageConstants.OrderMessageConstrant.OrderTotalPriceRange)]
        public double TotalPrice { get; set; }
        public IList<OrderDetailList>? OrderDetails { get; set; }

        public class OrderDetailList
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            [Required]
            public int Quantity { get; set; }
            public double Price { get; set; }
            public string? Note { get; set; }
        }



    }
}
