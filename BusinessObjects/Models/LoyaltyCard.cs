namespace BusinessObjects.Models
{
    public class LoyaltyCard : BaseEntity
    {
        public Guid ProfileId { get; set; }
        public Guid CardTypeId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string QRCode { get; set; }
        public string BackgroundImagePath { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual CardType? CardType { get; set; }
        public virtual ICollection<ExchangeGift>? ExchangeGifts { get; set; }
    }
}
