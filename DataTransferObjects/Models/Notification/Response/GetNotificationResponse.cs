using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Notification.Response
{
    public class GetNotificationResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        //public string? Type { get; set; }
        public string? Link { get; set; }
        public Guid UserId { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public int Status { get; set; }
    }
}
