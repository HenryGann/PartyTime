using Microsoft.EntityFrameworkCore;
using PartyTime.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyTime.Models
{


    [Table("users")]
    public class User
    {
        public User(int id, string username, string password, string salt, string role)
        {
            Id = id;
            Username = username;
            Password = password;
            Salt = salt;
            Role = role;
        }

        [Key]
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string Salt { get; set; }
        public string Role { get; set; }
    }


}
