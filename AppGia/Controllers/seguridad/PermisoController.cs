using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        PermisoDataAccessLayer permiso = new PermisoDataAccessLayer();
        // GET: api/Permiso
        [HttpGet]
        public IEnumerable<Permiso> Get()
        {
            return permiso.GetAllPermisos();
        }

        // GET: api/Permiso/5
        [HttpGet("{id}", Name = "GetPermisos")]
        public string GetPermisos(int id)
        {
            return "value";
        }

        // POST: api/Permiso
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Permiso/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithAction/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
