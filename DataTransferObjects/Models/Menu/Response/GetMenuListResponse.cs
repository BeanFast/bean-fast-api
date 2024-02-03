namespace DataTransferObjects.Models.Menu.Response;

public class GetMenuListResponse
{
    public Guid Id { get; set; }
    public Guid KitchenId { get; set; }
    public Guid? CreaterId { get; set; }
    public Guid? UpdaterId { get; set; }
    public string Code { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public GetMenuKitchenResponse? Kitchen { get; set; }

    public class GetMenuKitchenResponse
    {
        public Guid Id { get; set; }
        public Guid AreaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
    }
}
