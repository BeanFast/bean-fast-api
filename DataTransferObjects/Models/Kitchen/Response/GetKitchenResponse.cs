using DataTransferObjects.Models.Menu.Response;

namespace DataTransferObjects.Models.Kitchen.Response;

public class GetKitchenResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string Address { get; set; }
    public virtual AreaOfKitchen? Area { get; set; }

    public class AreaOfKitchen
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
    }
}