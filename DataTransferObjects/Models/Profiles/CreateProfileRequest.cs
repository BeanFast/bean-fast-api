using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Profiles
{
    public class CreateProfileRequest
    {
        [RequiredGuid]
        public Guid SchoolId { get; set; }
        [Required(ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileFullNameRequired)]
        [StringLength(200, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileFullNameLength)]
        public string FullName { get; set; }
        [StringLength(50, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileNickNameLength)]
        public string NickName { get; set; }
        [Required(ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileDobRequired)]
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileDobInvalid)]
        public DateTime Dob { get; set; }
        [Required(ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileClassRequired)]
        [StringLength(20, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileClassLength)]
        public string Class { get; set; }
        public bool Gender { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }

        public BMIOfProfile Bmi { get; set; }

        public class BMIOfProfile
        {
            [Range(1, 300, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileHeightRange)]
            public double Height { get; set; }
            [Range(1, 300, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileWeightRange)]
            public double Weight { get; set; }
            [Range(1, 100, ErrorMessage = MessageConstants.ProfileMessageConstrant.ProfileAgeRange)]
            public double Age { get; set; }
        }
    }
}
