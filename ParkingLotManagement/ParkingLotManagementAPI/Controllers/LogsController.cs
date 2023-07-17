using Microsoft.AspNetCore.Http;
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

        public LogsController(ILogsRepository logsRepository)
        {
            this.logsRepository = logsRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogsForViewDTO>>> GetLogs(string? searchQuery)
        {
            var logs = await logsRepository.GetLogsAsync(searchQuery);
            var logsDTO=new List<LogsForViewDTO>();
            foreach (var log in logs)
            {
                logsDTO.Add(new LogsForViewDTO
                {
                    Code = log.Code,
                    CheckInTime = log.CheckInTime,
                    CheckOutTime = log.CheckOutTime,
                    //CheckOutTime = log.CheckOutTime != DateTime.MinValue ? log.CheckOutTime : (DateTime?)null,
                    Price = log.Price,
                    SubscriptionId = log.SubscriptionId,

                });
            }
            return Ok(logsDTO);
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
        //[HttpGet("{date}")]
        //public async Task<ActionResult<LogsForViewDTO>> GetLogByDate(DateTime date)
        //{

        //    var log = await logsRepository.GetLogByDateAsync(date);
        //    var logsDTO = new LogsForViewDTO()
        //    {
        //        Code = log.Code,
        //        CheckInTime = log.CheckInTime,
        //        CheckOutTime = log.CheckOutTime != DateTime.MinValue ? log.CheckOutTime : (DateTime?)null,
        //        Price = log.Price,
        //        SubscriptionId = log.SubscriptionId,
        //    };
           
            
              
            
        //    return Ok(logsDTO);
        //}

        [HttpPost]
        public async Task<ActionResult<LogsForViewDTO>> CheckIn([FromBody] CheckInDTO logsDTO)
        {
            try
            {

                var log = new Logs
                {
                    Code = Guid.NewGuid().ToString("N").Substring(0, 8),
                CheckInTime = DateTime.Now,
                    SubscriptionId = logsDTO.SubscriptionId,

                };
                if( logsRepository.ExistingCode(log.Code))
                {
                    return Conflict("This code is already checked in");
                }
                if (logsRepository.SuscriptionCheckedIn(log.SubscriptionId))
                {
                    return Conflict("Subscription already checked In ");
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
            if(log.CheckOutTime != DateTime.MinValue)
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
