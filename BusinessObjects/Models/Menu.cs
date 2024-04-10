namespace BusinessObjects.Models
{
    public class Menu : BaseAuditableEntity
    {
        public Guid KitchenId { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual Kitchen? Kitchen { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
        public virtual ICollection<MenuDetail>? MenuDetails { get; set; }
    }
}
