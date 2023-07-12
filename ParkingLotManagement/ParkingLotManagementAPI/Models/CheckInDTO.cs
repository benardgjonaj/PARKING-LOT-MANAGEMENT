using System.ComponentModel.DataAnnotations;

namespace ParkingLotManagementAPI.Models
{
    public class CheckInDTO
    {
        [Required]
        public string Code { get; set; }
        public int? SubscriptionId { get; set; }
        [Required]
        public DateTime CheckInTime { get; set; }
    }
}
