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
            return await context.ParkingSpots.FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<ParkingSpotViewDTO> GetParkingSpotInfo()
        {
            var parkingSpotInfo = new ParkingSpotViewDTO();
            parkingSpotInfo.TotalSpots = await context.ParkingSpots.SumAsync(ps => ps.TotalSpots);
            parkingSpotInfo.ReservedSpots = await context.Subscriptions.CountAsync(s => s.IsDeleted == false);

            parkingSpotInfo.OccupiedReservedSpots = await context.Logs.CountAsync(s => s.Subscription != null && s.CheckOutTime == DateTime.MinValue);
            parkingSpotInfo.FreeReservedSpots = parkingSpotInfo.ReservedSpots - parkingSpotInfo.OccupiedReservedSpots;

            parkingSpotInfo.RegularSpots = parkingSpotInfo.TotalSpots - parkingSpotInfo.ReservedSpots;
            parkingSpotInfo.OccupiedRegularSpots = await context.Logs.CountAsync(l => l.SubscriptionId == null && l.CheckOutTime == DateTime.MinValue);

            parkingSpotInfo.FreeRegularSpots = parkingSpotInfo.RegularSpots - parkingSpotInfo.OccupiedRegularSpots;

            return parkingSpotInfo;


        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }
    }
}
