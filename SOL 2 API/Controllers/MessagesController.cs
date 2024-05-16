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
    public class MessagesController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public MessagesController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var message = new Message
            {
                UserId = ObjectId.Parse(userId),
                MessageContent = model.Message
            };

            await _dbService.Messages.InsertOneAsync(message);
            return Ok(new { message = "Message sent successfully" });
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _dbService.Messages.Find(_ => true).ToListAsync();
            return Ok(messages);
        }
    }
}

