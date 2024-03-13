using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PartyTime.Util;
using PartyTime.Contexts;
using PartyTime.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartyTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var Users = _context.Users.FromSqlRaw("SELECT * FROM users").ToList();
            return Ok(Users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] LoginModel body)
        {
            var user = _context.Users.Where(u => u.username == body.username).FirstOrDefault();
            Console.Out.WriteLine(Auth.CalculateUserSHA256(user));
            if (Auth.CalculateUserSHA256(user) == user.password)
            {
                return Ok("Correct");
            }
            else
            {
                return Unauthorized("Wrong Password");
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
