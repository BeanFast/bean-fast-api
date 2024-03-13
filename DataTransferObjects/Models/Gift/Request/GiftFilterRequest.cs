using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace DataTransferObjects.Models.Gift.Request
{
    public class GiftFilterRequest
    {
        [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.GiftMessageConstrant.GiftCodeLength)]
        public string? Code { get; set; }
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.GiftMessageConstrant.GiftNameLength)]
        public string? Name { get; set; }
        [Range(1, 9999, ErrorMessage = MessageConstants.GiftMessageConstrant.GiftPointsRange)]
        public int? Points { get; set; }
    }
}
