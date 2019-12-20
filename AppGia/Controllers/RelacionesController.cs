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
    public class RelacionesController : ControllerBase
    {
        RelacionesDataAccessLayer objrelacion = new RelacionesDataAccessLayer();
        // GET: api/Relaciones
        [HttpGet]
        public IEnumerable<Relacion> Get()
        {
            return objrelacion.GetAllRelaciones();
        }

        // GET: api/Relaciones/5
        [HttpGet("{id}", Name = "GetRelaciones")]
        public string GetRelaciones(int id)
        {
            return "value";
        }

        // POST: api/Relaciones
        [HttpPost]
        public int Post([FromBody] Usuario usuario)
        {
            return objrelacion.insert(usuario);
        }

        // PUT: api/Relaciones/5
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
