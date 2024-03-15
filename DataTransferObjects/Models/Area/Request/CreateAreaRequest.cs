using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace DataTransferObjects.Models.Area.Request
{
    internal class CreateAreaRequest
    {
        [Required(ErrorMessage = MessageConstants.AreaMessageConstrant.AreaCityRequired)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = MessageConstants.AreaMessageConstrant.AreaCityLength)]
        public string City { get; set; } = "";
        [Required(ErrorMessage = MessageConstants.AreaMessageConstrant.AreaDistrictRequired)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = MessageConstants.AreaMessageConstrant.AreaDistrictLength)]
        public string District { get; set; } = "";
        [Required(ErrorMessage = MessageConstants.AreaMessageConstrant.AreaWardRequired)]
        [StringLength(50, MinimumLength = 1, ErrorMessage = MessageConstants.AreaMessageConstrant.AreaWardLength)]
        public string Ward { get; set; } = "";
    }
}
