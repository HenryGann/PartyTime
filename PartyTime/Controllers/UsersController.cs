using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PartyTime.Util;
using PartyTime.Contexts;
using PartyTime.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PartyTime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _secret;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
            _secret = Environment.GetEnvironmentVariable("Jwt:secret");
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var Users = _context.Users.FirstOrDefault(u => u.Id == 1);
            return Ok(Users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost("Login")]
        public IActionResult LoginPost([FromBody] LoginModel body)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == body.username);

            if (user == null)
            {
                // Return a 404 Not Found response if the user is not found
                return NotFound("User not found");
            }

            if (Auth.CalculateUserSHA256(body.password, user.Salt) == user.Password)
            {
                var accessToken = Auth.GenerateAccessToken(user, _secret);

                return Ok(accessToken);
            }
            else
            {
                // Return a 401 Unauthorized response if the password is incorrect
                return Unauthorized("Incorrect password");
            }
        }

        [HttpPost]
        public IActionResult CreateUserPost([FromBody] NewUser newUser)
        {
            User user = new(newUser);
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
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
