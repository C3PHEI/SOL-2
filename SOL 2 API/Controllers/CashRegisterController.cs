using Microsoft.AspNetCore.Mvc;
using SOL_2_API.Models;
using SOL_2_API.Services;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace SOL_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashRegisterController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public CashRegisterController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCashRegisters()
        {
            var cashRegisters = await _dbService.CashRegisters.Find(_ => true).ToListAsync();
            return Ok(cashRegisters);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCashRegister([FromBody] CashRegister model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var filter = Builders<CashRegister>.Filter.Eq(c => c.UserId, ObjectId.Parse(userId));
            var update = Builders<CashRegister>.Update
                .Set(c => c.Bills, model.Bills)
                .Set(c => c.Coins, model.Coins);

            var result = await _dbService.CashRegisters.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 0)
            {
                return NotFound(new { message = "Cash register not found" });
            }

            return Ok(new { message = "Cash register updated successfully" });
        }
    }
}