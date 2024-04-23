using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.SessionDetail.Request
{
    public class UpdateSessionDetailRequest
    {
        //[RequiredGuid]
        //public Guid LocationId { get; set; }
        //[RequiredGuid]
        public ICollection<Guid> DelivererIds { get; set; }
    }
}
