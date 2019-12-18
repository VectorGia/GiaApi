﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
using System.DirectoryServices;

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
                return new Response { MESSAGE = true };

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
