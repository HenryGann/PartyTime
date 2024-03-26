using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyTime.Contexts;
using PartyTime.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartyTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {


        private readonly EventRepository eventRepository;
        private readonly string _secret;

        public EventsController(ApplicationDbContext context)
        {
            eventRepository = new EventRepository(context); 
            _secret = Environment.GetEnvironmentVariable("Jwt:secret");
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await eventRepository.GetEventWithUsername(id);
            return Ok(eventItem);
        }
    }
}
