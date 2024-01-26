namespace DataTransferObjects.Core.Response
{
    public class BaseApiResponse
    {
        public DateTime Time { get; set; } = DateTime.UtcNow;

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
