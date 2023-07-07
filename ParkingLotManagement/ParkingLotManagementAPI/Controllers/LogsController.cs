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
                    CheckOutTime = log.CheckOutTime,
                    Price = log.Price,
                    SubscriptionId = log.SubscriptionId,

                });
            }
            return Ok(logsDTO);
        }
        [HttpPost]
        public async Task<ActionResult<LogsForViewDTO>> CreateLog([FromBody] LogsDTO logsDTO)
        {
            try
            {
                var logEntity = new Logs
                {
                    Code = logsDTO.Code,
                    CheckInTime = logsDTO.CheckInTime,
                    CheckOutTime = logsDTO.CheckOutTime,
                    SubscriptionId = logsDTO.SubscriptionId,

                };
                var log = new Logs
                {
                    Code = logEntity.Code,
                    CheckInTime = logEntity.CheckInTime,
                    CheckOutTime = logEntity.CheckOutTime,
                    SubscriptionId = logEntity.SubscriptionId,
                    Price = logsRepository.CalculatePrice(logEntity)
                };
                await logsRepository.AddLogAsync(log);
                var createdLog = new LogsForViewDTO
                {
                    Code = log.Code,
                    CheckInTime = log.CheckInTime,
                    CheckOutTime = log.CheckOutTime,
                    SubscriptionId = log.SubscriptionId,
                    Price = log.Price
                };
                return Ok(createdLog);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
    }
}
