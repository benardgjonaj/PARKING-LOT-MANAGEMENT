using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Data;
using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public class PricingPlanRepository:IPricingPlanRepository
    {
        private readonly ApplicationContext context;

        public PricingPlanRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<PricingPlan?> GetPricingPlanAsync(string type)
        {
           return await context.PricingPlans.Where(p=>p.Type == type).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PricingPlan>> GetPricingPlansAsync()
        {
            return await context.PricingPlans.ToListAsync();

        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
