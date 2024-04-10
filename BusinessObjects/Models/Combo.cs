namespace BusinessObjects.Models
{
    public class Combo : BaseAuditableEntity
    {
        public Guid MasterFoodId { get; set; }
        public Guid FoodId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
        public virtual Food? MasterFood { get; set; }
        public virtual Food? Food { get; set; }
    }
}
