namespace ParkingLotManagementAPI.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SubscriberId { get; set; }
        public decimal Price { get; set; }
      
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<Logs> Logs { get; set; }
    }
}
