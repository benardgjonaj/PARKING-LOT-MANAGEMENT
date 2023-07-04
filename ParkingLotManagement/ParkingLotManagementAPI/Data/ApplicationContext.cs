using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Data
{
    public class ApplicationContext:DbContext
    {
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<PricingPlan> PricingPlans { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Logs> Logs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            
        }

    }
}
