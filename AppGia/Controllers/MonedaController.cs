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
    public class MonedaController : ControllerBase
    {
        MonedaDataAccessLayer objmoneda = new MonedaDataAccessLayer();
        // GET: api/Moneda
        [HttpGet]
        public IEnumerable<Moneda> Get()
        {
            return objmoneda.GetAllMoneda();
        }

        // GET: api/Moneda/5
        [HttpGet("{id}", Name = "GetMoneda")]
        public Moneda Get(string id)
        {
            return objmoneda.GetMoneda(id);
        }

        // POST: api/Moneda
        [HttpPost]
        public int Post([FromBody] Moneda moneda)
        {
            return objmoneda.insert(moneda);
        }

        // PUT: api/Moneda/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Moneda moneda)
        {
            return objmoneda.update(id, moneda);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objmoneda.Delete(id);
        }
    }
}
