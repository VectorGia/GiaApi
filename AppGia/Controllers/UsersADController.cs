using System;
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
    public class UsersADController : ControllerBase
    {
        // GET: api/UsersAD
        [HttpGet]
        public List<Usuario> Get()
        {
            List<Usuario> rst = new List<Usuario>();

            string path = "LDAP://ServerOmnisys.local/CN=users, DC=Infogia, DC=local", us = "Administrador", pass = "Omnisys1958";
            DirectoryEntry adSearchRoot = new DirectoryEntry(path, us, pass);
            DirectorySearcher adSearcher = new DirectorySearcher(adSearchRoot);

            adSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            SearchResult result;
            SearchResultCollection iResult = adSearcher.FindAll();

            Usuario item;
            if (iResult != null)
            {
                for (int counter = 3; counter < iResult.Count; counter++)
                {
                    result = iResult[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        item = new Usuario();

                        item.userName = (String)result.Properties["samaccountname"][0];

                        if (result.Properties.Contains("displayname"))
                        {
                            item.displayName = (String)result.Properties["displayname"][0];
                        }

                        rst.Add(item);
                    }
                }
            }
            else
            {
                // PONER SI VIENE NULLO

            }

            adSearcher.Dispose();
            adSearchRoot.Dispose();

            return rst;

            // aqui debemos de hacer elinssert 

        }

        // GET: api/UsersAD/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UsersAD
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UsersAD/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
