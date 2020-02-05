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
    public class Empresa_ProyectoController : ControllerBase
    {
        Empresa_ProyectoDataAccessLayer objempresapro = new Empresa_ProyectoDataAccessLayer();
        // GET: api/Empresa_Proyecto
        [HttpGet("{id}", Name = "Gett")]
        public IEnumerable<Empresa> Gett(int id)
        {
            return objempresapro.GetAllEmpresaByProyectoId(id);
        }

        /* GET: api/Empresa_Proyecto/5
        [HttpGet("{id}", Name = "Gett")]
        public string Gett(int id)
        {
            return "value";
        }*/

        // POST: api/Empresa_Proyecto
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Empresa_Proyecto/5
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
