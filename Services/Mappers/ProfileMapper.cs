using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles.Request;
using DataTransferObjects.Models.Profiles.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class ProfileMapper : AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<CreateProfileRequest, Profile>();
            CreateMap<CreateProfileRequest.BMIOfProfile, ProfileBodyMassIndex>();
            CreateMap<Profile, GetProfilesByCurrentCustomerResponse>();
            CreateMap<Profile, GetProfileResponse>();
            CreateMap<School, GetProfileResponse.SchoolOfProfile>();
            CreateMap<Kitchen, GetProfileResponse.KitchenOfSchool>();
            CreateMap<Menu, GetProfileResponse.MenuOfKitchen>();
            CreateMap<MenuDetail, GetProfileResponse.MenuDetailOfMenu>();
        }
    }
}