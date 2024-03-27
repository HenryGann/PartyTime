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

        public Event(EventBaseDTO eventDTO, int ownerId)
        {
            if (eventDTO == null)
            {
                throw new ArgumentNullException(nameof(eventDTO));
            }

            Title = eventDTO.Title;
            Summary = eventDTO.Summary;
            Location = eventDTO.Location;
            DateTime = eventDTO.DateTime;
            OwnerId = ownerId;
        }
    }

    public class NewEvent
    {

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public int OwnerId { get; set; }

        public NewEvent(EventDTO eventDTO, int ownerId)
        {
            if (eventDTO == null)
            {
                throw new ArgumentNullException(nameof(eventDTO));
            }

            Title = eventDTO.Title;
            Summary = eventDTO.Summary;
            Location = eventDTO.Location;
            DateTime = eventDTO.DateTime;
            OwnerId = ownerId;
        }
    }

    public class EventBaseDTO
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class EventDTO : EventBaseDTO
    {
        public int Id { get; set; }
        public string EventCreator { get; set; }
    }

    public class NewEventDTO : EventBaseDTO
    {
        public string EventCreator { get; set; }
    }
}
