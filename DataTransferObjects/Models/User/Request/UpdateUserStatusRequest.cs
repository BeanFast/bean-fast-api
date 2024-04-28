using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.User.Request
{
    public class UpdateUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
