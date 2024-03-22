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
}
