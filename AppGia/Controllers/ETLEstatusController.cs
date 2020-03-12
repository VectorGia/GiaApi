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
    public class ETLEstatusController : ControllerBase
    {
        ETLEstatusDataAccessLayer objestatusetl = new ETLEstatusDataAccessLayer();
        // GET: api/EstatusETL
        [HttpGet]
        public IEnumerable<Etl_Estatus> Get()
        {
            return objestatusetl.GetAllEstatusETL();
        }

        // GET: api/EstatusETL/5
        [HttpGet("{id}", Name = "GetEstatusETL")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/EstatusETL
        [HttpPost]
        public int Post([FromBody] Etl_Estatus estatusetl)
        {
            return objestatusetl.insert(estatusetl);
        }

        // PUT: api/EstatusETL/5
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
