using System.ComponentModel.DataAnnotations;

namespace ParkingLotManagementAPI.Models
{
    public class LogsDTO
    {
        [Required]
        public string Code { get; set; }
        public int? SubscriptionId { get; set; }
        [Required]
        public DateTime CheckInTime { get; set; }
        [Required]
        public DateTime CheckOutTime { get; set; }
    }
}
