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
    public class TipoProformaController : ControllerBase
    {
        TipoProformaDataAccessLayer objtipoproforma = new TipoProformaDataAccessLayer();
        // GET: api/TipoProforma
        [HttpGet]
        public IEnumerable<Tipo_Proforma> Get()
        {
            return objtipoproforma.GetAllTipoProformas();
        }

        // GET: api/TipoProforma/5
        [HttpGet("{id}", Name = "GetProformar")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TipoProforma
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TipoProforma/5
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
