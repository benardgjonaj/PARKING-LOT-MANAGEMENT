using System.ComponentModel.DataAnnotations;

namespace ParkingLotManagementAPI.Models
{
    public class SubscriptionForViewDTO
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public int SubscriberId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal DiscountValue { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
