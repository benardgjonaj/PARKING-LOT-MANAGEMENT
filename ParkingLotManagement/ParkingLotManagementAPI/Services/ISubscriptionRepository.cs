using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface ISubscriptionRepository
    {
        Task AddSubscriptionAsync(Subscription subscription);
        Task<IEnumerable<Subscription>> GetSubscriptionsAsync(string? searchQuery);
        Task<Subscription> GetSubscriptionAsync(int id);

        void DeleteSubscription(int id);
        Task<bool> CodeExistAsync(string code);
        public decimal CalculatePrice(DateTime start, DateTime end);
        Task<bool> SaveChangesAsync();
    }
}
