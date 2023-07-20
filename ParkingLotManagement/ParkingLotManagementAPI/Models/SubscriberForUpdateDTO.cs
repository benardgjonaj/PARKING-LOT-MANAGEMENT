using System.ComponentModel.DataAnnotations;

namespace ParkingLotManagementAPI.Models
{
    public class SubscriberForUpdateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string IdCardNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string PlateNumber { get; set; }
      
       
    }
}
