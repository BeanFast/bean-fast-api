﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Core
{
    public class SuccessApiResponse : BaseApiResponse
    {
        public object Data { get; set; }
    }
}
