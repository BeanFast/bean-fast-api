namespace Utilities.Statuses
{
    public class OrderStatus : BaseEntityStatus
    {
        public static readonly int Pending = 2;
        public static readonly int Cooking = 3;
        public static readonly int Delivering = 4;
        public static readonly int Completed = 5;
        public static readonly int CancelledByCustomer = 6;
        public static readonly int Cancelled = 7;
    }
}
