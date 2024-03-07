using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.CardType.Request
{
    public class UpdateCardTypeRequest
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        [RequiredFileExtensions(Utilities.Enums.AllowedFileTypes.IMAGE)]
        public IFormFile? Image { get; set; }
    }
}
