using ParkingLotManagementAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotManagementAPI.Models
{
    public class SubscriptionDTO
    {
       
        [Required]
        public decimal DiscountValue { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int SubscriberId { get; set; }
     
        
      
       
    }
}
