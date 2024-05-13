using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Kitchen.Request
{
    public class CreateKitchenRequest
    {
        [RequiredGuid]
        public Guid AreaId { get; set; }
        
        [Required(ErrorMessage = MessageConstants.KitchenMessageConstrant.KitchenNameRequired)]
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.KitchenMessageConstrant.KitchenNameLength)]
        public string Name { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = MessageConstants.KitchenMessageConstrant.KitchenAddressRequired)]
        [StringLength(500, MinimumLength = 10, ErrorMessage = MessageConstants.KitchenMessageConstrant.KitchenAddressLength)]
        public string Address { get; set; }
        [RequiredGuid]
        public Guid ManagerId { get; set; }
    }
}
