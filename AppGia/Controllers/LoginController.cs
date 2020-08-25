using System;
using System.DirectoryServices;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public IActionResult Login(Login lg)
        {
            string dominio = "infogia";
            string path = "LDAP://ServerOmnisys/CN=users, DC=Infogia, DC=local";
            string domainAndUsername = dominio + @"\" + lg.UserName;
            DirectoryEntry entry = new DirectoryEntry(path, domainAndUsername, lg.Password);
            try
            {
                DirectorySearcher dir = new DirectorySearcher(entry);
                SearchResult result = dir.FindOne();
                Relacion relacion = new Relacion();

                //bool existe = new LoginDataAccessLayer().validacionLoginUsuario(relacion, lg);
                return Ok(new Response {MESSAGE = false});
            }
            catch (Exception ex)
            {
                logger.Error(ex,"Error de autenticacion");
                return Unauthorized();
            }
        }
    }
}