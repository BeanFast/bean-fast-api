using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Profiles.Response
{
    public class GetProfilesByCurrentCustomerResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? NickName { get; set; }
        public string AvatarPath { get; set; }
        public DateTime Dob { get; set; }
        public string? Class { get; set; }
        public bool Gender { get; set; }
        public double CurrentBMI { get; set; }
        public SchoolOfGetProfilesByCurrentCustomerResponse School { get; set; }
        public BmiOfProfile Bmi { get; set; }
        public class BmiOfProfile
        {
            public double Height { get; set; }
            public double Weight { get; set; }
            public DateTime RecordDate { get; set; }
        }
        public class SchoolOfGetProfilesByCurrentCustomerResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            //public virtual KitchenOfSchool? Kitchen { get; set; }
        }
    }
}
