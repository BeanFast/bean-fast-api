namespace BusinessObjects.Models
{
    public class Order : BaseAuditableEntity
    {
        public Guid DelivererId { get; set; }
        public Guid SessionDetailId { get; set; }
        public Guid ProfileId { get; set; }
        public string Code { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int RewardPoints { get; set; }
        public string? Feedback { get; set; }
        public virtual SessionDetail? SessionDetail { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual User? Deliverer { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
        public virtual ICollection<OrderActivity>? OrderActivities { get; set; }
    }
}
