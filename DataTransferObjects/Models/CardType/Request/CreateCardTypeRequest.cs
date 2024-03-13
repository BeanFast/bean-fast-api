using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.CardType.Request
{
    public class CreateCardTypeRequest
    {
        [Required(ErrorMessage = MessageConstants.CardTypeMessageConstrant.CardTypeNameRequired)]
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.CardTypeMessageConstrant.CardTypeNameLength)]
        public string Name { get; set; }
        [Required(ErrorMessage = MessageConstants.CardTypeMessageConstrant.CardTypeHeightRequired)]
        [Range(1, 10, ErrorMessage = MessageConstants.CardTypeMessageConstrant.CardTypeHeightRange)]
        public double Height { get; set; }
        [Required(ErrorMessage = MessageConstants.CardTypeMessageConstrant.CardTypeWidthRequired)]
        [Range(1, 10, ErrorMessage = MessageConstants.CardTypeMessageConstrant.CardTypeWidthRange)]
        public double Width { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
    }
}
