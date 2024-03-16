using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.School.Response
{
    public class GetSchoolIncludeAreaAndLocationResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }

        public AreaOfGetSchoolResponse Area { get; set; }

        public ICollection<LocationOfGetSchoolResponse> Locations { get; set; }
        public class AreaOfGetSchoolResponse
        {
            public string Code { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Ward { get; set; }
        }
        public class LocationOfGetSchoolResponse
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
        }
    }
}
