using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.CardType.Request
{
    public class GetCardTypeResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string BackgroundImagePath { get; set; }
    }
}
