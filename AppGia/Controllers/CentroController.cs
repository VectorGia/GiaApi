using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentroController : ControllerBase
    {
        CentroCostosDataAccessLayer objcentro = new CentroCostosDataAccessLayer();
        // GET: api/Centro
        [HttpGet]
        public IEnumerable<CentroCostos> Get()
        {
            return objcentro.GetAllCentros();
        }

        // GET: api/Centro/5
        [HttpGet("{id}", Name = "GetCentro")]
        public List<CentroCostos> GetCentro(int id)
        {
            return objcentro.GetCentroData(id);
        }

        // POST: api/Centro
        [HttpPost]
        public int Create([FromBody] CentroCostos centroCostos)
            {
                return objcentro.AddCentroManageModelos(centroCostos);
            }

        // PUT: api/Centro/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] CentroCostos centroCostos)
        {
            return objcentro.update(id, centroCostos);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return objcentro.Delete(id);
        }
    }
}
