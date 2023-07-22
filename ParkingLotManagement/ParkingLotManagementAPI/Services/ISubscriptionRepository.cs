using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface ISubscriptionRepository
    {
        Task AddSubscriptionAsync(Subscription subscription);
        Task<IEnumerable<Subscription>> GetSubscriptionsAsync(string? searchQuery);
        Task<Subscription> GetSubscriptionAsync(int id);
        Task<IEnumerable<Subscription>> GetSubscriptionsBySubscriberIdAsync(int id);
        Task<IEnumerable<Subscription>> GetSubscriptionsWithNoActiveLogsAsync();
        bool DeleteSubscription(int id);
        Task<bool> CodeExistAsync(string code);
        public decimal CalculatePrice(DateTime start, DateTime end);
        Task<bool> SaveChangesAsync();
        Task<Subscription> GetSubscriptionIncludedDeletedAsync(int id);
    }
}
