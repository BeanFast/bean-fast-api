namespace BusinessObjects.Models
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid ProfileId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Balance { get; set; }
        public virtual User? User { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}
