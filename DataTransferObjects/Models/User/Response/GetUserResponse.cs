using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.User.Response
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }    
        public string? FullName { get; set; }
        public int? Status { get; set; }
        public string? Code { get; set; }
        public string? Phone { get; set; }
        //public string? Password { get; set; }
        public string? Email { get; set; }
        public string? AvatarPath { get; set; }
        public string? RoleName { get; set; }
        public RoleOfGetUserResponse? Role { get; set; }
        public class RoleOfGetUserResponse
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string EnglishName { get; set; }
            public string Description { get; set; }
            public string ShortDescription { get; set; }
        }
    }
}
