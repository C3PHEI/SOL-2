using Microsoft.AspNetCore.Mvc;
using SOL_2_API.Models;
using SOL_2_API.Services;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOL_2_API.Dtos;

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
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllCashRegisters()
        {
            var cashRegisters = await _dbService.CashRegisters.Find(_ => true).ToListAsync();
            var result = cashRegisters.Select(cr => new
            {
                Id = cr.Id.ToString(),
                UserId = cr.UserId.ToString(),
                Bills = cr.Bills,
                Coins = cr.Coins
            }).ToList();
            return Ok(result);
        }

        [HttpGet("byuserid/{userId}")]
        public async Task<IActionResult> GetCashRegistersByUserId(string userId)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return BadRequest("Invalid UserId format.");
            }

            var filter = Builders<CashRegister>.Filter.Eq(cr => cr.UserId, objectId);
            var cashRegisters = await _dbService.CashRegisters.Find(filter).ToListAsync();

            var cashRegisterDtos = cashRegisters.Select(cr => new CashRegisterDto
            {
                Id = cr.Id.ToString(),
                UserId = cr.UserId.ToString(),
                Bills = cr.Bills,
                Coins = cr.Coins
            }).ToList();

            return Ok(cashRegisterDtos);
        }

        [HttpPut("{userId:length(24)}")]
        public async Task<IActionResult> UpdateCashRegisterByUserId(string userId, [FromBody] CashRegister updatedCashRegister)
        {
            if (!ObjectId.TryParse(userId, out var objectId))
            {
                return BadRequest(new { message = "Ungültiges UserID-Format." });
            }

            var filter = Builders<CashRegister>.Filter.Eq(cr => cr.UserId, objectId);
            var cashRegister = await _dbService.CashRegisters.Find(filter).FirstOrDefaultAsync();

            if (cashRegister == null)
            {
                return NotFound();
            }

            // Validate the bills
            foreach (var bill in updatedCashRegister.Bills)
            {
                if (cashRegister.Bills.ContainsKey(bill.Key))
                {
                    if (cashRegister.Bills[bill.Key] + bill.Value < 0)
                    {
                        return BadRequest(new { message = $"Unzureichende {bill.Key}-Scheine zum Entfernen." });
                    }
                }
                else
                {
                    if (bill.Value < 0)
                    {
                        return BadRequest(new { message = $"Kann {bill.Value} von {bill.Key}-Scheinen nicht entfernen, da sie nicht existieren." });
                    }
                }
            }

            // Validate the coins
            foreach (var coin in updatedCashRegister.Coins)
            {
                if (cashRegister.Coins.ContainsKey(coin.Key))
                {
                    if (cashRegister.Coins[coin.Key] + coin.Value < 0)
                    {
                        return BadRequest(new { message = $"Unzureichende {coin.Key}-Münzen zum Entfernen." });
                    }
                }
                else
                {
                    if (coin.Value < 0)
                    {
                        return BadRequest(new { message = $"Kann {coin.Value} von {coin.Key}-Münzen nicht entfernen, da sie nicht existieren." });
                    }
                }
            }

            // Update the bills
            foreach (var bill in updatedCashRegister.Bills)
            {
                if (cashRegister.Bills.ContainsKey(bill.Key))
                {
                    cashRegister.Bills[bill.Key] += bill.Value;
                }
                else
                {
                    cashRegister.Bills[bill.Key] = bill.Value;
                }
            }

            // Update the coins
            foreach (var coin in updatedCashRegister.Coins)
            {
                if (cashRegister.Coins.ContainsKey(coin.Key))
                {
                    cashRegister.Coins[coin.Key] += coin.Value;
                }
                else
                {
                    cashRegister.Coins[coin.Key] = coin.Value;
                }
            }

            await _dbService.CashRegisters.ReplaceOneAsync(filter, cashRegister);
            return NoContent();
        }
    }
}