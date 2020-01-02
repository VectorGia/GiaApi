using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        RolDataAccessLayer objRol = new RolDataAccessLayer();
        // GET: api/Rol
          [HttpGet]
        public IEnumerable<Rol> Get()
        {
            return objRol.GetAllRoles();
        }

        //// GET: api/Rol/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Rol
        [HttpPost]
        public int Create([FromBody]Rol rol)
        {
            return objRol.addRol(rol);
        }

        // PUT: api/Rol/5
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
