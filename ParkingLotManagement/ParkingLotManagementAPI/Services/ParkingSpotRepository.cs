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

        public async Task<ParkingSpot> GetParkingSpot(int id)
        {
           return await context.ParkingSpots.FirstOrDefaultAsync(p=>p.Id==id);
            
        }

        public async Task<ParkingSpotViewDTO> GetParkingSpotInfo()
        {
            var parkingSpotInfo = new ParkingSpotViewDTO();
            parkingSpotInfo.TotalSpots = await context.ParkingSpots.SumAsync(ps => ps.TotalSpots);
            parkingSpotInfo.ReservedSpots = await context.Subscriptions.CountAsync(s => s.IsDeleted == false);
            parkingSpotInfo.RegularSpots = parkingSpotInfo.TotalSpots - parkingSpotInfo.ReservedSpots;

            return parkingSpotInfo;


        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
