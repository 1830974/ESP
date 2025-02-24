using API_CIUSSS_1830974.Data.Context;
using API_CIUSSS_1830974.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API_CIUSSS_1830974.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly CiusssContext _Context;
        public PricesController(CiusssContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prices>>> GetAll()
        {
            List<Prices> prices = await _Context.Prices.ToListAsync();

            return Ok(prices);
        }

        [HttpGet("CurrentPrices")]
        public async Task<ActionResult<IEnumerable<Prices>>> GetCurrent()
        {
            List<Prices?> latestPrices = await _Context.Prices
                                        .GroupBy(p => p.PriceName)
                                        .Select(g => g.OrderByDescending(p => p.SetPriceDate)
                                        .FirstOrDefault())
                                        .ToListAsync();

            return Ok(latestPrices);
        }

        [HttpGet("DatePrice")]
        public async Task<ActionResult<IEnumerable<Prices>>> GetPriceForDate([Required]DateTime start)
        {
            List<Prices?> activePrices = await _Context.Prices
                .Where(p => p.SetPriceDate <= start)
                .GroupBy(p => p.PriceName)
                .Select(g => g.OrderByDescending(p => p.SetPriceDate).FirstOrDefault())
                .ToListAsync();

            if (activePrices.Count == 0)
                return NotFound("No prices found for the specified time period.");

            return Ok(activePrices);
        }

        [HttpPost("SetHourly")]
        public async Task<ActionResult<string>> SetHourlyRate([Required] double newRate)
            => await SetRateAsync("Horaire", newRate);


        [HttpPost("SetHalfDay")]
        public async Task<ActionResult<string>> SetHalfDayRate([Required] double newRate)
            => await SetRateAsync("Demi-journée", newRate);

        [HttpPost("SetDaily")]
        public async Task<ActionResult<string>> SetDailyRate([Required] double newRate)
            => await SetRateAsync("Journée complète", newRate);



        [HttpPut("SetTPS")]
        public async Task<ActionResult<Prices>> SetTPS([Required]double newTPS)
        {
            if (newTPS <= 0)
                return BadRequest("New rate cannot be lower than 0%");

            double? currentTPS = _Context.OtherValues?.FirstOrDefault()?.TPS;

            if (currentTPS == null)
                return NotFound("No TPS rate was found, enter it manually in the database first" +
                    "before using this API call");

            currentTPS = newTPS;
            await _Context.SaveChangesAsync();

            return Ok(currentTPS);
        }

        [HttpPut("SetTVQ")]
        public async Task<ActionResult<Prices>> SetTVQ([Required] double newTVQ)
        {
            if (newTVQ <= 0)
                return BadRequest("New rate cannot be lower than 0%");

            double? currentTVQ = _Context.OtherValues?.FirstOrDefault()?.TPS;

            if (currentTVQ == null)
                return NotFound("No TVQ rate was found, enter it manually in the database first" +
                    "before using this API call");

            currentTVQ = newTVQ;
            await _Context.SaveChangesAsync();

            return Ok(currentTVQ);
        }

        private async Task<ActionResult<string>> SetRateAsync(string priceName, double newRate)
        {

            if (newRate <= 0)
                return BadRequest("New rate cannot be lower than 0.00$");

            Prices newPrice = new Prices()
            {
                Id = 0,
                SetPriceDate = DateTime.Now,
                PriceName = priceName,
                Price = newRate
            };

            _Context.Prices.Add(newPrice);

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "tarif",
                Description = $"Modification du tarif {priceName} {newRate}",
            };

            _Context.Logs.Add(newLogs);
            await _Context.SaveChangesAsync();

            return Ok(newLogs.Description);
        }
    }
}
