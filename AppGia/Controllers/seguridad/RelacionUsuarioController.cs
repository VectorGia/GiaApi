using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionUsuarioController : ControllerBase
    {
        RelacionUsuarioDataAccessLayer objrelacionUsuario = new RelacionUsuarioDataAccessLayer();

        // GET: api/RelacionUsuario
        [HttpGet]
        public IEnumerable<Relacion_Usuario> Get()
        {
            return objrelacionUsuario.GetAllRelacionUsuario();
        }

        // GET: api/RelacionUsuario/5
        [HttpGet("{id}", Name = "GetRelacion")]
        //public string GetRelacion(int id)
        //{
        //    //return objrelacionUsuario.GetAllRelacionUsuario(int);
        //}

        // POST: api/RelacionUsuario
        [HttpPost]
        public int Post([FromBody] Relacion_Usuario relacion)
        {
            return objrelacionUsuario.insert(relacion);
        }

        // PUT: api/RelacionUsuario/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            objrelacionUsuario.delete(id);
        }

        [HttpGet("permisos")]
        public List<Dictionary<string, string>> getPermisos()
        {
            return SeguridadConstantes.getPermisos();
        }

        [HttpGet("pantallas")]
        public List<Dictionary<string, string>> getPantallas()
        {
            return SeguridadConstantes.getPantallas();
        }
    }
}