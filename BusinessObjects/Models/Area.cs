namespace BusinessObjects.Models
{
    public class Area : BaseEntity
    {
        public string Code { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public virtual ICollection<School>? PrimarySchools { get; set; }
        public virtual ICollection<Kitchen>? Kitchens { get; set; }
    }
}
