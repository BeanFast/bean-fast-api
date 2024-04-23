using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Request
{
    public class CancelOrderRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
