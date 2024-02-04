
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
namespace Services.Mappers;

public class FoodMapper : AutoMapper.Profile
{
    public FoodMapper()
    {
        CreateMap<Food, GetFoodResponse>()
            .ReverseMap();
        CreateMap<CreateFoodRequest, Food>();
        CreateMap<CreateFoodRequest.CreateFoodCombo, Combo>();

        CreateMap<Category, GetFoodResponse.CategoryOfFood>()
            .ReverseMap();
        
    }
}