using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class NotificationDetail : BaseEntity
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public virtual User User { get; set; }
        public virtual Notification Notification { get; set; }
        
    }
}
