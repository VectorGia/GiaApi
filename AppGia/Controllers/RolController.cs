using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        RolDataAccessLayer objRol = new RolDataAccessLayer();

        // GET: api/Grupo
        [HttpGet]
        public IEnumerable<Rol> Get()
        {
            return objRol.GetAllRoles();
        }

        //// GET: api/Grupo/5
        //[HttpGet("{id}", Name = "")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Grupo
        [HttpPost]
        public int Create([FromBody]Rol rol)
        {
            return objRol.addRol(rol);
        }

        //// PUT: api/Grupo/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
