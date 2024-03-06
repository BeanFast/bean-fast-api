namespace DataTransferObjects.Models.Food.Response;

public class GetFoodResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public bool IsCombo { get; set; }
    public string ImagePath { get; set; }
    public virtual CategoryOfFood? Category { get; set; }
    public List<ComboOfFood> Combos { get; set; }
    // public virtual ICollection<Combo>? MasterCombos { get; set; }
    // public virtual ICollection<Combo>? Combos { get; set; }
    // public virtual ICollection<MenuDetail>? MenuDetails { get; set; }

    public class CategoryOfFood
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    
        public string Code { get; set; }

    }

    public class ComboOfFood
    {
        public Guid MasterFoodId { get; set; }
        public Guid FoodId { get; set; }
        public string Code { get; set; }
        public int Quantity { get; set; }
    }
}

