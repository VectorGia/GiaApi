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
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Compania
        [HttpPost]
        public int Create([FromBody]Compania comp)
        {
            return objCompania.addCompania(comp);
        }

        //// PUT: api/Compania/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
