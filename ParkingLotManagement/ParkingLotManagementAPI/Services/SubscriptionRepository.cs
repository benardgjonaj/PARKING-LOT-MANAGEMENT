using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Data;
using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationContext context;

        public SubscriptionRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            await context.Subscriptions.AddAsync(subscription);
            await context.SaveChangesAsync();   
        }

        public async Task<bool> CodeExistAsync(string code)
        {
            var existingSubscription=await context.Subscriptions.FirstOrDefaultAsync(s=>s.Code == code);
            if (existingSubscription != null)
            {
                return true;
            }
            return false;
        }

        public void DeleteSubscription(int id)
        {
            var subscription = context.Subscriptions.Find(id);
            if (subscription != null)
            {
                subscription.IsDeleted = true;
                context.SaveChanges();
            }
        }

        public async Task<Subscription> GetSubscriptionAsync(int id)
        {
            return await context.Subscriptions.Where(s=>s.IsDeleted==false)
                .FirstOrDefaultAsync(s=>s.Id == id);
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(string? searchQuery)
        {
            var subscriptions =  context.Subscriptions.Where(s => s.IsDeleted == false)
                .Include(s=>s.Subscriber).AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                subscriptions = subscriptions.Where(s =>
                    s.Subscriber.FirstName.Contains(searchQuery) ||
                    s.Code.Contains(searchQuery));
            }

             return await subscriptions.ToListAsync();    
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
