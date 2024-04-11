using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Gift.Response
{
    public class GetGiftResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public int InStock { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }

    }
}
