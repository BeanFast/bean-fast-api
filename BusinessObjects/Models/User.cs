﻿namespace BusinessObjects.Models
{
    public class User : BaseEntity
    {
        public Guid RoleId { get; set; }
        public string Code { get; set; }
        public string? FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string AvatarPath { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceToken { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Profile>? Profiles { get; set; }
        public virtual ICollection<Menu>? CreatedMenus { get; set; }
        public virtual ICollection<Menu>? UpdatedMenus { get; set; }
        public virtual ICollection<SessionDetail>? SessionDetails { get; set; }
        public virtual ICollection<Wallet>? Wallets { get; set; }
        public virtual ICollection<SmsOtp>? SmsOtps { get; set; }
        public virtual ICollection<NotificationDetail>? NotificationDetails { get; set; }
    }
}
