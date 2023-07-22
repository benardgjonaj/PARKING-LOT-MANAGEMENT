using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotManagementAPI.Entities;
using ParkingLotManagementAPI.Models;
using ParkingLotManagementAPI.Services;

namespace ParkingLotManagementAPI.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly ISubscriberRepository subscriberRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository, ISubscriberRepository subscriberRepository)
        {
            this.subscriptionRepository = subscriptionRepository;
            this.subscriberRepository = subscriberRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionForViewDTO>>> GetSubscriptions(string? searchQuery)
        {
            var subscriptions = await subscriptionRepository.GetSubscriptionsAsync(searchQuery);
            var subscriptionsDTO = new List<SubscriptionForViewDTO>();

            foreach (var subscription in subscriptions)
            {
                subscriptionsDTO.Add(new SubscriptionForViewDTO
                {
                    Id= subscription.Id,
                    Code = subscription.Code,
                    SubscriberId = subscription.SubscriberId,
                    Price = subscription.Price,
                    DiscountValue = subscription.DiscountValue,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                   
                });
            }

            return Ok(subscriptionsDTO);
        }
        [HttpGet("GetSubscriptionsWithNoActiveLogs")]
        public async Task<ActionResult<IEnumerable<SubscriptionForViewDTO>>> GetSubscriptionsWithNoActiveLogs()
        {
            var subscriptions = await subscriptionRepository.GetSubscriptionsWithNoActiveLogsAsync();
            var subscriptionsDTO = new List<SubscriptionForViewDTO>();

            foreach (var subscription in subscriptions)
            {
                if (subscription.Logs == null || subscription.Logs.All(log => log.CheckOutTime != DateTime.MinValue))
                {
                    subscriptionsDTO.Add(new SubscriptionForViewDTO
                    {
                        Id = subscription.Id,
                        Code = subscription.Code,
                        SubscriberId = subscription.SubscriberId,
                        Price = subscription.Price,
                        DiscountValue = subscription.DiscountValue,
                        StartDate = subscription.StartDate,
                        EndDate = subscription.EndDate,
                    });
                }
            }

            return Ok(subscriptionsDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionForViewDTO>> GetSubscription(int id)
        {
            var subscription = await subscriptionRepository.GetSubscriptionAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }
            var subscriptionDTO = new SubscriptionForViewDTO
            {
                Id= subscription.Id,
                Code = subscription.Code,
                SubscriberId = subscription.SubscriberId,
                Price = subscription.Price,
                DiscountValue = subscription.DiscountValue,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                
            };

            return Ok(subscriptionDTO);
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionForViewDTO>> CreateSubscription([FromBody] SubscriptionDTO subscriptionDTO)
        {
            var subscriptionDetails = subscriptionDTO;
            var subscriber = await subscriberRepository.GetSubcriberAsync(subscriptionDetails.SubscriberId);
            if (subscriber == null)
            {
                return BadRequest("You are trying to add subscription to a not existing subscriber");
            }
            var subscriptions =await subscriptionRepository.GetSubscriptionsBySubscriberIdAsync(subscriber.Id);
            
            bool hasActiveSubscription = subscriptions.Any(sub =>sub.IsDeleted==false && sub.EndDate >= subscriptionDetails.StartDate);

            if (hasActiveSubscription)
            {
               
                return BadRequest("Cannot add a new subscription while there is an active subscription.");
            }

            var subscription = new Subscription
            {
                Code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(),
                StartDate = subscriptionDetails.StartDate,
                EndDate = subscriptionDetails.EndDate,
                DiscountValue = subscriptionDetails.DiscountValue,
                IsDeleted = false,
                SubscriberId=subscriptionDetails.SubscriberId
                

            };
            subscription.Price = subscriptionRepository.CalculatePrice(subscription.StartDate,subscription.EndDate)*(1-subscription.DiscountValue/100);
           

           
            await subscriptionRepository.AddSubscriptionAsync(subscription);


            var createdSubscriptionDTO = new SubscriptionForViewDTO
            {
                Id= subscription.Id,
                Code = subscription.Code,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                DiscountValue = subscription.DiscountValue,
                Price = subscription.Price,
                SubscriberId=subscription.SubscriberId
                
             

            };
            return Ok(createdSubscriptionDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteSubscription(int id)
        {
            bool isDeleted = subscriptionRepository.DeleteSubscription(id);
            if (isDeleted)
            {

                return Ok(id);
            }
            else
            {

                return NotFound();
            }
        }
    }
}
