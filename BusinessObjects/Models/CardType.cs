namespace BusinessObjects.Models
{
    public class CardType : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string BackgroundImagePath { get; set; }
        public virtual ICollection<LoyaltyCard>? LoyaltyCards { get; set; }
    }
}
