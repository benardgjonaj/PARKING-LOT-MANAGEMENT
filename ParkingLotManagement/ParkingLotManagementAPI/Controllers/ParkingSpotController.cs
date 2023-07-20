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
        public async Task<ActionResult<ParkingSpotViewDTO>> GetParkingSpots()
        {
            var parkingSpotInfo = await parkingSpotRepository.GetParkingSpotInfo();
            if (parkingSpotInfo == null)
            {
                return NotFound();
            }

            return Ok(parkingSpotInfo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateParkingSpot(int id, [FromBody] ParkingSpotDTO parkingSpotDTO)
        {
            var parkingSpot = await parkingSpotRepository.GetParkingSpot(id);
            if (parkingSpot == null)
            {
                return NotFound();
            }
            parkingSpot.TotalSpots = parkingSpotDTO.TotalSpots;
            await parkingSpotRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
