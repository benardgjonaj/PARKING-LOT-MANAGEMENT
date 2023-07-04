using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface IParkingSpotRepository
    {
        Task<ParkingSpot> GetParkingSpots();
       
    }
}
