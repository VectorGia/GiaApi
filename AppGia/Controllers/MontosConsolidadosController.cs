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
    public class MontosConsolidadosController : ControllerBase
    {
        MontosConsolidadosDataAccessLayer objmontos = new MontosConsolidadosDataAccessLayer();
        // GET: api/MontosConsolidados
        [HttpGet]
        public IEnumerable<MontosConsolidados> Get(int montConsAnio, int montConsMes, int montConsEmpresa, int montConsModeloNeg, int montConsProyecto, int montConsRubro)
        {
            return objmontos.GetMontosConsolidados(montConsAnio, montConsMes, montConsEmpresa, montConsModeloNeg, montConsProyecto, montConsRubro);
        }

        // GET: api/MontosConsolidados/5
        [HttpGet("{id}", Name = "GetMontosConsolidados")]
        public string GetMontosConsolidados(int id)
        {
            return "value";
        }

        // POST: api/MontosConsolidados
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MontosConsolidados/5
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
