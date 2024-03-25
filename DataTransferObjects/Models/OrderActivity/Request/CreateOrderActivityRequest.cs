using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.OrderActivity.Request
{
    public class CreateOrderActivityRequest
    {
        public Guid? OrderId { get; set; }
        public Guid? ExchangeGiftId { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
