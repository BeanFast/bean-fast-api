namespace BusinessObjects.Models
{
    public class Role : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
