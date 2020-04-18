using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repository)
        {
            this._repo = repository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            username = username.ToLower();

            if(await _repo.UserExist(username)) { return BadRequest("Username already exists!"); }

            var userToCreate = new User
            {
                Username = username,

            };
            var createdUser = await _repo.Register(userToCreate, password);

            return StatusCode(201);
        }
    }
}
