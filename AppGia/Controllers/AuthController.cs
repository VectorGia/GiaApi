using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppGia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AuthController : ControllerBase    
    {    
        private IConfiguration _config;    
    
        public AuthController(IConfiguration config)    
        {    
            _config = config;    
        }    
        [AllowAnonymous]    
        [HttpPost]
        public IActionResult Login([FromBody]UserModel login)    
        {    
            IActionResult response = Unauthorized();    
            var user = AuthenticateUser(login);    
    
            if (user != null)    
            {    
                var tokenString = GenerateJSONWebToken(user);    
                response = Ok(new { token = tokenString });    
            }    
    
            return response;    
        }    
    
        private string GenerateJSONWebToken(UserModel userInfo)    
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
            var claims = new[] {    
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),    
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress)   
            };    
    
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],    
                _config["Jwt:Issuer"],    
                claims,    
                expires: DateTime.Now.AddDays(1),    
                signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }    
    
        private UserModel AuthenticateUser(UserModel login)    
        {    
            UserModel user = null;    
 
            if (login.Username.Length > 0 )    
            {    
                user = new UserModel { Username = "HNA", EmailAddress = "hna@gmail.com" };    
            }    
            return user;    
        }    
    }    
}