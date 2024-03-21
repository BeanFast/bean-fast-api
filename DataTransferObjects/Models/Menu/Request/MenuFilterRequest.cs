using System.ComponentModel.DataAnnotations;
using Utilities.Constants;

namespace DataTransferObjects.Models.Menu.Request
{
    public class MenuFilterRequest
    {
        //[RequiredGuid]
        public Guid? KitchenId { get; set; }
        public Guid? SchoolId { get; set; }
        public DateTime? OrderStartTime { get; set; }
        public Guid? CreaterId { get; set; }
        public Guid? UpdaterId { get; set; }
        [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCodeLength)]
        public string? Code { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCreateDateInvalid)]
        public DateTime? CreateDate { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = MessageConstants.MenuMessageContrant.MenuCreateDateInvalid)]
        public DateTime? UpdateDate { get; set; }
        public bool SessionExpired { get; set; }
        public bool SessionOrderable { get; set; }
        public bool SessonIncomming { get; set; }
        public int? Status { get; set; }
    }
}
