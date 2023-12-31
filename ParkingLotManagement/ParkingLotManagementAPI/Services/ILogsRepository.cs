﻿using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public interface ILogsRepository
    {
        Task<IEnumerable<Logs>> GetLogsAsync(string? searchQuery);
        Task<IEnumerable<Logs>> GetLogsByDayAsync(DateTime day);
        Task<Logs> GetLogByDateAsync(DateTime date);
        Task<Logs> GetLogByCodeAsync(string code);
        Task AddLogAsync(Logs log);



        public decimal CalculatePrice(Logs log);
        Task<Logs> FindLogByCode(string code);
        bool SuscriptionCheckedIn(int? subscriptionId);
       bool SubscriptionNotValid(Subscription subscription);
        bool SubscriptionExpired(Subscription subscription);
        bool ExistingCode(string code);
        Task<bool> SaveChangesAsync();

    }
}
