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
    public class UsuarioController : ControllerBase
    {
        UsuarioDataAccessLayer objusuario = new UsuarioDataAccessLayer();
        UsersADController objUserAD = new UsersADController();
        // GET: api/Usuario
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            
            return objusuario.GetAllUsuarios();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}", Name = "GetUsuario")]
        public Usuario GetUsuario(string id)
        {
            return objusuario.GetUsuario(id);
        }
       
        // POST: api/Usuario
        [HttpPost]
        public int Post([FromBody] Usuario usuario)
        {
            return objusuario.addUsuario(usuario);
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Usuario usuario)
        {
            return objusuario.updateUsuario(id,usuario);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objusuario.DeleteUser(id);
        }
    }
}
