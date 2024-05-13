using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles.Request;
using DataTransferObjects.Models.Profiles.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Utils;

namespace Services.Mappers
{
    public class ProfileMapper : AutoMapper.Profile
    {
        public ProfileMapper()
        {
            CreateMap<CreateProfileRequest, Profile>();
            CreateMap<CreateProfileRequest.BMIOfProfile, ProfileBodyMassIndex>();

            CreateMap<UpdateProfileRequest, Profile>();
            CreateMap<UpdateProfileRequest.BMIOfProfile, ProfileBodyMassIndex>();

            CreateMap<Profile, GetProfilesByCurrentCustomerResponse>()
                .ForMember(dest => dest.Bmi, opt => opt.MapFrom(src => src.BMIs!.OrderByDescending(bmi => bmi.RecordDate).FirstOrDefault()))
                .ForMember(dest => dest.CurrentBMI, opt => opt.MapFrom(src => Math.Round(src.CurrentBMI!.Value, 2)));
            CreateMap<School, GetProfilesByCurrentCustomerResponse.SchoolOfGetProfilesByCurrentCustomerResponse>();
            CreateMap<ProfileBodyMassIndex, GetProfilesByCurrentCustomerResponse.BmiOfProfile>();

            CreateMap<Profile, GetProfileResponse>()
                .ForMember(dest => dest.Wallet, opt => opt.MapFrom(src => src.User!.Wallets!.First()))
                .ForMember(dest => dest.Bmi, opt => opt.MapFrom(src => src.BMIs!.OrderByDescending(bmi => bmi.RecordDate).FirstOrDefault()))
                .ForMember(dest => dest.CurrentBMI, opt => opt.MapFrom(src => Math.Round(src.CurrentBMI!.Value, 2)));
            CreateMap<ProfileBodyMassIndex, GetProfileResponse.BmiOfProfile>();
            //.ForMember(dest => dest.BMIStatus, opt => opt.MapFrom(src => BmiUltil.GetBMIStatus(src.CurrentBMI!.Value, src.Gender, src.Dob)));
            CreateMap<Wallet, GetProfileResponse.WalletOfGetProfileResponse>() ;
            CreateMap<School, GetProfileResponse.SchoolOfGetProfileResponse>();
            CreateMap<Kitchen, GetProfileResponse.KitchenOfSchool>();
            CreateMap<Menu, GetProfileResponse.MenuOfKitchen>();
            CreateMap<MenuDetail, GetProfileResponse.MenuDetailOfMenu>();
        }
    }
}