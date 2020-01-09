using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionModeloCuentaController : ControllerBase
    {
        RelacionModeloCuentaDataAccesLayer objModelo = new RelacionModeloCuentaDataAccesLayer();
        // GET: api/RelacionModeloCuenta
        [HttpGet]
        public IEnumerable<RelacionModeloCta> Get()
        {
            return objModelo.GetAllRelacionModeloCta();
        }

        // GET: api/RelacionModeloCuenta/5
        [HttpGet("{id}", Name = "GetRelacionModeloCta")]
        public RelacionModeloCta GetRelacionModeloCta(int id)
        {
            return objModelo.GetRelacionModeloCta(id);
        }

        // POST: api/RelacionModeloCuenta
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/RelacionModeloCuenta/5
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
