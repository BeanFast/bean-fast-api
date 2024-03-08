using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace DataTransferObjects.Models.Area.Request
{
    public class AreaFilterRequest
    {
        [Required(ErrorMessage = MessageConstants.AreaMessageConstrant.AreaCityRequired)]
        public string City { get; set; } = "";

        public string District { get; set; } = "";

        public string Ward { get; set; } = "";


    }
}
