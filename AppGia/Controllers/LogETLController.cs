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
    public class LogETLController : ControllerBase
    {
        LogETLDataAccessLayer objlogetl = new LogETLDataAccessLayer();
        // GET: api/LogETL
        [HttpGet]
        public IEnumerable<LogEtl> Get()
        {
            return objlogetl.GetAllLogETL();
        }

        // GET: api/LogETL/5
        [HttpGet("{id}", Name = "GetLogETL")]
        public LogEtl GetLogEtl()
        {
            return objlogetl.GetLogETLData();
        }

        // POST: api/LogETL
        [HttpPost]
        public void Post([FromBody] LogEtl logetl)
        {
            objlogetl.insert(logetl);
        }

        // PUT: api/LogETL/5
        [HttpPut("{id}")]
        public int Put(int id, string campo, int valor)
        {
            return objlogetl.update(campo,valor,id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
