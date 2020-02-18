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
        TipoCambioDataAccessLayer objTipoCambio = new TipoCambioDataAccessLayer();

        // GET: api/ActualizaProforma
        [HttpGet]
        public List<TipoCambio> Get(int idProforma)
        {
            return objTipoCambio.GetTpoCambioPorIdProforma(idProforma);
        }

        // GET: api/ActualizaProforma/5
        [HttpGet("{id}", Name = "GetActualiza")]
        public string Get(string value)
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
