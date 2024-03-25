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
        //Task<bool> ProcessVnPayPayment(Guid customerId, VnPayRequest request);
        //Task<bool> ConfirmVnPayPayment(GetVnPayResponse response);

        string CreatePaymentUrl(HttpContext context, VnPayRequest model);
        GetVnPayResponse PaymentExecute(IQueryCollection collections);
    }
}
