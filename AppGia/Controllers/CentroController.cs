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
        [HttpGet("{id}", Name = "")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Centro
        [HttpPost]
        public int Create([FromBody] CentroCostos centro)
            {
                return objcentro.AddCentro(centro);
            }

        // PUT: api/Centro/5
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
