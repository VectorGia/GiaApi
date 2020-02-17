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
    public class ActualizaProformaController : ControllerBase
    {
        ProformaDataAccessLayer objProforma = new ProformaDataAccessLayer();
        ProformaDetalleDataAccessLayer objProformaDetalle = new ProformaDetalleDataAccessLayer();

        // GET: api/ActualizaProforma
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ActualizaProforma/5
        [HttpGet("{id}", Name = "GetActualiza")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ActualizaProforma
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ActualizaProforma/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] List<ProformaDetalle> profDetalle)
        {
            return objProforma.ActualizaProforma(profDetalle);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
