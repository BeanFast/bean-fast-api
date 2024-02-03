using BusinessObjects.Models;
using DataTransferObjects.Models.Menu.Response;
using Profile = AutoMapper.Profile;

namespace Services.Mappers;

public class MenuMapper : Profile
{
    public MenuMapper()
    {
        CreateMap<Menu, GetMenuListResponse>();
        CreateMap<Kitchen, GetMenuListResponse.GetMenuKitchenResponse>();
    }
}