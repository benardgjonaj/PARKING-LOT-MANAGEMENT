namespace ParkingLotManagementAPI.Models
{
    public class SubscriberWithSubscriptionDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCardNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string PlateNumber { get; set; }
        public bool IsDeleted { get; set; }
        public SubscriptionForLogViewDTO Subscription { get; set; }
    }
}
