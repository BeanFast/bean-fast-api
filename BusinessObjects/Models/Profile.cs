namespace BusinessObjects.Models
{
    public class Profile : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid SchoolId { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string? NickName { get; set; }
        public string AvatarPath { get; set; }
        public DateTime Dob { get; set; }
        public string? Class { get; set; }
        public bool Gender { get; set; }
        public double? CurrentBMI { get; set; }
        public virtual User? User { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<Wallet>? Wallets { get; set; }
        public virtual ICollection<ProfileBodyMassIndex>? BMIs { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<ExchangeGift>? ExchangeGifts { get; set; }
        public virtual ICollection<LoyaltyCard>? LoyaltyCards { get; set; }
    }
}
