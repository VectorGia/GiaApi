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
    public class TipoCambioController : ControllerBase
    {
        TipoCambioDataAccessLayer objCambio = new TipoCambioDataAccessLayer();
        // GET: api/TipoCambio
        [HttpGet]
        public IEnumerable<TipoCambio> Get()
        {
            return objCambio.GetAllTipoCambio();
        }

        // GET: api/TipoCambio/5
        [HttpGet("{id}", Name = "GetTipoCambio")]
        public string Get(int id)
        {
            return "value";
        }
        

        // POST: api/TipoCambio
        [HttpPost]
        public int Post([FromBody] TipoCambio cambio)
        {
            return objCambio.insert(cambio);
        }

        // PUT: api/TipoCambio/5
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
