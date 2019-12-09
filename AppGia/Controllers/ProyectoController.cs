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
        // GET: api/Proyecto
        [HttpGet]
        public IEnumerable<Proyecto> Get()
        {
            return objProyecto.GetAllProyectos();
        }

        //// GET: api/Proyecto/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Proyecto
        [HttpPost]
        public int Create([FromBody]Proyecto proyecto)
        {
            return objProyecto.addProyecto(proyecto);
        }

        //// PUT: api/Proyecto/5
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