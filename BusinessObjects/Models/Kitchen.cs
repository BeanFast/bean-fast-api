namespace BusinessObjects.Models
{
    public class Kitchen : BaseEntity
    {
        public Guid? AreaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public virtual Area? Area { get; set; }
        public virtual ICollection<School>? PrimarySchools { get; set; }
        public virtual ICollection<Menu>? Menus { get; set; }
    }
}
