using Microsoft.EntityFrameworkCore;
using PartyTime.Enums;
using PartyTime.Util;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyTime.Models
{
    public class NewUser
    {
        public NewUser(string username, string password)
        {
            string salt = Guid.NewGuid().ToString();

            Username = username;
            Password = Auth.CalculateUserSHA256(password, salt);
            Salt = salt;
            Role = "User";
        }

        public string Username { get; set; }
        public string Password { get; set; }
        internal string Salt { get; set; }
        internal string Role { get; set; }
    }

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

        public User(NewUser newUser)
        {
            Username = newUser.Username;
            Password = newUser.Password;
            Salt = newUser.Salt;
            Role = newUser.Role;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")] // Explicitly specify the column name
        public string Username { get; set; }

        [Column("password")] // Explicitly specify the column name
        public string Password { get; set; }

        [Column("salt")] // Explicitly specify the column name
        public string Salt { get; set; }

        [Column("role")] // Explicitly specify the column name
        public string Role { get; set; }
    }


}
