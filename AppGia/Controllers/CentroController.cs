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
        public CentroCostos GetCentro(int id)
        {
            return objcentro.GetCentroData(id);
        }

        // POST: api/Centro
        [HttpPost]
        public int Create([FromBody] CentroCostos centro)
            {
                return objcentro.AddCentro(centro);
            }

        // PUT: api/Centro/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] CentroCostos centro)
        {
            return objcentro.update(id, centro);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objcentro.Delete(id);
        }
    }
}
