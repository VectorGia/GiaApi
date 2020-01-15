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
    public class ETLProgController : ControllerBase
    {
        ETLProgDataAccessLayer objetl = new ETLProgDataAccessLayer();
        // GET: api/ETLProg
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ETLProg/5
        [HttpGet("{id}", Name = "GetETL")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ETLProg
        [HttpPost]
        public int Post([FromBody] ETLProg etl)
        {
            return objetl.AddEtlprog(etl);
        }

        // PUT: api/ETLProg/5
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
