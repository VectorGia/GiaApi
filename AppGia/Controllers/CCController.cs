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
    public class CCController : ControllerBase
    {
        CentroCostosDataAccessLayer objcc = new CentroCostosDataAccessLayer();
        // GET: api/CC
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CC/5
        [HttpGet("{id}", Name = "GetC")]
        public CentroCostos GetC(int id)
        {
            return objcc.GetCentro(id);
        }

        // POST: api/CC
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/CC/5
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
