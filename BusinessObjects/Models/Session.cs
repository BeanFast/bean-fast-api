namespace BusinessObjects.Models
{
    public class Session : BaseEntity
    {
        public Guid MenuId { get; set; }
        public string Code { get; set; }
        public DateTime OrderStartTime { get; set; }
        public DateTime OrderEndTime { get; set; }
        public DateTime DeliveryStartTime { get; set; }
        public DateTime DeliveryEndTime { get; set; }
        public virtual Menu? Menu { get; set; }
        public virtual ICollection<SessionDetail>? SessionDetails { get; set; }
    }
}
