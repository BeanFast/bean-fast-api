using AutoMapper;
using DataTransferObjects.Models.Kitchen.Response;
using DataTransferObjects.Models.Kitchen.Request;

namespace Services.Mappers;

public class KitchenMapper : Profile
{
    public KitchenMapper()
    {
        CreateMap<CreateKitchenRequest, BusinessObjects.Models.Kitchen>();
        CreateMap<BusinessObjects.Models.Kitchen, GetKitchenResponse>();
        CreateMap<BusinessObjects.Models.Area, GetKitchenResponse.AreaOfKitchen>();


    }
}
