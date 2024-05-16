using Microsoft.AspNetCore.Mvc;
using SOL_2_API.Models;
using SOL_2_API.Services;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System;

namespace SOL_2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly string _secret = "yoursecretkey";  // Replace with your actual secret key

        public UsersController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = model.Role
            };

            await _dbService.Users.InsertOneAsync(user);
            await _dbService.CashRegisters.InsertOneAsync(new CashRegister { UserId = user.Id });

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _dbService.Users.Find(u => u.Username == model.Username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("yourissuer", "youraudience", claims, expires: DateTime.Now.AddHours(1), signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), role = user.Role });
        }
    }
}

