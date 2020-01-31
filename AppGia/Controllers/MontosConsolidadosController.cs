﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MontosConsolidadosController : ControllerBase
    {
        // GET: api/MontosConsolidados
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MontosConsolidados/5
        [HttpGet("{id}", Name = "GetMontosConsolidados")]
        public string GetMontosConsolidados(int id)
        {
            return "value";
        }

        // POST: api/MontosConsolidados
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/MontosConsolidados/5
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