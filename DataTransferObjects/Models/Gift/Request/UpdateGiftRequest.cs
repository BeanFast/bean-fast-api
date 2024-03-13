using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Gift.Request
{
    public class UpdateGiftRequest
    {
        [Required(ErrorMessage = MessageConstants.GiftMessageConstrant.GiftNameRequired)]
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.GiftMessageConstrant.GiftNameLength)]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = MessageConstants.GiftMessageConstrant.GiftCodeRequired)]
        [Range(1, 9999, ErrorMessage = MessageConstants.GiftMessageConstrant.GiftPointsRange)]
        public int Points { get; set; }
        [Required(ErrorMessage = MessageConstants.GiftMessageConstrant.GiftInStockRequired)]
        [Range(1, 9999, ErrorMessage = MessageConstants.GiftMessageConstrant.GiftInStockRange)]
        public int InStock { get; set; }
        [RequiredFileExtensions(Utilities.Enums.AllowedFileTypes.IMAGE)]
        //[Required]
        public IFormFile? Image { get; set; }
    }
}
