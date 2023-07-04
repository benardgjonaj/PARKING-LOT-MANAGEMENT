namespace ParkingLotManagementAPI.Entities
{
    public class Logs
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? SubscriptionId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public decimal Price { get; set; }
        
    }
}
