namespace BusinessObjects.Models
{
    public class School : BaseAuditableEntity
    {
        public Guid AreaId { get; set; }
        public Guid? KitchenId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
        public virtual Area? Area { get; set; }
        public virtual Kitchen? Kitchen { get; set; }
        public virtual ICollection<Profile>? Profiles { get; set; }
        public virtual ICollection<Location>? Locations { get; set; }
    }
}
