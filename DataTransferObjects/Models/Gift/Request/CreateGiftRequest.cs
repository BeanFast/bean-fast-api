using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Gift.Request
{
    public class CreateGiftRequest
    {
        //public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int Points { get; set; } 
        public int InStock { get; set; }
        [RequiredFileExtensions(Utilities.Enums.AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
    }
}
