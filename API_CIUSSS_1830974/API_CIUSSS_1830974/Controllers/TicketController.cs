using API_CIUSSS_1830974.Data.Context;
using API_CIUSSS_1830974.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using API_CIUSSS_1830974.Models.DTO;

namespace API_CIUSSS_1830974.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly CiusssContext _Context;
        public TicketController(CiusssContext context)
        {
            _Context = context;
        }

        [HttpGet("GetAllActive")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetAllActive()
        {
            List<Ticket> tickets = await _Context.Tickets
                                  .Where(t => t.State == "Non-payé")
                                  .ToListAsync();
            return Ok(tickets);
        }

        [HttpPost("GenerateTicket")]
        public async Task<ActionResult<Ticket>> GenerateTicket([FromBody][Required]TicketDTO newTicketDTO)
        {
            Ticket newTicket = new Ticket()
            {
                Id = 0,
                ArrivalTime = DateTime.Now,
                PaymentTime = null,
                LicensePlate = newTicketDTO.LicensePlate,
                State = "Non-payé",
            };

            _Context.Tickets.Add(newTicket);
            await _Context.SaveChangesAsync();
            
            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Borne d'entrée",
                Description = $"Création du billet {newTicket.Id}",
            };

            _Context.Logs.Add(newLogs);
            await _Context.SaveChangesAsync();

            return Ok(newTicket);
        }

        [HttpPut("PayTicket")]
        public async Task<ActionResult<string>> PayTicket([Required]int ticketId)
        {
            Ticket? paidTicket = _Context.Tickets.Find(ticketId);

            if (paidTicket == null)
                return BadRequest($"Could not find ticket with ID {ticketId}");

            paidTicket.PaymentTime = DateTime.Now;
            paidTicket.State = "Payé";

            await _Context.SaveChangesAsync();

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Borne de paiement",
                Description = $"Paiement du billet {ticketId}",
            };

            _Context.Logs.Add(newLogs);
            await _Context.SaveChangesAsync();

            return Ok($"Paiement enregistré pour le billet avec l'ID {ticketId}");
        }

        [HttpPost("DeleteTicket")]
        public async Task<ActionResult<string>> DeleteTicket([Required]int ticketId)
        {
            Ticket? deletedTicket = _Context.Tickets.Find(ticketId);

            if (deletedTicket == null)
                return BadRequest($"Could not find ticket with ID {ticketId}");

            _Context.Tickets.Remove(deletedTicket);
            await _Context.SaveChangesAsync();

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Suppression de billet",
                Description = $"Suppression du billet {ticketId}",
            };

            _Context.Logs.Add(newLogs);
            await _Context.SaveChangesAsync();

            return Ok($"Billet {ticketId} supprimé avec succès");
        }
    }
}
