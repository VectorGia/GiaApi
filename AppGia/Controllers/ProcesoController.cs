using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcesoController : ControllerBase
    {
        ProcesoDataAccessLayer objProceso = new ProcesoDataAccessLayer();
        // GET: api/Proceso
        [HttpGet]
        public IEnumerable<Proceso> Get()
        {
            Proceso proceso = new Proceso();
            proceso.empresa = "quesque la mejor ";
            proceso.estatus = "iniciado ";
            proceso.fecha_inicio = DateTime.Now;
            proceso.fecha_fin = DateTime.Now;
            proceso.mensaje = "error";
            proceso.tipo = "manual";
            proceso.id_empresa = 4;

            objProceso.AddProceso(proceso);
            return objProceso.GetAllProcesos();
        }

        // GET: api/Proceso/5
        [HttpGet("{id}", Name = "GetProcesos")]
        public Proceso GetProceso(int id)
        {
            return objProceso.GetProcesoData(id);
        }

        // POST: api/Proceso
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Proceso/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        public int Create([FromBody]Proceso proceso)
        {
            return objProceso.AddProceso(proceso);
        }
    }
}
