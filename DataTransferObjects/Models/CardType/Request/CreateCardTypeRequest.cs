using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.CardType.Request
{
    public class CreateCardTypeRequest
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public IFormFile Image { get; set; }
    }
}
