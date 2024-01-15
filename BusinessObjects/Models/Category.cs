namespace BusinessObjects.Models
{
    public class Category : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<Food>? Foods { get; set; }
    }
}
