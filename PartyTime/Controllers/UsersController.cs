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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var Users = await _context.Users.ToListAsync();
            return Ok(Users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> GetUserData(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if( user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginPost([FromBody] LoginModel body)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == body.username);

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
        public async Task<IActionResult> CreateUserPost([FromBody] NewUser newUser)
        {
            User user = new(newUser);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
