using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Food.Response
{
    public class GetBestSellerFoodsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SoldCount { get; set; } = 0;
    }
}
