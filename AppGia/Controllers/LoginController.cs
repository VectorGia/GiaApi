using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
using System.DirectoryServices;
using AppGia.Dao;
using Npgsql;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/Login
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public Response Login(Login lg)
        {

            string dominio = "infogia";
            string path = "LDAP://ServerOmnisys/CN=users, DC=Infogia, DC=local";
            string domainAndUsername = dominio + @"\" + lg.UserName;
            DirectoryEntry entry = new DirectoryEntry(path,
            domainAndUsername, lg.Password);
            try
            {
                DirectorySearcher dir = new DirectorySearcher(entry);
                dir.ToString();
                SearchResult result = dir.FindOne();

                Relacion relacion = new Relacion();

                LoginDataAccessLayer loginDLayer = new LoginDataAccessLayer();
                loginDLayer.validacionLoginUsuario(relacion, lg);

                bool existe = loginDLayer.validacionLoginUsuario(relacion, lg);

                if (existe == true)
                {
                    return new Response { MESSAGE = true };
                }
                else
                {
                    return new Response { MESSAGE = false };
                }

            }
            catch (DirectoryServicesCOMException cex)
            {
                return new Response { MESSAGE = false };
            }

            catch (Exception ex)
            {
                return new Response { MESSAGE = false };
            }
        }
    }
}