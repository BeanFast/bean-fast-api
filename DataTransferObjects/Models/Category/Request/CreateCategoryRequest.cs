﻿using System;
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
        public string ImagePath { get; set;}
    }
}
