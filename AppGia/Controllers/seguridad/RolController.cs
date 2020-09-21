using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/Grupo/5
        [HttpGet("{id}", Name = "GetRol")]
        public Rol Get(string id)
        {
            return objRol.GetRolById(id);
        }

        // POST: api/Rol
        [HttpPost]
        public int Create([FromBody] Rol rol)
        {
            return objRol.addRol(rol);
        }

        // PUT: api/Rol/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Rol value)
        {
            value.INT_IDROL_P = id;
            objRol.update(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Rol rol = new Rol();
            rol.INT_IDROL_P = id;
            objRol.Delete(rol);
        }
    }
}