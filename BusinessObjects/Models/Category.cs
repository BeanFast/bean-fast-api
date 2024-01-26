using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public class Category : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [JsonIgnore]
        public virtual ICollection<Food>? Foods { get; set; }
    }
}
