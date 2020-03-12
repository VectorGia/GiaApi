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
    public class PeriodoController : ControllerBase
    {

        PeriodoDataAccessLayer objperiodo = new PeriodoDataAccessLayer();
        // GET: api/Periodo
        [HttpGet]
        public IEnumerable<Periodo> Get()
        {
            return objperiodo.GetAllPeriodos();
        }

        // GET: api/Periodo/5
        [HttpGet("{id}", Name = "GetPeriodo")]
        public Periodo Get(string id)
        {
            return objperiodo.GetPeriodoData(id);
        }

        // POST: api/Periodo
        [HttpPost]
        public int Post([FromBody] Periodo periodo)
        {
            return objperiodo.AddPeriodo(periodo);
        }

        // PUT: api/Periodo/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Periodo periodo)
        {
            return objperiodo.updatePeriodo(id, periodo);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objperiodo.deletePeriodo(id);
        }
    }
}
