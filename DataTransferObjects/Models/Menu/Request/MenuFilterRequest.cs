using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Menu.Request
{
    public class MenuFilterRequest
    {
        public Guid KitchenId { get; set; }
        public Guid? CreaterId { get; set; }
        public Guid? UpdaterId { get; set; }
        public string? Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
