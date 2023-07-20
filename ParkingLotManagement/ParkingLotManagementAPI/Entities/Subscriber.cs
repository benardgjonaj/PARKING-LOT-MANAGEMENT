namespace ParkingLotManagementAPI.Entities
{
    public class Subscriber
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
        public List<Subscription> Subscriptions { get; set; }=new List<Subscription>();
    }
}
