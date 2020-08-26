using System;
using System.DirectoryServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppGia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Int64 MINS_TO_EXPIRIRE = 5;
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
            var user = AuthenticateUserDummy(login);

            if (user != null)
            {
                var token = GenerateJSONWebToken(user);
                response = Ok(new {token = token, minutes = MINS_TO_EXPIRIRE});
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
        /*public UserModel AuthenticateUser(UserModel userModel)
        {
            string dominio = "infogia";
            string path = "LDAP://ServerOmnisys/CN=users, DC=Infogia, DC=local";
            string domainAndUsername = dominio + @"\" + userModel.Username;
            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, userModel.Password);
            try
            {
                DirectorySearcher dirSearcher = new DirectorySearcher(entry);
                //dirSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
                dirSearcher.FindOne();
                //Relacion relacion = new Relacion();
                //bool existe = new LoginDataAccessLayer().validacionLoginUsuario(relacion, lg);
                return userModel;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error de autenticacion");
                return null;
            }
        }*/

        private UserModel AuthenticateUserAD(UserModel userModel)
        {
            // DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userModel.Username, userModel.Password);

            string dominio = _config["AD:Dominio"];
            string path = _config["AD:Path"];
            ;
            string domainAndUsername = dominio + @"\" + userModel.Username;

            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, userModel.Password);
            try
            {
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + userModel.Username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                //Relacion relacion = new Relacion();
                //bool existe = new LoginDataAccessLayer().validacionLoginUsuario(relacion, lg);

                if (null == result)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en autenticacion");
                return null;
            }

            return userModel;
        }
    }
}