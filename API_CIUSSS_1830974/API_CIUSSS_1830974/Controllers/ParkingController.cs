using API_CIUSSS_1830974.Data.Context;
using API_CIUSSS_1830974.Models;
using API_CIUSSS_1830974.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_CIUSSS_1830974.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly CiusssContext _Context;
        public ParkingController(CiusssContext context)
        {
            _Context = context;
        }

        [HttpGet("State")]
        public async Task<ActionResult<Parking>> GetState()
        {
            Parking? parking = await _Context.Parking.FirstOrDefaultAsync();

            if (parking == null)
                return NotFound("Current parking state is not set");

            return Ok(parking);
        }

        [HttpGet("Plates")]
        public async Task<ActionResult<IEnumerable<LicensePlateDTO>>> GetAllActiveLicensePlates()
        {
            List<LicensePlateDTO> plates = await _Context.Tickets
                                                .Where(t => t.State != "Payé")
                                                .Select(t => new LicensePlateDTO
                                                {
                                                    LicensePlate = t.LicensePlate,
                                                }).ToListAsync();

            return Ok(plates);
        }

        [HttpPut("Occupy")]
        public async Task<ActionResult<Parking>> AddOccupiedSpace()
        {
            Parking? parking = await _Context.Parking.FirstOrDefaultAsync();

            if (parking == null)
                return NotFound("Current parking state is not set");

            if (parking.OccupiedSpaces >= parking.AllSpaces)
                return BadRequest("The parking is already full");

            parking.OccupiedSpaces += 1;

            await _Context.SaveChangesAsync();

            return Ok(parking);
        }

        [HttpPut("Release")]
        public async Task<ActionResult<Parking>> ReleaseSpace()
        {
            Parking? parking = await _Context.Parking.FirstOrDefaultAsync();

            if (parking == null)
                return NotFound("Current parking state is not set");

            if (parking.OccupiedSpaces <= 0)
                return BadRequest("The parking is already empty");

            parking.OccupiedSpaces -= 1;

            await _Context.SaveChangesAsync();

            return Ok(parking);
        }

        [HttpPut("MaxSpaces")]
        public async Task<ActionResult<string>> SetMaxSpaces([Required]int maxSpaces)
        {
            if (maxSpaces <= 0)
                return BadRequest("The parking size should be at least 1");
                
            Parking? parking = await _Context.Parking.FirstOrDefaultAsync();

            if (parking == null)
                return NotFound("Current parking state is not set");

            parking.AllSpaces = maxSpaces;

            await _Context.SaveChangesAsync();

            return Ok();
        }
    }
}
