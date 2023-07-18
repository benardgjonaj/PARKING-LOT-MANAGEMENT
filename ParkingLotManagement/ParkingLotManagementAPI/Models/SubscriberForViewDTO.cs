using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Models
{
    public class SubscriberForViewDTO
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCardNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string PlateNumber { get; set; }
        public int SubscriptionID { get; set; }
    }
}
