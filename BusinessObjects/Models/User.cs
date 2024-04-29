namespace BusinessObjects.Models
{
    public class User : BaseAuditableEntity
    {
        public Guid RoleId { get; set; }
        public string Code { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string AvatarPath { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceToken { get; set; }
        public string? QRCode { get; set; }
        public DateTime? QrCodeExpiry { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Profile>? Profiles { get; set; }
        public virtual ICollection<Area>? CreatedAreas { get; set; }
        public virtual ICollection<Area>? UpdatedAreas { get; set; }
        public virtual ICollection<CardType>? CreatedCardTypes { get; set; }
        public virtual ICollection<CardType>? UpdatedCardTypes { get; set; }

        public virtual ICollection<Category>? CreatedCategories { get; set; }
        public virtual ICollection<Category>? UpdatedCategories { get; set; }
        public virtual ICollection<Combo>? CreatedCombos { get; set; }
        public virtual ICollection<Combo>? UpdatedCombos { get; set; }
        public virtual ICollection<ExchangeGift>? CreatedExchangeGifts { get; set; }
        public virtual ICollection<ExchangeGift>? UpdatedExchangeGifts { get; set; }
        public virtual ICollection<Food>? CreatedFoods { get; set; }
        public virtual ICollection<Food>? UpdatedFoods { get; set; }
        public virtual ICollection<Game>? CreatedGames { get; set; }
        public virtual ICollection<Game>? UpdatedGames { get; set; }
        public virtual ICollection<Gift>? CreatedGifts { get; set; }
        public virtual ICollection<Gift>? UpdatedGifts { get; set; }
        public virtual ICollection<Kitchen>? CreatedKitchens { get; set; }
        public virtual ICollection<Kitchen>? UpdatedKitchens { get; set; }
        
        public virtual ICollection<Location>? CreatedLocations { get; set; }
        public virtual ICollection<Location>? UpdatedLocations { get; set; }
        
        public virtual ICollection<LoyaltyCard>? CreatedLoyaltyCards { get; set; }
        public virtual ICollection<LoyaltyCard>? UpdatedLoyaltyCards { get; set; }
        public virtual ICollection<Menu>? CreatedMenus { get; set; }
        public virtual ICollection<Menu>? UpdatedMenus { get; set; }
        public virtual ICollection<Order>? CreatedOrders { get; set; }
        public virtual ICollection<Order>? UpdatedOrders { get; set; }
        
        public virtual ICollection<OrderActivity>? CreatedOrderActivities { get; set; }
        public virtual ICollection<OrderActivity>? UpdatedOrderActivities { get; set; }
        
        public virtual ICollection<School>? CreatedSchools { get; set; }
        public virtual ICollection<School>? UpdatedSchools { get; set; }
        
        public virtual ICollection<Session>? CreatedSessions { get; set; }
        public virtual ICollection<Session>? UpdatedSessions { get; set; }

        public virtual ICollection<SessionDetailDeliverer>? CreatedSessionDetailDeliverer { get; set; }
        public virtual ICollection<SessionDetailDeliverer>? UpdatedSessionDetailDeliverer { get; set; }

        public virtual ICollection<User>? CreatedUsers { get; set; }
        public virtual ICollection<User>? UpdatedUsers { get; set; }

        public virtual ICollection<Wallet>? Wallets { get; set; }
        public virtual ICollection<SessionDetailDeliverer>? SessionDetailDeliverers { get; set; }
        public virtual ICollection<SmsOtp>? SmsOtps { get; set; }
        public virtual ICollection<NotificationDetail>? NotificationDetails { get; set; }
    }
}
