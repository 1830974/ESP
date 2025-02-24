using API_CIUSSS_1830974.Data.Context;
using API_CIUSSS_1830974.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API_CIUSSS_1830974.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly CiusssContext _Context;
        public UserController(CiusssContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getall()
        {
            return Ok(await _Context.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateUser([Required]User newUser)
        {
            _Context.Users.Add(newUser);
            await _Context.SaveChangesAsync();

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Création d'utilisateur",
                Description = $"Création de l'utilisateur \"{newUser.Username}\"",
            };

            _Context.Logs.Add(newLogs);
            await _Context.SaveChangesAsync();

            return Ok($"New user {newUser.Username} was created");
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateUser([Required]User updatedUser)
        {
            User? originalUser = await _Context.Users.FindAsync(updatedUser.Id);

            if (originalUser == null)
                return NotFound($"User with Id {updatedUser.Id} was not found");

            originalUser = updatedUser;
            await _Context.SaveChangesAsync();

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Modification d'utilisateur",
                Description = $"Modification de l'utilisateur \"{updatedUser.Username}\"",
            };

            return Ok($"User with Id {updatedUser.Id} was updated");
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteUser([Required]User userToDelete)
        {
            User? foundUser = await _Context.Users.FindAsync(userToDelete.Id);

            if (foundUser == null)
                return NotFound($"User with Id {userToDelete.Id} was not found");

            _Context.Users.Remove(foundUser);
            await _Context.SaveChangesAsync();

            Logs newLogs = new Logs()
            {
                Id = 0,
                EntryTime = DateTime.Now,
                Origin = "Suppression d'utilisateur",
                Description = $"Suppression de l'utilisateur \"{userToDelete.Username}\"",
            };

            return Ok($"User with Id {userToDelete.Id} was deleted");
        }
    }
}
