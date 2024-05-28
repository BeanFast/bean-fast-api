namespace BusinessObjects.Models
{
    public class ExchangeGift : BaseAuditableEntity
    {
        public Guid ProfileId { get; set; }
        public Guid SessionDetailId { get; set; }
        public Guid GiftId { get; set; }
        public Guid? DelivererId { get; set; }
        public string Code { get; set; }
        public int Points { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual SessionDetail? SessionDetail { get; set; }
        public virtual Gift? Gift { get; set; }
        public virtual User? Deliverer { get; set; }
        public virtual ICollection<OrderActivity>? Activities { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
