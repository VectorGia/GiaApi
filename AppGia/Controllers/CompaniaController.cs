using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;


namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniaController : ControllerBase
    {
        CompaniaDataAccessLayer objCompania = new CompaniaDataAccessLayer();

        // GET: api/Compania
        [HttpGet]
        public IEnumerable<Compania> Get()
        {
            return objCompania.GetAllCompanias();
        }


        // GET: api/Compania/5
        [HttpGet("{id}", Name = "Get")]
        public Compania Details(string id)
        {
            return objCompania.GetCompaniaData(id);
        }

        // POST: api/Compania
        [HttpPost]
        public int Create([FromBody]Compania compania)
        {
            return objCompania.AddCompania(compania);
        }

        // PUT: api/Compania/5
        [HttpPut("{id}")]
        public int Put(string id,[FromBody] Compania compania)
        {
           return objCompania.Update(id, compania);

          // return objCompania.Update(id, comp);

        }

        //DELETE: api/ApiWithActions/5
   
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objCompania.Delete(id);
        }
    }
}
