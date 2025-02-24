using Microsoft.EntityFrameworkCore;

namespace ParkingApp_1830974.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool State { get; set; }

        public User() { }

        public User(string username, string password, string name, string lastName, string email, bool state)
        {
            Username = username;
            Password = password;
            FirstName = name;
            LastName = lastName;
            Email = email;
            State = state;
        }
    }
}
