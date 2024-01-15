namespace BusinessObjects.Models
{
    public class Transaction : BaseEntity
    {
        public Guid? OrderId { get; set; }
        public Guid? ExchangeGiftId { get; set; }
        public Guid WalletId { get; set; }
        public string Code { get; set; }
        public DateTime Time { get; set; }
        public double Value { get; set; }
        public virtual Order? Order { get; set; }
        public virtual ExchangeGift? ExchangeGift { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
