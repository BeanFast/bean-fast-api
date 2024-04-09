using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Menu.Request
{
    public class UpdateMenuRequest
    {
        [RequiredGuid]
        public Guid KitchenId { get; set; }
        [RequiredListLength(max: 20)]
        public List<MenuDetailOfUpdateMenuRequest> MenuDetails { get; set; }

        public class MenuDetailOfUpdateMenuRequest
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            [Required(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRequired)]
            [Range(1000, 500000, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRange)]
            public double Price { get; set; }
        }
    }
}
