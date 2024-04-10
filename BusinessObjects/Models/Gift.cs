namespace BusinessObjects.Models
{
    public class Gift : BaseAuditableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int InStock { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<ExchangeGift>? ExchangeGifts { get; set; }
    }
}
