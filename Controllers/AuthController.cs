using DatingApp.API.Data;
using DatingApp.API.DTOSs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using AutoMapper;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repository, IConfiguration configuration,
                                IMapper mapper)
        {
            _repo = repository;
            _configuration = configuration;
            _mapper = mapper;
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

            var userToCreate = _mapper.Map<User>(user);
            var createdUser = await _repo.Register(userToCreate, user.Password);

            // another dto could be created but no need for it
            // we cannot return createdUser in CreateAtRoute cause it has password etc. 
            var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);


            //with CreatedAtRoute we send back Location Header with the Request as well as
            // the resource we created - createdUser
            // Location Header: http://localhost:5000/api/Users/11?controllers=Users 
            // this is a route to get this individual user as well
            return CreatedAtRoute("GetUser", new { controllers = "Users", id=createdUser.Id}, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto user)
        {
            //try
            //{
            //    //code
            //    throw new Exception("siemka to ja exception");
            //}
            //catch
            //{
            //    return StatusCode(500, "hello from the catch block!");
            //}

            var userFromRepo = await _repo.Login(user.Username.ToString().ToLower(), user.Password);

            if(userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),    
                new Claim(ClaimTypes.Name, userFromRepo.Username)    
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //returing user's main photo, no new dto for clarity
            var userForPhoto = _mapper.Map<UserForListDto>(userFromRepo);


            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                userForPhoto
            });

        }
    }
}
