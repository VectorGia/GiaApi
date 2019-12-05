using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        PermisoDataAccessLayer objPermiso = new PermisoDataAccessLayer();
        // GET: api/Centro
        [HttpGet]
        public IEnumerable<Permiso> Get()
        {
            return objPermiso.GetAllPermisos();
        }

        //// GET: api/Centro/5
        //[HttpGet("{id}", Name = "")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Centro
        [HttpPost]
        public int Create([FromBody] Permiso permiso)
        {
            return objPermiso.addPermiso(permiso);
        }

        //// PUT: api/Centro/5
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
