
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
namespace Services.Mappers;

public class FoodMapper : AutoMapper.Profile
{
    public FoodMapper()
    {
        CreateMap<Food, GetFoodResponse>().ForMember(dest => dest.Combos, opt => opt.MapFrom(src => src.MasterCombos));
        CreateMap<Food, GetBestSellerFoodsResponse>().ForMember(dest => dest.SoldCount, opt => opt.MapFrom(src => src.OrderDetails!.Count()));
        CreateMap<Combo, GetFoodResponse.ComboOfFood>();
        CreateMap<Category, GetFoodResponse.CategoryOfFood>().ReverseMap();
        CreateMap<CreateFoodRequest, Food>();
        CreateMap<CreateFoodRequest.CreateFoodCombo, Combo>();



    }
}