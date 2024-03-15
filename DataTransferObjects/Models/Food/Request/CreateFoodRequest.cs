using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Http;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Food.Request
{
    public class CreateFoodRequest
    {
        [Required(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodNameRequired)]
        [StringLength(200, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodNameLength)]
        public string Name { get; set; } = default!;
        [Required(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRequired)]
        [Range(1000, 500000, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRange)]
        public double Price { get; set; }
        [Required(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodDescriptionRequired)]
        public string Description { get; set; } = default!;
        [RequiredGuid(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodCategoryIdRequired)]
        public Guid CategoryId { get; set; }
        public IList<CreateFoodCombo>? Combos { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; } = default!;

        public class CreateFoodCombo
        {
            [RequiredGuid]
            public Guid FoodId { get; set; }
            [Required(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodQuantityRequired)]
            [Range(1, 999, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodQuantityRange)]
            public int Quantity { get; set; }
        }
    }
}