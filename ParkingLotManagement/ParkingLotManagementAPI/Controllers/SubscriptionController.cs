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

        public SubscriptionController(ISubscriptionRepository subscriptionRepository,ISubscriberRepository subscriberRepository)
        {
            this.subscriptionRepository = subscriptionRepository;
            this.subscriberRepository = subscriberRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionForViewDTO>>> GetSubscriptions(string? searchQuery)
        {
            var subscriptions= await subscriptionRepository.GetSubscriptionsAsync(searchQuery);
            var subscriptionsDTO=new List<SubscriptionForViewDTO>();

            foreach (var subscription in subscriptions)
            {
                subscriptionsDTO.Add(new SubscriptionForViewDTO
                {
                    Code = subscription.Code,
                    SubscriberId = subscription.SubscriberId,
                    Price = subscription.Price,
                    DiscountValue = subscription.DiscountValue,
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    IsDeleted = subscription.IsDeleted,
                });
            }

            return Ok(subscriptionsDTO);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionForViewDTO>> GetSubscription(int id)
        {
            var subscription = await subscriptionRepository.GetSubscriptionAsync(id);

            if(subscription == null)
            {
                return NotFound();
            }
            var subscriptionDTO = new SubscriptionForViewDTO
            {
                Code = subscription.Code,
                SubscriberId = subscription.SubscriberId,
                Price = subscription.Price,
                DiscountValue = subscription.DiscountValue,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                IsDeleted = subscription.IsDeleted,
            };

            return Ok(subscriptionDTO);
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionDTO>> CreateSubscription([FromBody] SubscriptionDTO subscriptionDTO)
        {
            var subscriptionDetails = subscriptionDTO;
            var subscriberDetails = subscriptionDTO.subscriberForCreationDTO;

            var subscription = new Subscription
            {

                StartDate = subscriptionDetails.StartDate,
                EndDate = subscriptionDetails.EndDate,
                DiscountValue = subscriptionDetails.DiscountValue,
                Price = subscriptionDetails.Price,
                Code = subscriptionDetails.Code,
                IsDeleted = subscriptionDetails.IsDeleted,

            };

            var subscriber = new Subscriber
            {

                FirstName = subscriberDetails.FirstName,
                LastName = subscriberDetails.LastName,
                Email = subscriberDetails.Email,
                IdCardNumber = subscriberDetails.IdCardNumber,
                PhoneNumber = subscriberDetails.PhoneNumber,
                Birthday = subscriberDetails.Birthday,
                PlateNumber = subscriberDetails.PlateNumber,
                IsDeleted = subscriberDetails.IsDeleted,

            };

            subscription.Subscriber= subscriber;
            if(await subscriptionRepository.CodeExistAsync(subscription.Code))
            {
                return Conflict("A subscription with the same Code  number already exists.");
            }
            if (await subscriberRepository.IdCarNumberExistAsync(subscription.Subscriber.IdCardNumber))
            {
                return Conflict("A subscriber with the same ID card number already exists.");
            }
            await subscriptionRepository.AddSubscriptionAsync(subscription);


            var createdSubscriptionDTO = new SubscriptionDTO
            {
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,
                DiscountValue = subscription.DiscountValue,
                Price = subscription.Price,
                Code = subscription.Code,
                IsDeleted = subscription.IsDeleted,
                subscriberForCreationDTO = new SubscriberForCreationDTO
                {
                    FirstName = subscriber.FirstName,
                    LastName = subscriber.LastName,
                    PhoneNumber = subscriber.PhoneNumber,
                    Email = subscriber.Email,
                    IdCardNumber = subscriber.IdCardNumber,
                    PlateNumber = subscriber.PlateNumber,
                    IsDeleted = subscriber.IsDeleted,
                    Birthday = subscriber.Birthday,
                }

            };
            return Ok(createdSubscriptionDTO);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubscription(int id)
        {
            subscriptionRepository.DeleteSubscription(id);
            return NoContent();
        }
    }
}
