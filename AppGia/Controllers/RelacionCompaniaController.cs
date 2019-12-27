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
    public class RelacionCompaniaController : ControllerBase
    {
        RelacionCompaniaDataAccessLayer objRelacion = new RelacionCompaniaDataAccessLayer();
        // GET: api/RelacionCompania
        [HttpGet]
        public IEnumerable<RelacionCompania> Get()
        {
            return objRelacion.GetAllRelacionesCompania();
        }

        // GET: api/RelacionCompania/5
        [HttpGet("{id}", Name = "GetRelacionCompania")]
        public RelacionCompania GetRelacionCompania(int id)
        {
            return objRelacion.GetRelacionCompaniaData(id);
        }

        // POST: api/RelacionCompania
        [HttpPost]
        public int Post([FromBody] RelacionCompania relacionCompania)
        {
            return objRelacion.insert(relacionCompania);
        }

        // PUT: api/RelacionCompania/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] RelacionCompania relacionCompania)
        {
            return objRelacion.update(id,relacionCompania);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(RelacionCompania relacionCompania)
        {
            return objRelacion.delete (relacionCompania);
        }
    }
}
