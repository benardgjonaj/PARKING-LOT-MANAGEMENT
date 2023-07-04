using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface IPricingPlanRepository
    {
        Task<IEnumerable<PricingPlan>> GetPricingPlansAsync();
        Task<PricingPlan> GetPricingPlanAsync(int id);
        Task<bool> SaveChangesAsync();

    }
    
}
