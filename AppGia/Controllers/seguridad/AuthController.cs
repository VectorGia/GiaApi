using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Novell.Directory.Ldap;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Int64 MINS_TO_EXPIRIRE = 600;
        private IConfiguration _config;


        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUserAD(login);
            //var user = AuthenticateUserDummy(login);

            if (user != null)
            {
                var token = GenerateJSONWebToken(user);
                List<Dictionary<string, string>> relaciones =
                    new RelacionUsuarioDataAccessLayer().getRelacionesByUserName(user.Username);
                response = Ok(new {token = token, minutes = MINS_TO_EXPIRIRE, relaciones = relaciones});
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress ?? "")
            };
            DateTime expires = DateTime.Now.AddMinutes(MINS_TO_EXPIRIRE);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUserDummy(UserModel login)
        {
            UserModel user = null;

            if (login.Username.Length >= 5)
            {
                return login;
            }

            return user;
        }


        private UserModel AuthenticateUserAD(UserModel userModel)
        {
            string dominio = _config["AD:Dominio"];
            string path =_config["AD:Path"];
            string domainAndUsername = dominio + @"\" + userModel.Username;
            logger.Info("Conectando con ldap. Datos conexion path='{0}',domainAndUsername='{1}' ",path,domainAndUsername);
            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, userModel.Password);
            logger.Info("Conexion ldap OK");
            try
            {
                logger.Info("Autenticando domainAndUsername='{0}'",domainAndUsername);
                object obj = entry.NativeObject;
                logger.Info("Autenticacion ldap OK");
                return userModel;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en autenticacion");
                return null;
            }
        }
    }
}