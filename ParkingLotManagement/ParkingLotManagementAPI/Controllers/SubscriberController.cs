using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Entities;
using ParkingLotManagementAPI.Models;
using ParkingLotManagementAPI.Services;

namespace ParkingLotManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberRepository subscriberRepository;
        private readonly ISubscriptionRepository subscriptionRepository;

        public SubscriberController(ISubscriberRepository subscriberRepository,ISubscriptionRepository subscriptionRepository)
        {
            this.subscriberRepository = subscriberRepository;
            this.subscriptionRepository = subscriptionRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriberForCreationDTO>>> GetSubcribers(string? searchQuery)
        {
            var subscribers = await subscriberRepository.GetSubcribersAsync(searchQuery);
            var subscribersDto = new List<SubscriberForCreationDTO>();
            if (subscribers == null)
            {
                return NotFound();
            }

            foreach (var subscriber in subscribers)
            {
                subscribersDto.Add(new SubscriberForCreationDTO
                {
                    Id = subscriber.Id,
                    FirstName = subscriber.FirstName,
                    LastName = subscriber.LastName,
                    PhoneNumber = subscriber.PhoneNumber,
                    Email = subscriber.Email,
                    IdCardNumber = subscriber.IdCardNumber,
                    PlateNumber = subscriber.PlateNumber,
                    Birthday = subscriber.Birthday,
                    IsDeleted=subscriber.IsDeleted,
                });
            }

            return Ok(subscribersDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriberForCreationDTO>> GetSubcriber(int id)
        {
            var subscriber= await subscriberRepository.GetSubcriberAsync(id);
            if (subscriber == null)
            {
                return NotFound();
            }
            var subscriberDTO = new SubscriberForCreationDTO
            {
                Id = subscriber.Id,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                Birthday = subscriber.Birthday,
                IdCardNumber = subscriber.IdCardNumber,
                PhoneNumber = subscriber.PhoneNumber,
                Email = subscriber.Email,
                PlateNumber = subscriber.PlateNumber,
                IsDeleted=subscriber.IsDeleted,
            };

            return Ok(subscriberDTO);
        }
        [HttpPost]
        public async Task<ActionResult<SubscriberForViewDTO>> CreateSubscriber([FromBody] SubscriberDTO subscriberDTO)
        {
            
            var subscriberDetails = subscriberDTO;
            var subscriptionDetails = subscriberDTO.subscriptionForCreationDTO;

           
            var subscriber = new Subscriber
            {
              
               FirstName=subscriberDetails.FirstName,
               LastName=subscriberDetails.LastName,
                Email = subscriberDetails.Email,
                IdCardNumber= subscriberDetails.IdCardNumber,
                PhoneNumber=subscriberDetails.PhoneNumber,
                Birthday=subscriberDetails.Birthday,
                PlateNumber=subscriberDetails.PlateNumber,
                IsDeleted=false,
                
            };

           
            var subscription = new Subscription
            {
                
              
                StartDate = subscriptionDetails.StartDate,
                EndDate = subscriptionDetails.EndDate,
               DiscountValue=subscriptionDetails.DiscountValue,
               
                Code = Guid.NewGuid().ToString("N").Substring(0,6).ToUpper(),
                IsDeleted = false,
              
            };

            subscription.Price = subscriptionRepository.
                CalculatePrice(subscription.StartDate, subscription.EndDate) - subscription.DiscountValue;
       
            subscriber.Subscription = subscription;

           

            if (await subscriberRepository.IdCarNumberExistAsync(subscriber.IdCardNumber))
            {
                return Conflict("A subscriber with the same ID card number already exists.");
            }
            if (await subscriptionRepository.CodeExistAsync(subscriber.Subscription.Code))
            {
                return Conflict("A subscription with the same Code  number already exists.");
            }

            await subscriberRepository.AddSubcriberAsync(subscriber);





            var createdSubscriberDTO = new SubscriberForViewDTO
            {

                Id = subscriber.Id,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                PhoneNumber = subscriber.PhoneNumber,
                Email = subscriber.Email,
                IdCardNumber = subscriber.IdCardNumber,
                PlateNumber = subscriber.PlateNumber,
                Birthday = subscriber.Birthday,
                SubscriptionID = subscriber.Subscription.Id
            };

            
            return Ok(createdSubscriberDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubscriber(int id,
          [FromBody] SubscriberForUpdateDTO updatedSubscriberDTO)
        {
            var subscriber = await subscriberRepository.GetSubcriberAsync(id);
            if(subscriber == null)
            {
                return NotFound();
            }

            subscriber.FirstName= updatedSubscriberDTO.FirstName;
            subscriber.LastName= updatedSubscriberDTO.LastName;
            subscriber.PhoneNumber= updatedSubscriberDTO.PhoneNumber;
            subscriber.Email= updatedSubscriberDTO.Email;
            subscriber.IdCardNumber= updatedSubscriberDTO.IdCardNumber;
            subscriber.Birthday= updatedSubscriberDTO.Birthday;
            subscriber.IsDeleted = updatedSubscriberDTO.IsDeleted;
            subscriber.PlateNumber=updatedSubscriberDTO.PlateNumber;
            await subscriberRepository.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubscriber(int id)
        {
            subscriberRepository.DeleteSubscriber(id);
            return NoContent();
        }
    }
}
