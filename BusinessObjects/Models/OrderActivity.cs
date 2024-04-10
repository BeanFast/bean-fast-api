namespace BusinessObjects.Models
{
    public class OrderActivity : BaseAuditableEntity
    {
        public Guid? OrderId { get; set; }
        public Guid? ExchangeGiftId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string? ImagePath { get; set; }
        public virtual Order? Order { get; set; }
        public virtual ExchangeGift? ExchangeGift { get; set; }
    }
}
