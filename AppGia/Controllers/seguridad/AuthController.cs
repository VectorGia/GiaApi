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
            //var user = AuthenticateUserAD(login);
            var user = AuthenticateUserDummy(login);

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

        private UserModel AuthenticateUserNew(UserModel userModel)
        {
            try
            {
                var cn = new LdapConnection();
                cn.Connect("10.10.0.102", 389);
                logger.Info("Active directory connected='{0}'", cn.Connected);
                String loginDN = "Svc.infogia@gia.mx";
                String password1 = "1nf0g14202009*";

                cn.Bind(loginDN, password1);
                logger.Info("Active directory autenticado='{0}'", cn.Bound);
                cn.Disconnect();
                return userModel;
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en autenticacion de active directory");
                throw;
            }
        }

        private UserModel AuthenticateUserAD(UserModel userModel)
        {
            // DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userModel.Username, userModel.Password);

            string dominio = _config["AD:Dominio"];
            string path =
                "LDAP://10.10.0.102/CN=Servicio infogia,OU=Service Accounts,DC=gia,DC=mx"; //_config["AD:Path"];
            string domainAndUsername = "GIA" + @"\" +"huilver.nolasco"; //dominio + @"\" + userModel.Username;
            logger.Info("Conectando con AD.....");
            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, "2020*HuNo-Ag___");
            logger.Info("Conectado con AD --> ok");
            try
            {
                logger.Info("Forzando autenticacion.....>>>> debe fallar");
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                logger.Info("Buscando usuario en modelo.....");
                /*search.Filter = "(SAMAccountName=" + userModel.Username + ")";
                search.PropertiesToLoad.Add("cn");*/
                /*SearchResult result = search.FindOne();*/
                /*logger.Info("Buscando resultado...." + result);*/


                //Relacion relacion = new Relacion();
                //bool existe = new LoginDataAccessLayer().validacionLoginUsuario(relacion, lg);

                /*if (null == result)
                {
                    return null;
                }*/
                search.Filter = "(&(objectClass=user))";
                /*search.Filter = "(&(objectClass=user)(objectCategory=person))";*/
                //search.PropertiesToLoad.Add("samaccountname");
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        result = resultCol[counter];
                        logger.Info("   =====================res '{0}'======================== ",counter);
                        ResultPropertyCollection myResultPropColl = result.Properties;
                        foreach( string myKey in myResultPropColl.PropertyNames)  
                        {  
                            string tab = "          ";  
                            logger.Info("     key: "+ myKey + " = ");  
                            foreach( Object myCollection in myResultPropColl[myKey])  
                            {  
                                logger.Info(tab + myCollection);  
                            }  
                        }  
                        
                        /*if (result.Properties.Contains("samaccountname"))
                        {
                            logger.Info("usuario:'{0}'", result.Properties["samaccountname"][0]);
                        }*/
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en autenticacion");
                return null;
            }

            return userModel;
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
    }
}