using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace DataTransferObjects.Models.Kitchen.Request
{
    public class KitchenFilterRequest
    {
        [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.KitchenMessageConstrant.KitchenCodeLength)]
        public string? Code { get; set; }
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.KitchenMessageConstrant.KitchenNameLength)]
        public string? Name { get; set; }
        public Guid? AreaId { get; set; }
    }
}
