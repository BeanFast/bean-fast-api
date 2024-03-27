using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.LoyaltyCard.Request
{
    public class CreateLoyaltyCardRequest
    {
        [RequiredGuid]
        public Guid ProfileId { get; set; }
        [RequiredGuid]
        public Guid CardTypeId { get; set; }
        public string Title { get; set; }
        [RequiredFileExtensions(Utilities.Enums.AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
    }
}
