using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Data;
using ParkingLotManagementAPI.Entities;
using ParkingLotManagementAPI.Models;

namespace ParkingLotManagementAPI.Services
{
    public class ParkingSpotRepository : IParkingSpotRepository
    {
        private readonly ApplicationContext context;

        public ParkingSpotRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<ParkingSpot> GetParkingSpots()
        {
           return await context.ParkingSpots.FirstOrDefaultAsync(p=>p.Id==1);
            
           

        }
    }
}
