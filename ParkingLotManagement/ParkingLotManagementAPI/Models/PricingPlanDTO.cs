namespace ParkingLotManagementAPI.Models
{
    public class PricingPlanDTO
    {
     
        public decimal HourlyPricing { get; set; }
        public decimal DailyPricing { get; set; }
        public int MinimumHours { get; set; }
        public string Type { get; set; }
    }
}
