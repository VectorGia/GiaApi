using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLMovPolizaSemanalController : ControllerBase
    {
        // GET: api/ETLMovimientoPolizaSemanal
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ETLMovimientoPolizaSemanal/5
        [HttpGet("{id}", Name = "GetMovPolSemanal")]
        public string GetMovPolSemanal(int id)
        {
            return "value";
        }

        // POST: api/ETLMovimientoPolizaSemanal
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ETLMovimientoPolizaSemanal/5
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
