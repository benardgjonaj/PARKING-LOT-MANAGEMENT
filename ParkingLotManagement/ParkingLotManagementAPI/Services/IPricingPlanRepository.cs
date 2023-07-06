using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface IPricingPlanRepository
    {
        Task<IEnumerable<PricingPlan>> GetPricingPlansAsync();
        Task<PricingPlan> GetPricingPlanAsync(string type);
        Task<bool> SaveChangesAsync();

    }
    
}
