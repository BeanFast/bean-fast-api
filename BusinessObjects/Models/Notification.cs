using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        //public string? Type { get; set; }
        public string? Link { get; set; }
        public virtual ICollection<NotificationDetail> NotificationDetails { get; set; }
    }
}
