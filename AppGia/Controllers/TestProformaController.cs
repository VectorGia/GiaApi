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
    public class TestProformaController : ControllerBase
    {
        TestProformaDataAccessLayer objtest = new TestProformaDataAccessLayer();
        // GET: api/TestProforma
        [HttpGet]
        public List<ProformaDetalle> GetAllUsuarios()
        {
            return objtest.GetAllUsuarios();
        }

        // GET: api/TestProforma/5
        [HttpGet("{id}", Name = "GetTestProforma")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TestProforma
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TestProforma/5
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
