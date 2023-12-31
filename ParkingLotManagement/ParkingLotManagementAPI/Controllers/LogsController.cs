﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotManagementAPI.Entities;
using ParkingLotManagementAPI.Models;
using ParkingLotManagementAPI.Services;

namespace ParkingLotManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository logsRepository;
        private readonly ISubscriptionRepository subscriptionRepository;
        private readonly ISubscriberRepository subscriberRepository;

        public ISubscriptionRepository SubscriptionRepository { get; }

        public LogsController(ILogsRepository logsRepository, ISubscriptionRepository subscriptionRepository,ISubscriberRepository subscriberRepository)
        {
            this.logsRepository = logsRepository;
           this.subscriptionRepository = subscriptionRepository;
            this.subscriberRepository = subscriberRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogsForViewDTO>>> GetLogs(string? searchQuery)
        {
            var logs = await logsRepository.GetLogsAsync(searchQuery);
            var logsDTOList = new List<LogsForViewDTO>();

            foreach (var log in logs)
            {
                var logsDTO = new LogsForViewDTO()
                {
                    Code = log.Code,
                    CheckInTime = log.CheckInTime,
                    CheckOutTime = log.CheckOutTime != DateTime.MinValue ? log.CheckOutTime : (DateTime?)null,
                    Price = log.Price,
                    SubscriptionId = log.SubscriptionId,
                };

                if (log.SubscriptionId == null)
                {
                    logsDTO.Subscription = null;
                }
                else
                {
                    var subscription = await subscriptionRepository.GetSubscriptionAsync(log.SubscriptionId);
                    if (subscription == null)
                    {

                        logsDTO.Subscription = null;
                    }
                    else
                    {
                        var subscriber= await subscriberRepository.GetSubcriberAsync(subscription.SubscriberId);
                        logsDTO.Subscription = new SubscriptionForViewDTO
                        {
                            Id = subscription.Id,
                            Code = subscription.Code,
                            SubscriberId = subscription.SubscriberId,
                            Price = subscription.Price,
                            DiscountValue = subscription.DiscountValue,
                            StartDate = subscription.StartDate,
                            EndDate = subscription.EndDate,
                            Subscriber= new SubscriberForViewDTO
                            {
                                FirstName=subscriber.FirstName,
                                LastName=subscriber.LastName,
                                IdCardNumber=subscriber.IdCardNumber,
                                PhoneNumber=subscriber.PhoneNumber,
                                PlateNumber=subscriber.PhoneNumber,
                                Email=subscriber.Email,
                                Birthday=subscriber.Birthday,
                                Id = subscriber.Id

                            }

                        };
                    }
                }

                logsDTOList.Add(logsDTO);
            }

            return Ok(logsDTOList);
        }
    
        [HttpGet("{day}")]
        public async Task<ActionResult<IEnumerable<LogsForViewDTO>>> GetLogsByDay(DateTime day)
        {

            var logs = await logsRepository.GetLogsByDayAsync(day);
            var logsDTO = new List<LogsForViewDTO>();
            foreach (var log in logs)
            {
                logsDTO.Add(new LogsForViewDTO
                {
                    Code = log.Code,
                    CheckInTime = log.CheckInTime,
                    CheckOutTime = log.CheckOutTime != DateTime.MinValue ? log.CheckOutTime : (DateTime?)null,
                    Price = log.Price,
                    SubscriptionId = log.SubscriptionId,

                }); 
            }
            return Ok(logsDTO);
        }
        [HttpGet("code/{code}")]
        public async Task<ActionResult<LogsForViewDTO>> GetLogByCode(string code)
        {
            var log = await logsRepository.GetLogByCodeAsync(code);
            if (log == null)
            {
                return NotFound();
            }

            var logsDTO = new LogsForViewDTO()
            {
                Code = log.Code,
                CheckInTime = log.CheckInTime,
                CheckOutTime = log.CheckOutTime != DateTime.MinValue ? log.CheckOutTime : (DateTime?)null,
                Price = log.Price,
                SubscriptionId = log.SubscriptionId,
            };

            if (log.SubscriptionId != null)
            {
                var subscription = await subscriptionRepository.GetSubscriptionAsync(log.SubscriptionId);
                if (subscription != null)
                {
                    logsDTO.Subscription = new SubscriptionForViewDTO
                    {
                        Id = subscription.Id,
                        Code = subscription.Code,
                        SubscriberId = subscription.SubscriberId,
                        Price = subscription.Price,
                        DiscountValue = subscription.DiscountValue,
                        StartDate = subscription.StartDate,
                        EndDate = subscription.EndDate,
                    };

                    if (subscription.SubscriberId != null)
                    {
                        var subscriber = await subscriberRepository.GetSubcriberAsync(subscription.SubscriberId);
                        if (subscriber != null)
                        {
                            logsDTO.Subscription.Subscriber = new SubscriberForViewDTO
                            {
                                FirstName = subscriber.FirstName,
                                LastName = subscriber.LastName,
                                IdCardNumber = subscriber.IdCardNumber,
                                PhoneNumber = subscriber.PhoneNumber,
                                PlateNumber = subscriber.PhoneNumber, // Is this correct? It was using PhoneNumber before, verify and change if necessary.
                                Email = subscriber.Email,
                                Birthday = subscriber.Birthday,
                                Id = subscriber.Id,
                            };
                        }
                    }
                }
            }

            return Ok(logsDTO);
        }


        [HttpPost]
        public async Task<ActionResult<LogsForViewDTO>> CheckIn([FromBody] CheckInDTO logsDTO)
        {
            try
            {
                if (logsDTO.SubscriptionId == null)
                {
                    // If SubscriptionId is null, create a log without associating it with any subscription
                    var log = new Logs
                    {
                        Code = Guid.NewGuid().ToString("N").Substring(0, 8),
                        CheckInTime = DateTime.Now,
                    };

                    await logsRepository.AddLogAsync(log);
                   
                    
       
                    var createdLog = new LogsForViewDTO
                    {
                        Code = log.Code,
                        CheckInTime = log.CheckInTime,
                        SubscriptionId = null,
                    };

                    return Ok(createdLog);


                }
                else
                {
                    var log = new Logs
                    {
                        Code = Guid.NewGuid().ToString("N").Substring(0, 8),
                        CheckInTime = DateTime.Now,
                        SubscriptionId = logsDTO.SubscriptionId,
                    };
                    // Check if the subscription is already checked in
                    if (logsRepository.SuscriptionCheckedIn(logsDTO.SubscriptionId))
                    {
                        return Conflict("Subscription already checked in.");
                    }

                    // Get the subscription details
                    var subscription = await subscriptionRepository.GetSubscriptionIncludedDeletedAsync(logsDTO.SubscriptionId.Value);
                    if (subscription == null)
                    {
                        return BadRequest("Subscription does not exist.");
                    }
                    bool activeSubscription = subscription.IsDeleted==false&&subscription.EndDate>= log.CheckInTime;

                    if (!activeSubscription)
                    {
                        return BadRequest("Subscription is not active or has expired.");
                    }

                  

                    await logsRepository.AddLogAsync(log);

                    var createdLog = new LogsForViewDTO
                    {
                        Code = log.Code,
                        CheckInTime = log.CheckInTime,
                        SubscriptionId = log.SubscriptionId,
                    };

                    return Ok(createdLog);
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut("{code}")]
        public async Task<ActionResult<LogsForViewDTO>> CheckOut(string code)
        {
            var log = await logsRepository.FindLogByCode(code);
            if (log == null)
            {
                return NotFound();

            }
            if (log.CheckOutTime != DateTime.MinValue)
            {
                return Conflict("Code is already Checked Out");
            }

            log.CheckOutTime = DateTime.Now;
            log.Price = logsRepository.CalculatePrice(log);
            await logsRepository.SaveChangesAsync();
            return Ok(new LogsForViewDTO
            {
                Code = log.Code,
                SubscriptionId = log.SubscriptionId,
                CheckInTime = log.CheckInTime,
                CheckOutTime = log.CheckOutTime,
                Price = log.Price
            });


        }
    }
}
