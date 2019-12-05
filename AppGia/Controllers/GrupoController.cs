using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : ControllerBase
    {
        GrupoDataAccessLayer objgrupo = new GrupoDataAccessLayer();

        // GET: api/Grupo
        [HttpGet]
        public IEnumerable<Grupo> Get()
        {
            return objgrupo.GetAllGrupos();
        }

        //// GET: api/Grupo/5
        //[HttpGet("{id}", Name = "")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Grupo
        [HttpPost]
        public int Create([FromBody]Grupo grup )
        {
            return objgrupo.addGrupo(grup);
        }

        //// PUT: api/Grupo/5
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
