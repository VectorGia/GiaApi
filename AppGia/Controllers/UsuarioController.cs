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
    public class UsuarioController : ControllerBase
    {
        UsuarioDataAccessLayer objusuario = new UsuarioDataAccessLayer();
        // GET: api/Usuario
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return objusuario.GetAllUsuarios();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}", Name = "GetUsuario")]
        public string GetUsuario(int id)
        {
            return "value";
        }
       
        // POST: api/Usuario
        [HttpPost]
        public int Post([FromBody] Usuario usuario)
        {
            return objusuario.InsertaUsuarios(usuario);
        }

        // PUT: api/Usuario/5
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
