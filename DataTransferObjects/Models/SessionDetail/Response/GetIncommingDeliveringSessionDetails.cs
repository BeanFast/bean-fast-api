﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.SessionDetail.Response
{
    public class GetIncommingDeliveringSessionDetails
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public LocationOfSessionDetail? Location { get; set; }
        public ICollection<OrderOfSessionDetail>? Orders { get; set; }

        public class LocationOfSessionDetail
        {
            public Guid Id { get; set; }
            public string? Code { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? ImagePath { get; set; }
            public SchoolOfLocation? School { get; set; }
            public class SchoolOfLocation
            {
                public string Code { get; set; }
                public string Name { get; set; }
                public string Address { get; set; }
                public string ImagePath { get; set; }
                public AreaOfSchool? Area { get; set; }

                public class AreaOfSchool
                {
                    public string Code { get; set; }
                    public string City { get; set; }
                    public string District { get; set; }
                    public string Ward { get; set; }
                }
            }
        }
        
        public class OrderOfSessionDetail
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public double TotalPrice { get; set; }
            public DateTime PaymentDate { get; set; }
            public ICollection<OrderDetailOfOrder>? OrderDetails { get; set; }

            public class OrderDetailOfOrder
            {
                public Guid FoodId { get; set; }
                public int Quantity { get; set; }
                public double Price { get; set; }
                public string? Note { get; set; }
            }
        }
    }
}
