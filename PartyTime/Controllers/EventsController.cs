using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyTime.Contexts;
using PartyTime.Models;
using PartyTime.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartyTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {


        private readonly EventRepository eventRepository;
        private readonly ApplicationDbContext _context;
        private readonly string _secret;

        public EventsController(ApplicationDbContext context)
        {
            eventRepository = new EventRepository(context); 
            _context = context;
            _secret = Environment.GetEnvironmentVariable("Jwt:secret");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var eventList = await eventRepository.GetAllEvents();
            return Ok(eventList);
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await eventRepository.GetEventWithUsername(id);
            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] NewEventDTO eventDTO)
        {
            var newEvent = await eventRepository.EventDTOtoEvent(eventDTO);
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var e = await _context.Events.FindAsync(id);
            if (e == null)
            {
                return NotFound();
            }

            _context.Events.Remove(e);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
