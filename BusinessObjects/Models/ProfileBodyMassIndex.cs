namespace BusinessObjects.Models
{
    public class ProfileBodyMassIndex : BaseEntity
    {
        public Guid ProfileId { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public int Age { get; set; }
        public DateTime RecordDate { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}
