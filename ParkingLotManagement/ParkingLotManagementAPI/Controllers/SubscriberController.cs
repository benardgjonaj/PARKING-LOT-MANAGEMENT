﻿using Microsoft.AspNetCore.Http;
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

        public SubscriberController(ISubscriberRepository subscriberRepository, ISubscriptionRepository subscriptionRepository)
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
                    IsDeleted = subscriber.IsDeleted,
                });
            }

            return Ok(subscribersDto);
        }
        [HttpGet("GetSubscribersWithNoActiveSubscriptions")]
        public async Task<ActionResult<IEnumerable<SubscriberForCreationDTO>>> GetSubscribersWithNoActiveSubscriptionsAsync()
        {
            var subscribers = await subscriberRepository.GetSubscribersWithNoActiveSubscriptionsAsync();
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
                    IsDeleted = subscriber.IsDeleted,
                });
            }

            return Ok(subscribersDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriberForCreationDTO>> GetSubcriber(int id)
        {
            var subscriber = await subscriberRepository.GetSubcriberAsync(id);
            if (subscriber == null)
            {
                return NotFound();
            }
            var activeSubscription=subscriber.Subscriptions.FirstOrDefault(s=>s.IsDeleted==false&&s.EndDate>DateTime.Now);
            var subscriberDTO = new SubscriberWithSubscriptionDTO
            {
                Id = subscriber.Id,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                Birthday = subscriber.Birthday,
                IdCardNumber = subscriber.IdCardNumber,
                PhoneNumber = subscriber.PhoneNumber,
                Email = subscriber.Email,
                PlateNumber = subscriber.PlateNumber,
                IsDeleted = subscriber.IsDeleted,
                Subscription= activeSubscription==null? null : new SubscriptionForLogViewDTO
                {
                    Id=activeSubscription.Id,
                    Code=activeSubscription.Code,
                    Price=activeSubscription.Price,
                    DiscountValue=activeSubscription.DiscountValue,
                    StartDate=activeSubscription.StartDate,
                    EndDate=activeSubscription.EndDate,
                    SubscriberId=activeSubscription.SubscriberId,
                    

                }
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

                FirstName = subscriberDetails.FirstName,
                LastName = subscriberDetails.LastName,
                Email = subscriberDetails.Email,
                IdCardNumber = subscriberDetails.IdCardNumber,
                PhoneNumber = subscriberDetails.PhoneNumber,
                Birthday = subscriberDetails.Birthday,
                PlateNumber = subscriberDetails.PlateNumber,
                IsDeleted = false,

            };


            var subscription = new Subscription
            {


                StartDate = subscriptionDetails.StartDate,
                EndDate = subscriptionDetails.EndDate,
                DiscountValue = subscriptionDetails.DiscountValue,

                Code = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper(),
                IsDeleted = false,

            };

            subscription.Price = subscriptionRepository.
                CalculatePrice(subscription.StartDate, subscription.EndDate) - subscription.DiscountValue;

            subscriber.Subscriptions.Add(subscription);



            if (await subscriberRepository.IdCarNumberExistAsync(subscriber.IdCardNumber))
            {
                return Conflict("A subscriber with the same ID card number already exists.");
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
                
            };


            return Ok(createdSubscriberDTO);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<SubscriberForViewDTO>> UpdateSubscriber(int id,
          [FromBody] SubscriberForUpdateDTO updatedSubscriberDTO)
        {
            var subscriber = await subscriberRepository.GetSubcriberAsync(id);
            if (subscriber == null)
            {
                return NotFound();
            }

            subscriber.FirstName = updatedSubscriberDTO.FirstName;
            subscriber.LastName = updatedSubscriberDTO.LastName;
            subscriber.PhoneNumber = updatedSubscriberDTO.PhoneNumber;
            subscriber.Email = updatedSubscriberDTO.Email;
            subscriber.IdCardNumber = updatedSubscriberDTO.IdCardNumber;
            subscriber.Birthday = updatedSubscriberDTO.Birthday;
            subscriber.PlateNumber = updatedSubscriberDTO.PlateNumber;
            await subscriberRepository.SaveChangesAsync();

            var updatedSubscriber = new SubscriberForViewDTO
            {
                Id = subscriber.Id,
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                PhoneNumber = subscriber.PhoneNumber,
                Email = subscriber.Email,
                IdCardNumber = subscriber.IdCardNumber,
                Birthday = subscriber.Birthday,
                PlateNumber = subscriber.PlateNumber,

            };
            return Ok(updatedSubscriber);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteSubscriber(int id)
        {
            bool isDeleted = subscriberRepository.DeleteSubscriber(id);

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
