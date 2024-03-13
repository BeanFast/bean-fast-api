﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Menu.Request
{
    public class MenuFilterRequest
    {
        [RequiredGuid]
        public Guid KitchenId { get; set; }
        public Guid? CreaterId { get; set; }
        public Guid? UpdaterId { get; set; }
        [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCodeLength)]
        public string? Code { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCreateDateInvalid)]
        public DateTime? CreateDate { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCreateDateInvalid)]
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
    }
}
