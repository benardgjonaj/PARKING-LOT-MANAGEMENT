namespace ParkingLotManagementAPI.Models
{
    public class PricingPlanForUpdateDTO
    {
        public decimal HourlyPricing { get; set; }
        public decimal DailyPricing { get; set; }
        public int MinimumHours { get; set; }
    }
}
