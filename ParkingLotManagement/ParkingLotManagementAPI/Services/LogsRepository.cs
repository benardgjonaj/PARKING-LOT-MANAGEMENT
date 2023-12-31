﻿using Microsoft.EntityFrameworkCore;
using ParkingLotManagementAPI.Data;
using ParkingLotManagementAPI.Entities;

namespace ParkingLotManagementAPI.Services
{
    public class LogsRepository : ILogsRepository
    {
        private readonly ApplicationContext context;

        public LogsRepository(ApplicationContext context)
        {
            this.context = context;
        }
        public async Task AddLogAsync(Logs log)
        {
            await context.AddAsync(log);
            await context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Logs>> GetLogsAsync(string? searchQuery)
        {
            var logs = context.Logs.AsQueryable();
            var logsBySubscriber=new List<Logs>();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                logs = logs.Where(l => l.Code == searchQuery);
            }


            var matchingSubscribers = context.Subscribers
    .Where(subscriber => subscriber.FirstName == searchQuery)
    .ToList();
            if(matchingSubscribers.Any())
            {
                var matchingSubscriberIds = matchingSubscribers.Select(subscriber => subscriber.Id).ToList();
             logsBySubscriber = context.Logs
       .Where(log => matchingSubscriberIds.Contains(log.Subscription.SubscriberId))
       .ToList();
            }
            logs.ToList();
           var result=new List<Logs> ();

            result.AddRange(logs);
            result.AddRange(logsBySubscriber);





            return  result.ToList();
           
        }

        public async Task<IEnumerable<Logs>> GetLogsByDayAsync(DateTime day)
        {
            return await context.Logs.Where(l => l.CheckInTime.Date == day.Date).ToListAsync();
        }
        public decimal CalculatePrice(Logs log)
        {


            decimal totalPrice = 0;
            int totalMin = (int)(log.CheckOutTime - log.CheckInTime).TotalMinutes;
            decimal totalHours = (decimal)totalMin / 60;
            bool iswekeend = log.CheckInTime.DayOfWeek == DayOfWeek.Saturday || log.CheckInTime.DayOfWeek == DayOfWeek.Sunday;
            var pricingPlan = context.PricingPlans.FirstOrDefault(p => p.Type.ToLower() == (iswekeend ? "weekend" : "weekday"));
            if (log.SubscriptionId != null || totalMin <= 15)
            {
                return 0;
            }

            if ((int)totalHours <= pricingPlan.MinimumHours)
            {
                totalPrice = totalHours * pricingPlan.HourlyPricing;
            }
            else
            {
                int numberOfDays = (int)totalHours / 24;
                decimal remainingHours = totalHours % 24;
                if ((int)remainingHours <= pricingPlan.MinimumHours)
                {
                    totalPrice = numberOfDays * pricingPlan.DailyPricing + remainingHours * pricingPlan.HourlyPricing;
                }
                else
                {
                    totalPrice = (numberOfDays + 1) * pricingPlan.DailyPricing;
                }
            }
            return totalPrice;

        }
        public async Task<Logs> FindLogByCode(string code)
        {
            return await context.Logs.FirstOrDefaultAsync(l => l.Code == code);
        }

        public bool ExistingCode(string code)
        {
            var existingLog = context.Logs.FirstOrDefault(l => l.Code == code);
            if (existingLog != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync() >= 0);
        }

        public bool SuscriptionCheckedIn(int? subscriptionId)
        {
            if (subscriptionId != null)
            {
                var log = context.Logs.Where(l => l.SubscriptionId == subscriptionId)
                    .FirstOrDefault(l => l.CheckOutTime == DateTime.MinValue);

                if (log != null)
                {
                    return true;
                }
                return false;
            }

            return false;

        }
        public bool SubscriptionExpired(Subscription subscription)
        {

            if (subscription.EndDate <= DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public bool SubscriptionNotValid(Subscription subscription)
        {

            if (subscription.IsDeleted ==true)
            {
                return true;
            }
            return false;
        }

        public async Task<Logs> GetLogByDateAsync(DateTime date)
        {
            return await context.Logs.FirstOrDefaultAsync(l => l.CheckInTime == date);
        }

        public async Task<Logs> GetLogByCodeAsync(string code)
        {
            return await context.Logs.FirstOrDefaultAsync(l => l.Code == code);
        }
    }
}
