﻿using System;
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
    public class BalanzaController : ControllerBase

    {
        BalanzaDataAccessLayer objBalanza = new BalanzaDataAccessLayer();
        LogETLDataAccessLayer objlogetl = new LogETLDataAccessLayer();
        // GET: api/Balanza
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Balanza/5
        [HttpGet("{id}", Name = "GetBalanza")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Balanza
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST: api/Centro
        [HttpPost]
        public int Create([FromBody] Balanza balanza)
        {
            
            return objBalanza.AddBalanza(balanza);
        }

        // PUT: api/Balanza/5
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
