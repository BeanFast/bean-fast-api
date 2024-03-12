using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Profiles
{
    public class CreateProfileRequest
    {
        [RequiredGuid]
        public Guid SchoolId { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public DateTime Dob { get; set; }
        public string Class { get; set; }
        public bool Gender { get; set; }
        public IFormFile Image { get; set; }

        public BMIOfProfile Bmi { get; set; }

        public class BMIOfProfile
        {
            public double Height { get; set; }
            public double Weight { get; set; }
            public double Age { get; set; }
        }
    }
}
