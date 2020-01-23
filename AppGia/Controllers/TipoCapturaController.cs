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
    public class TipoCapturaController : ControllerBase
    {
        TipoCapturaDataAccessLayer objCaptura = new TipoCapturaDataAccessLayer();
        // GET: api/TipoCaptura
        [HttpGet]
        public IEnumerable<TipoCaptura> Get()
        {
            return objCaptura.GetAllTipoCaptura();
        }

        // GET: api/TipoCaptura/5
        [HttpGet("{id}", Name = "GetTipoCaptura")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TipoCaptura
        [HttpPost]
        public int Post([FromBody] TipoCaptura tipocaptura)
        {
            return objCaptura.AddTipoCaptura(tipocaptura);
        }

        // PUT: api/TipoCaptura/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] TipoCaptura tipocaptura)
        {
            return objCaptura.Update(id, tipocaptura);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objCaptura.Delete(id);
        }
    }
}
