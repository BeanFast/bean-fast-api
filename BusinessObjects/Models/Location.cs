namespace BusinessObjects.Models
{
    public class Location : BaseEntity
    {
        public Guid SchoolId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<SessionDetail>? SessionDetails { get; set; }
    }
}
