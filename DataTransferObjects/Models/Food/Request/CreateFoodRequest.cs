using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects.Models.Food.Request
{
    public class CreateFoodRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsCombo { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
