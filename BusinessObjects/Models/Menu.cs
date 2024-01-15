namespace BusinessObjects.Models
{
    public class Menu : BaseEntity
    {
        public Guid KitchenId { get; set; }
        public Guid? CreaterId { get; set; }
        public Guid? UpdaterId { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual User? Creater { get; set; }
        public virtual User? Updater { get; set; }
        public virtual Kitchen? Kitchen { get; set; }
        public virtual ICollection<Session>? Sessions { get; set; }
        public virtual ICollection<MenuDetail>? MenuDetails { get; set; }
    }
}
