using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface ISubscriberRepository
    {
        Task AddSubcriberAsync(Subscriber subscriber);
        Task <IEnumerable<Subscriber>> GetSubcribersAsync(string? searchQuery);
        Task<IEnumerable<Subscriber>> GetSubscribersWithNoActiveSubscriptionsAsync();
        Task<Subscriber> GetSubcriberAsync(int id);
       
        bool DeleteSubscriber(int id);
        Task<bool> IdCarNumberExistAsync(string idCardNumber);
        Task<bool> SaveChangesAsync();

    }
}
