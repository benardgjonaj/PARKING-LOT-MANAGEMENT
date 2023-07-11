using ParkingLotManagementAPI.Entities;
using ParkingLotManagementAPI.Models;

namespace ParkingLotManagementAPI.Services
{
    public interface IParkingSpotRepository
    {
        Task<ParkingSpotViewDTO> GetParkingSpotInfo();
        Task<ParkingSpot> GetParkingSpot(int id);
        Task<bool> SaveChangesAsync();


    }
}
