namespace DataTransferObjects.Models.Food.Response;

public class GetFoodResponse
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Discription { get; set; }
    public bool IsCombo { get; set; }
    public string ImagePath { get; set; }
    public virtual CategoryOfFood? Category { get; set; }
    // public virtual ICollection<Combo>? MasterCombos { get; set; }
    // public virtual ICollection<Combo>? Combos { get; set; }
    // public virtual ICollection<MenuDetail>? MenuDetails { get; set; }

    public class CategoryOfFood
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    
        public string Code { get; set; }
    }
}

