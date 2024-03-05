using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Menu.Request
{
    public class CreateMenuRequest
    {
        [RequiredGuid]
        public Guid KitchenId { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<MenuDetailOfCreateMenuRequest> MenuDetails { get; set; }
        public class MenuDetailOfCreateMenuRequest
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            [RequiredGuid]
            public Guid MenuId { get; set; }
            public double Price { get; set; }
        }
    }
}
