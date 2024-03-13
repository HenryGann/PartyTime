using Microsoft.EntityFrameworkCore;
using PartyTime.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyTime.Models
{
    [Table("users")]
    public class UserModel
    {
        private int id { get; set; }
        private string first_name { get; set; }
        private string last_name { get; set; }
        [Key]
        public string username { get; set; }
        public string password { get; set; }

        public string salt { get; set; }
        private string role { get; set; }
    }
}
