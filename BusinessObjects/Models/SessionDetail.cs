namespace BusinessObjects.Models
{
    public class SessionDetail : BaseEntity
    {
        public Guid LocationId { get; set; }
        public Guid SessionId { get; set; }
        //public Guid? DelivererId { get; set; }
        public string Code { get; set; }
        public virtual Location? Location { get; set; }
        public virtual Session? Session { get; set; }
        //public virtual User? Deliverer { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<SessionDetailDeliverer>? SessionDetailDeliverers { get; set; }
        public virtual ICollection<ExchangeGift>? ExchangeGifts { get; set; }
    }
}
