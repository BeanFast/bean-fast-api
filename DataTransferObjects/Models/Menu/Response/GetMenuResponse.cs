namespace DataTransferObjects.Models.Menu.Response;

public class GetMenuResponse
{
    public Guid Id { get; set; }
    public Guid KitchenId { get; set; }
    public Guid? CreaterId { get; set; }
    public Guid? UpdaterId { get; set; }
    public string Code { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public ICollection<MenuDetailOfGetMenuResponse> MenuDetails { get; set; }
    //public ICollection<SessionOfGetMenuResponse> Sessions { get; set; }

    public class MenuDetailOfGetMenuResponse
    {
        public double Price { get; set; }
        public virtual FoodOfMenuDetail Food { get; set; }
        public class FoodOfMenuDetail
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public bool IsCombo { get; set; }
            public string ImagePath { get; set; }
            public CategoryOfFood Category { get; set; }
            public class CategoryOfFood
            {
                public string Name { get; set; }
            }
        }
    }

    public class SessionOfGetMenuResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderStartTime { get; set; }
        public DateTime OrderEndTime { get; set; }
        public DateTime DeliveryStartTime { get; set; }
        public DateTime DeliveryEndTime { get; set; }
        public ICollection<SessionDetailOfSession> SessionDetails { get; set; }
        public class SessionDetailOfSession
        {
            public LocationOfSessionDetail Location { get; set; }
            public class LocationOfSessionDetail
            {
                public Guid SchoolId { get; set; }
                public string Code { get; set; }
                public string Name { get; set; }
                public string Description { get; set; }
                public string ImagePath { get; set; }
            }
        }
    }
}
