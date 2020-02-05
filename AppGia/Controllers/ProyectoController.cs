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
    public class ProyectoController : ControllerBase
    {
        ProyectoDataAccessLayer objProyecto = new ProyectoDataAccessLayer();
        Proyecto proyecto = new Proyecto();
        // GET: api/Proyecto
        [HttpGet]
        public IEnumerable<Proyecto> Get()
        {
            return objProyecto.GetAllProyectos();
        }

        // GET: api/Proyecto/5
        [HttpGet("{id}", Name = "GetProyecto")]
        public Proyecto GetProyecto(string id)
        {
            return objProyecto.GetProyectoData(id);
        }

        // POST: api/Proyecto
        // POST: api/Proyecto
        [HttpPost]
        public void Create([FromBody]Proyecto proyecto)
        {
            long id = objProyecto.addProyecto(proyecto);
            objProyecto.addEmpresa_Proyecto(id, proyecto);
        }

        // PUT: api/Proyecto/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Proyecto proyecto)
        {
            return objProyecto.update(id, proyecto);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objProyecto.Delete(id);
        }      
    }
}