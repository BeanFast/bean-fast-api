namespace BusinessObjects.Models
{
    public class MenuDetail : BaseEntity
    {
        public Guid FoodId { get; set; }
        public Guid MenuId { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public virtual Food? Food { get; set; }
        public virtual Menu? Menu { get; set; }

    }
}
