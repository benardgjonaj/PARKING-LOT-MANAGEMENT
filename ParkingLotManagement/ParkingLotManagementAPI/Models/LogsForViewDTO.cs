namespace ParkingLotManagementAPI.Models
{
    public class LogsForViewDTO
    {
     
        public string Code { get; set; }
        public int? SubscriptionId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal Price { get; set; }
        public SubscriptionForLogViewDTO Subscription { get; set; }
    }
}
