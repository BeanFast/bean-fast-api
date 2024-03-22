using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class SmsOtp : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string Value { get; set; }
        public virtual User User { get; set; }
    }
}
