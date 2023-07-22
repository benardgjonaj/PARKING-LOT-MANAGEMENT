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

        public decimal CalculatePrice(DateTime start, DateTime end)
        {
            TimeSpan duration = end.Subtract(start);
            int days = duration.Days;
            decimal price = context.PricingPlans.FirstOrDefault(p => p.Type == "weekday").DailyPricing;
            return days * price;
        }

        public async Task<bool> CodeExistAsync(string code)
        {
            var existingSubscription = await context.Subscriptions.FirstOrDefaultAsync(s => s.Code == code);
            if (existingSubscription != null)
            {
                return true;
            }
            return false;
        }

        public bool DeleteSubscription(int id)
        {
            var subscription = context.Subscriptions.Find(id);
            if (subscription != null)
            {
                subscription.IsDeleted = true;
                var logs = context.Logs.Where(l => l.SubscriptionId == id).ToList();
                if (logs != null)
                {
                    foreach (var log in logs)
                    {
                        log.SubscriptionId = null;
                    }
                }

                        context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<Subscription> GetSubscriptionAsync(int id)
        {
            return await context.Subscriptions.Where(s => s.IsDeleted == false)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
      
        public async Task<Subscription> GetSubscriptionIncludedDeletedAsync(int id)
        {
            return await context.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(string? searchQuery)
        {
            var query =  context.Subscriptions.Where(s=>s.IsDeleted==false).AsQueryable();
            var subscribers=new List<Subscriber>(); 
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(sub =>
                    sub.Code== searchQuery
                );
                subscribers = context.Subscribers.Where(s => s.FirstName == searchQuery).
                    Include(s => s.Subscriptions.Where(s=>s.IsDeleted==false)).ToList();
            }
            var listofsubscriptions=query.ToList();
           
            foreach (var subscriber in subscribers)
            {
               foreach (var subscription in subscriber.Subscriptions)
                {
                    listofsubscriptions.Add(subscription);
                }

            }
            return listofsubscriptions;
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsBySubscriberIdAsync(int id)
        {
            var subscriprions=await context.Subscriptions.Where(s=>s.SubscriberId==id).ToListAsync();
            return subscriprions;
        }
        public async Task<IEnumerable<Subscription>> GetSubscriptionsWithNoActiveLogsAsync()
        {
            var subscriptions = context.Subscriptions.Where(s => s.IsDeleted == false&&s.EndDate>DateTime.Now).AsQueryable();
            var filteredSubscriptions = subscriptions
           .Where(sub => sub.Logs == null || sub.Logs.All(log => log.CheckOutTime != DateTime.MinValue));

            return filteredSubscriptions;

        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
