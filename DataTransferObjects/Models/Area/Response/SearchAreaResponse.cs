using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Area.Response
{
    public class SearchAreaResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public int Status { get; set; }

        public ICollection<SchoolOfSearchAreaResponse> PrimarySchools { get; set; }

        public class SchoolOfSearchAreaResponse
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string ImagePath { get; set; }
        }
    }
}
