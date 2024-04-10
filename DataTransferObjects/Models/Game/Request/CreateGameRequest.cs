using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Game.Request
{
    public class CreateGameRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
