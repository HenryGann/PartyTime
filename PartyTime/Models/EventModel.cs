using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyTime.Models
{
    public class NewEvent(string title, string summary, string location, DateTime dateTime)
    {
        public string Title { get; set; } = title;
        public string Summary { get; set; } = summary;
        public string Location { get; set; } = location;
        public DateTime DateTime { get; set; } = dateTime;
    }

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

        public Event(int id, string title, string summary, string location, DateTime dateTime)
        {
            Id = id;
            Title = title;
            Summary = summary;
            Location = location;
            DateTime = dateTime;
        }

        public Event(NewEvent newEvent)
        {
            Title = newEvent.Title;
            Summary = newEvent.Summary;
            Location = newEvent.Location;
            DateTime = newEvent.DateTime;
        }
    }
}
