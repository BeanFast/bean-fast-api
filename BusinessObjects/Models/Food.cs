namespace BusinessObjects.Models
{
    public class Food : BaseAuditableEntity
    {
        public Guid CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsCombo { get; set; }
        public string ImagePath { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<Combo>? MasterCombos { get; set; }
        public virtual ICollection<Combo>? Combos { get; set; }
        public virtual ICollection<MenuDetail>? MenuDetails { get; set; }
    }
}
