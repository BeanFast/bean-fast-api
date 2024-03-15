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
    public class CreateMenuRequest
    {
        [RequiredGuid]
        public Guid KitchenId { get; set; }
        [Required(ErrorMessage = MessageConstants.MenuMessageContrant.MenuCodeRequired)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCodeLength)]
        public string Code { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCreateDateInvalid)]
        public DateTime? CreateDate { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.MenuMessageContrant.MenuUpdateDateInvalid)]
        public DateTime? UpdateDate { get; set; }
        public List<MenuDetailOfCreateMenuRequest> MenuDetails { get; set; }
        public class MenuDetailOfCreateMenuRequest
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            [RequiredGuid]
            public Guid MenuId { get; set; }
            [Required(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRequired)]
            [Range(1000, 500000, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRange)]
            public double Price { get; set; }
        }
    }
}
