using Microsoft.AspNetCore.Mvc;
using SOL_2_API.Models;
using SOL_2_API.Services;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Security.Claims;
using MongoDB.Driver;

namespace SOL_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NachfullungController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public NachfullungController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _dbService.Messages.Find(_ => true).ToListAsync();
            return Ok(messages);
        }

        [HttpGet("{userId:length(24)}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetMessageByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return BadRequest(new { message = "Ungültiges UserID-Format." });
            }

            var filter = Builders<Restock>.Filter.Eq("userId", objectId);
            var message = await _dbService.Messages.Find(filter).FirstOrDefaultAsync();

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        [HttpPut("{userId:length(24)}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateNachfullungByUserId(string userId, [FromBody] bool nachfullung)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return BadRequest(new { message = "Ungültiges UserID-Format." });
            }

            var filter = Builders<Restock>.Filter.Eq("userId", objectId);
            var update = Builders<Restock>.Update.Set("nachfullung", nachfullung);
            var result = await _dbService.Messages.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return Ok(new { message = "Nachfüllung erfolgreich aktualisiert" });
        }
    }
}