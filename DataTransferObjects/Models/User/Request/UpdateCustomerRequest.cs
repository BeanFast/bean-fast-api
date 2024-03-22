using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.User.Request
{
    public class UpdateCustomerRequest
    {
        public string FullName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
