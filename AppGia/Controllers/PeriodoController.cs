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
    public class PeriodoController : ControllerBase
    {
        PeriodoDataAccessLayer objPeriodo = new PeriodoDataAccessLayer();
        Periodo periodo = new Periodo();
        // GET: api/Periodo
        [HttpGet]
        public IEnumerable<Periodo> Get()
        {
            return objPeriodo.GetAllPeriodos();
        }

        // GET: api/Periodo/5
        [HttpGet("{id}", Name = "GetPeriodo")]
        public Periodo GetPeriodo(string id)
        {
            return objPeriodo.GetPeriodoData(id);
        }

        // POST: api/Periodo
        [HttpPost]
        public void Create([FromBody]Periodo periodo)
        {
            objPeriodo.AddPeriodo(periodo);
        }

        // PUT: api/Periodo/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Periodo periodo)
        {
            return objPeriodo.updatePeriodo(id, periodo);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objPeriodo.deletePeriodo(id);
        }
    }
}
