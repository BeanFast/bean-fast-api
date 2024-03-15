﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.School.Request
{
    public class CreateSchoolRequest
    {
        [RequiredGuid]
        public Guid AreaId { get; set; }
        [Required(ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolNameRequired)]
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolNameLength)]
        public string Name { get; set; }
        [Required(ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolAddressRequired)]
        [StringLength(500, ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolAddressLength)]
        public string Address { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }

        public ICollection<LocationOfCreateSchoolRequest> Locations { get; set; }

        public class LocationOfCreateSchoolRequest
        {
            [Required(ErrorMessage = MessageConstants.LocationMessageConstant.LocationNameRequired)]
            [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.LocationMessageConstant.LocationNameLength)]
            public string Name { get; set; }
            [Required(ErrorMessage = MessageConstants.LocationMessageConstant.LocationDescriptionRequired)]
            public string Description { get; set; }
            [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
            public IFormFile Image { get; set; }
        }
    }
}
