namespace DataTransferObjects.Models.Food.Request;

public class FoodFilterRequest
{
    public Guid? CategoryId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public string? Description { get; set; }
}