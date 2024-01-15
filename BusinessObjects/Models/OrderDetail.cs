namespace BusinessObjects.Models
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid FoodId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? Note { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Food? Food { get; set; }
    }
}
