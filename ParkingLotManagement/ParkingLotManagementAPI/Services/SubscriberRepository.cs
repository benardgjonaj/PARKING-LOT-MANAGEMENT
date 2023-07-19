using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Data;
using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly ApplicationContext context;
       
        public SubscriberRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Subscriber>> GetSubcribersAsync(string? searchQuery)
        {
            var subscribers = context.Subscribers.Where(s=>s.IsDeleted==false).AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                subscribers = subscribers.Where(s =>
                    s.FirstName==searchQuery ||
                    s.LastName == searchQuery ||
                    s.IdCardNumber == searchQuery ||
                    s.Email == searchQuery);
            }

            return await subscribers.ToListAsync();
        }
        public async Task<Subscriber> GetSubcriberAsync(int id)
        {
           var subscriber =await context.Subscribers.FirstOrDefaultAsync(s => s.Id == id&&s.IsDeleted==false);
            return subscriber;
        }

        public async Task AddSubcriberAsync(Subscriber subscriber)
        {
           
            context.Subscribers.Add(subscriber);
            context.SaveChanges();
        }

        public void DeleteSubscriber(int id)
        {
            var subscriber = context.Subscribers.Find(id);
            if (subscriber != null)
            {
                subscriber.IsDeleted = true;
               var  subscription= context.Subscriptions.Find(subscriber.Id);
                if (subscription != null)
                {
                    subscription.IsDeleted = true;
                }
                context.SaveChanges();
            }
        }

        public  async Task<bool>  IdCarNumberExistAsync(string idCardNumber)
        {
            var existingSubscriber = await context.Subscribers.FirstOrDefaultAsync(
                s => s.IdCardNumber == idCardNumber);
            if(existingSubscriber != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
