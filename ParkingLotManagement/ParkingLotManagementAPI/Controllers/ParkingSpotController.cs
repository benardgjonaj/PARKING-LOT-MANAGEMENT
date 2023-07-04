using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotManagementAPI.Models;
using ParkingLotManagementAPI.Services;

namespace ParkingLotManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpotController : ControllerBase
    {
        private readonly IParkingSpotRepository parkingSpotRepository;

        public ParkingSpotController(IParkingSpotRepository parkingSpotRepository)
        {
            this.parkingSpotRepository = parkingSpotRepository;
        }
        [HttpGet]
        public async  Task<ActionResult<ParkingSpotDTO>> GetParkingSpots()
        {
           var parkingspot= await parkingSpotRepository.GetParkingSpots();
            if(parkingspot == null)
            {
                return NotFound();
            }
            var parkingspotDTO = new ParkingSpotDTO()
            {
                TotalSpots = parkingspot.TotalSpots,
                FreeSpots = parkingspot.FreeSpots,
                ReservedSpots = parkingspot.ReservedSpots,
            };
            return Ok(parkingspotDTO);
        }
       
        
    }
}
