using System.ComponentModel.DataAnnotations;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Food.Request;

public class FoodFilterRequest
{
    //[RequiredGuid(ErrorMessage = MessageConstants.FoodMessageConstrant.FoodCategoryIdRequired)]
    public Guid? CategoryId { get; set; }
    [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodCodeLength)]
    public string? Code { get; set; }
    [StringLength(200, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodNameLength)]
    public string? Name { get; set; }
    [Range(1000, 500000, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRange)]
    public double? MinPrice { get; set; }
    [Range(1000, 500000, ErrorMessage = MessageConstants.FoodMessageConstrant.FoodPriceRange)]
    public double? MaxPrice { get; set; }
    public string? Description { get; set; }
}