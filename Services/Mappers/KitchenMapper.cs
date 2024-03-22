using AutoMapper;
using DataTransferObjects.Models.Kitchen.Response;
using DataTransferObjects.Models.Kitchen.Request;
using BusinessObjects.Models;

namespace Services.Mappers;

public class KitchenMapper : AutoMapper.Profile
{
    public KitchenMapper()
    {
        CreateMap<CreateKitchenRequest, Kitchen>();
        CreateMap<Kitchen, GetKitchenResponse>();
        CreateMap<Area, GetKitchenResponse.AreaOfKitchen>();
    }
}
