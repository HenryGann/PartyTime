using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyTime.Models
{
    [Table("events")]
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("summary")]
        public string Summary { get; set; }

        [Column("location")]
        public string Location { get; set; }

        [Column("date_time")]
        public DateTime DateTime { get; set; }

        [Column("owner")] // Foreign key column
        public int OwnerId { get; set; }

        // Parameterized constructor
        public Event(int id, string title, string summary, string location, DateTime dateTime, int ownerId)
        {
            Id = id;
            Title = title;
            Summary = summary;
            Location = location;
            DateTime = dateTime;
            OwnerId = ownerId;
        }

        // Parameterless constructor (required by Entity Framework Core)
        public Event()
        {
        }
    }
    
    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string EventCreator { get; set; }
    }

}
