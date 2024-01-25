namespace BusinessObjects.Models
{
    public class ProfileBodyMassIndex : BaseEntity
    {
        public Guid ProfileId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public DateTime RecordDate { get; set; }
        public virtual Profile? Profile { get; set; }
    }
}
