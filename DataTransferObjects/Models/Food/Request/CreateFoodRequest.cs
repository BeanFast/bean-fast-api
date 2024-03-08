using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Food.Request
{
    public class CreateFoodRequest
    {
        public string Name { get; set; } = default!;
        [Range(1, Double.MaxValue)]
        public double Price { get; set; }
        public string Description { get; set; } = default!;
        [RequiredGuid]
        public Guid CategoryId { get; set; }
        public IList<CreateFoodCombo>? Combos { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; } = default!;

        public class CreateFoodCombo
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            [Range(1, Int32.MaxValue)]
            public int Quantity { get; set; }
        }
    }
}