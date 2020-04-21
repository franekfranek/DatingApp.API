using DatingApp.API.Data;
using DatingApp.API.DTOSs;
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
        public async Task<IActionResult> Register(UserForRegisterDto user) //[FromBody] can be used here but its not necesarry cuase of[ApiController]
        {
            //also (!ModelState.IsValid is not necesarry here because ApiController supply it)
            user.Username = user.Username.ToLower();

            if(await _repo.UserExist(user.Username)) 
            { 
                return BadRequest("Username already exists!");
            }

            var userToCreate = new User
            {
                Username = user.Username,
            };
            var createdUser = await _repo.Register(userToCreate, user.Password);

            return StatusCode(201);
        }
    }
}
