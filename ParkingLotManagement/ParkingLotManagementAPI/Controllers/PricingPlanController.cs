using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotManagementAPI.Entities;
using ParkingLotManagementAPI.Models;
using ParkingLotManagementAPI.Services;

namespace ParkingLotManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingPlanController : ControllerBase
    {
        private readonly IPricingPlanRepository pricingPlanRepository;

    
        public PricingPlanController(IPricingPlanRepository pricingPlanRepository)
        {
            this.pricingPlanRepository = pricingPlanRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PricingPlanDTO>>> PricingPlans()
        {
            var pricingPlansEntities= await pricingPlanRepository.GetPricingPlansAsync();
           var pricingPlansDTOs=new List<PricingPlanDTO>();

            foreach (var pricingplan in pricingPlansEntities)
            {
                pricingPlansDTOs.Add(new PricingPlanDTO {
                Id=pricingplan.Id,
                HourlyPricing=pricingplan.HourlyPricing,
                DailyPricing=pricingplan.DailyPricing,
                MinimumHours=pricingplan.MinimumHours,
                Type=pricingplan.Type
                });
            }
            return Ok(pricingPlansDTOs);
        }
        [HttpPut("{id:int}")]
        public async Task< ActionResult> UpdatePricingPlan(int id, [FromBody] PricingPlan updatedPricingPlanDTO)
        {
            var pricePlaningEntity=await pricingPlanRepository.GetPricingPlanAsync(id);
            if (pricePlaningEntity == null)
            {
                return NotFound();
            }

            pricePlaningEntity.MinimumHours = updatedPricingPlanDTO.MinimumHours;
            pricePlaningEntity.DailyPricing = updatedPricingPlanDTO.DailyPricing;
            pricePlaningEntity.HourlyPricing = updatedPricingPlanDTO.HourlyPricing;
            pricePlaningEntity.Type=updatedPricingPlanDTO.Type;
            await pricingPlanRepository.SaveChangesAsync();

            return NoContent();

        }

    }
}
