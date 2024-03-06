using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Food.Request
{
    public class UpdateFoodRequest
    {
        [RequiredGuid]
        public Guid CategoryId { get; set; } = default!;

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public IFormFile? Image { get; set; }
        
        public List<UpdateFoodCombo>? Combos { get; set; }

        public class UpdateFoodCombo
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
