using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects.Models.Food.Request
{
    public class CreateFoodRequest
    {
        public string Name { get; set; } = default!;
        public double Price { get; set; }
        public string Description { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public IList<CreateFoodCombo>? Combos { get; set; }
        public IFormFile Image { get; set; } = default!;

        public class CreateFoodCombo
        {
            public Guid FoodId { get; set; }
            public int Quantity { get; set; }
        }
    }
}