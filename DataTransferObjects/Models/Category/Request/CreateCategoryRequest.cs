﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Category.Request
{
    public class CreateCategoryRequest 
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set;}
    }
}
