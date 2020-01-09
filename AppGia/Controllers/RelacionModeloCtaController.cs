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
    public class RelacionModeloCtaController : ControllerBase
    {
        RelacionModeloCuentaDataAccesLayer objModelo = new RelacionModeloCuentaDataAccesLayer();
        // GET: api/RelacionModeloCta
        [HttpGet]
        public IEnumerable<RelacionModeloCta> Get()
        {
            return objModelo.GetAllRelacionModeloCta();
        }

        // GET: api/RelacionModeloCta/5
        [HttpGet("{id}", Name = "GetModCta")]
        public RelacionModeloCta GetRelacionModeloCta(int id)
        {
            return objModelo.GetRelacionModeloCta(id);
        }

        // POST: api/RelacionModeloCta
        [HttpPost]
        public int Post([FromBody] RelacionModeloCta relacion)
        {
            return objModelo.insertarModCta(relacion);
        }

        // PUT: api/RelacionModeloCta/5
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
