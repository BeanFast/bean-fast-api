using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class BaseAuditableEntity: BaseEntity
    {
        
        public Guid? CreatorId { get; set; }
        public Guid? UpdaterId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual User? Creator { get; set; }
        public virtual User? Updater { get; set; }
    }
}
