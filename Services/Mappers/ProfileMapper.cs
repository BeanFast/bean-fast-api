using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles;
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
        }
    }
}
