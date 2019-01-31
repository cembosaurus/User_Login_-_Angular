using Users.Data;
using Users.DTOs;
using Users.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Custom;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config; 
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegister user)
        {
            var username = user.Name.ToLower();

            if(await _repo.UserExist(username))
                return BadRequest("User already exist in dabtabase!");

            User createdUser = new User{ Name = username };

            var savedUser = await _repo.Register(createdUser, user.Password);

            return StatusCode(201);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLogin user)
        {
            var userFromRepo = await _repo.Login(user.Name.ToLower(), user.Password);

            if(userFromRepo == null)
                return Unauthorized();

            var secret = _config.GetSection("Cembo_Settings:Token").Value;

            var token = new JWTToken(userFromRepo, secret);

            return Ok(new { token = token.Build() });
        }
    }
}