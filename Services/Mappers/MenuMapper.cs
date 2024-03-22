using BusinessObjects.Models;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;
using Profile = AutoMapper.Profile;

namespace Services.Mappers;

public class MenuMapper : Profile
{
    public MenuMapper()
    {
        CreateMap<Menu, GetMenuResponse>();
        CreateMap<MenuDetail, GetMenuResponse.MenuDetailOfGetMenuResponse>();
        CreateMap<Food, GetMenuResponse.MenuDetailOfGetMenuResponse.FoodOfMenuDetail>();
        CreateMap<Category, GetMenuResponse.MenuDetailOfGetMenuResponse.FoodOfMenuDetail.CategoryOfFood>();
        //CreateMap<Kitchen, GetMenuResponse.GetMenuKitchenResponse>();
        

        CreateMap<CreateMenuRequest, Menu>();
        CreateMap<CreateMenuRequest.MenuDetailOfCreateMenuRequest, MenuDetail>();
    }
}