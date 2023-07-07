using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface ILogsRepository
    {
        Task<IEnumerable<Logs>> GetLogsAsync(string? searchQuery);
        Task<IEnumerable<Logs>> GetLogsByDayAsync(DateTime day);
        Task AddLogAsync(Logs log);

        void DeleteLog(int id);

        public decimal CalculatePrice(Logs log);

    }
}
