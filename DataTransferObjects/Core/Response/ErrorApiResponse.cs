using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Core.Response
{
    public class ErrorApiResponse : BaseApiResponse
    {
        public string Description { get; set; }
    }
}
