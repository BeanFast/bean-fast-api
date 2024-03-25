using DataTransferObjects.Models.School.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataTransferObjects.Models.Kitchen.Response.GetKitchenResponse;

namespace DataTransferObjects.Models.Profiles.Response
{
    public class GetProfileResponse
    {
        public string FullName { get; set; }
        public string? NickName { get; set; }
        public string AvatarPath { get; set; }
        public DateTime Dob { get; set; }
        public string? Class { get; set; }
        public bool Gender { get; set; }
        public double? CurrentBMI { get; set; }
        public SchoolOfGetProfileResponse School { get; set; } 
        public WalletOfGetProfileResponse Wallet { get; set; }
        public ICollection<LoyaltyCardOfGetProfileResponse> LoyaltyCards { get; set; }  
        public class SchoolOfGetProfileResponse
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string ImagePath { get; set; }
            //public virtual KitchenOfSchool? Kitchen { get; set; }
        }

        public class KitchenOfSchool
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public string Address { get; set; }
            public virtual ICollection<MenuOfKitchen> Menus { get; set; }
        }

        public class MenuOfKitchen
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public DateTime? CreateDate { get; set; }
            public DateTime? UpdateDate { get; set; }
            public virtual ICollection<MenuDetailOfMenu> MenuDetails { get; set; }
        }

        public class MenuDetailOfMenu
        {
            public Guid Id { get; set; }
            public Guid FoodId { get; set; }
            public string Code { get; set; }
            public double Price { get; set; }
        }
        public class WalletOfGetProfileResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public double Balance { get; set; }
        }
        public class LoyaltyCardOfGetProfileResponse
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string QRCode { get; set; }
            public string BackgroundImagePath { get; set; }
        }
    }
}
