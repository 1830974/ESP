using API_CIUSSS_1830974.Data.Context;
using API_CIUSSS_1830974.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API_CIUSSS_1830974.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecieptController : ControllerBase
    {
        private readonly CiusssContext _Context;
        public RecieptController(CiusssContext context)
        {
            _Context = context;
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddReciept([Required]Reciept reciept)
        {
            _Context.Reciepts.Add(reciept);
            await _Context.SaveChangesAsync();

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Impression de reçu",
                Description = $"Impression du reçu {reciept.Id} pour le billet {reciept.TicketId}",
            };

            _Context.Logs.Add(newLogs);
            await _Context.SaveChangesAsync();

            return Ok("");
        }
    }
}
