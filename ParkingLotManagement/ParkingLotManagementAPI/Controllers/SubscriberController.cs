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

        public SubscriberController(ISubscriberRepository subscriberRepository)
        {
            this.subscriberRepository = subscriberRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriberDTO>>> GetSubcribers(string? searchQuery)
        {
            var subscribers = await subscriberRepository.GetSubcribersAsync(searchQuery);
            var subscribersDto = new List<SubscriberDTO>();
            if (subscribers == null)
            {
                return NotFound();
            }

            foreach (var subscriber in subscribers)
            {
                subscribersDto.Add(new SubscriberDTO
                {
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
        public async Task<ActionResult<SubscriberDTO>> GetSubcriber(int id)
        {
            var subscriber= await subscriberRepository.GetSubcriberAsync(id);
            if (subscriber == null)
            {
                return NotFound();
            }
            var subscriberDTO = new SubscriberDTO
            {
                FirstName = subscriber.FirstName,
                LastName = subscriber.LastName,
                Birthday = subscriber.Birthday,
                IdCardNumber = subscriber.IdCardNumber,
                PhoneNumber = subscriber.PhoneNumber,
                Email = subscriber.Email,
                PlateNumber = subscriber.PlateNumber,
            };

            return Ok(subscriberDTO);
        }
        [HttpPost]
        public async Task<ActionResult<SubscriberDTO>> CreateSubscriber([FromBody] SubscriberDTO subscriberDTO)
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
                IsDeleted=subscriberDetails.IsDeleted,
                
            };

           
            var subscription = new Subscription
            {
                
              
                StartDate = subscriptionDetails.StartDate,
                EndDate = subscriptionDetails.EndDate,
               DiscountValue=subscriptionDetails.DiscountValue,
               Price=subscriptionDetails.Price,
               Code=subscriptionDetails.Code,
               IsDeleted=subscriptionDetails.IsDeleted,
              
            };
           

       
            subscriber.Subscription = subscription;

           

            if (await subscriberRepository.IdCarNumberExistAsync(subscriber.IdCardNumber))
            {
                return Conflict("A subscriber with the same ID card number already exists.");
            }
          
            await subscriberRepository.AddSubcriberAsync(subscriber);

          
          

           
            var createdSubscriberDTO = new SubscriberDTO
            {
            
               
                FirstName = subscriber.FirstName,
                LastName= subscriber.LastName,
                PhoneNumber= subscriber.PhoneNumber,
                Email = subscriber.Email,
                IdCardNumber=subscriber.IdCardNumber,
                PlateNumber=subscriber.PlateNumber,
                IsDeleted=subscriber.IsDeleted,
                Birthday= subscriber.Birthday,
                subscriptionForCreationDTO = new SubscriptionForCreationDTO
                {
                
                    StartDate = subscription.StartDate,
                    EndDate = subscription.EndDate,
                    DiscountValue= subscription.DiscountValue,
                    Price=subscription.Price,
                    Code= subscription.Code,
                    IsDeleted=subscription.IsDeleted
                }
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
