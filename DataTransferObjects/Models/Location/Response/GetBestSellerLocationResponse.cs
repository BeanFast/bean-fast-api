using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Location.Response
{
    public class GetBestSellerLocationResponse
    {
        public Guid Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int OrderCount { get; set; }
    }
}
