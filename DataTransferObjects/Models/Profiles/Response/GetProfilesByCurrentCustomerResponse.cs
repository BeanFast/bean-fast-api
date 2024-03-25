using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Profiles.Response
{
    public class GetProfilesByCurrentCustomerResponse
    {
        public string FullName { get; set; }
        public string? NickName { get; set; }
        public string AvatarPath { get; set; }
        public DateTime Dob { get; set; }
        public string? Class { get; set; }
        public bool Gender { get; set; }
        public double CurrentBMI { get; set; }\
        public string SchoolName { get; set; }  
    }
}
