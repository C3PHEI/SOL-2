using Microsoft.AspNetCore.Mvc;
using SOL_2_API.Models;
using SOL_2_API.Services;
using MongoDB.Bson;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace SOL_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public UsersController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _dbService.Users.Find(_ => true)
                .Project(u => new
                {
                    Id = u.Id.ToString(),
                    Username = u.Username,
                    Password = u.Password,
                    Role = u.Role
                })
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest(new { message = "Ungültiges ID-Format." });
            }

            var filter = Builders<User>.Filter.Eq(u => u.Id, objectId);
            var user = await _dbService.Users.Find(filter)
                .Project(u => new
                {
                    Id = u.Id.ToString(),
                    Username = u.Username,
                    Password = u.Password,
                    Role = u.Role
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}

