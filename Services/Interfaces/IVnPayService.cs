using DataTransferObjects.Models.VnPay.Request;
using DataTransferObjects.Models.VnPay.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPayRequest request);
        Task<GetVnPayResponse> PaymentExecute(IQueryCollection collections);
    }
}
