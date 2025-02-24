using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using API_CIUSSS_1830974.Data.Context;
using API_CIUSSS_1830974.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace API_CIUSSS_1830974.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly CiusssContext _Context;
        public LogsController(CiusssContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logs>>> GetAll(DateTime start, DateTime end)
        {
            if (start > end)
                return BadRequest("Start date must be earlier than end date.");

            if (!_Context.Logs.Any())
                return NotFound("No logs found in the specified date range.");

            List<Logs> logs = await _Context.Logs
                            .Where(l => l.EntryTime >= start && l.EntryTime <= end)
                            .ToListAsync();

            return Ok(logs);
        }

        /// <summary>
        /// Type de log valid : entry, exit, prices, usercreate, userupdate, 
        /// userdelete, ticketdelete, payment
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [HttpGet("{logType}")]
        public async Task<ActionResult<IEnumerable<Logs>>> GetActivityLogs([Required] string logType, [Required] DateTime start, [Required] DateTime end)
        {
            if (start > end)
                return BadRequest("Start date must be earlier than end date.");

            if (!_Context.Logs.Any())
                return NotFound("No logs found in the specified date range.");

            string origin = GetOrigin(logType);
            if (string.IsNullOrEmpty(origin))
                return BadRequest("Invalid log type.");

            List<Logs> logs = await _Context.Logs
                .Where(l => l.Origin == origin)
                .Where(l => l.EntryTime >= start && l.EntryTime <= end)
                .ToListAsync();

            return Ok(logs);
        }

        private string GetOrigin(string logType)
        {
            return logType.ToLower() switch
            {
                "entry" => "Borne d'entrée",
                "exit" => "Borne de sortie",
                "prices" => "tarifs",
                "usercreate" => "Création d'utilisateur",
                "userupdate" => "Modification d'utilisateur",
                "userdelete" => "Suppression d'utilisateur",
                "ticketdelete" => "Suppression de billet",
                "payment" => "Borne de paiement",
                "reciept" => "Impression de reçu",
                _ => string.Empty
            };
        }
    }
}
