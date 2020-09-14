using System;
using System.Collections.Generic;
using AppGia.Dao;
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

        // GET: api/Grupo/5
        [HttpGet("{id}", Name = "GetGrupo")]
        public Grupo Get(string id)
        {
            return objgrupo.GetGrupo(id);
        }

        // POST: api/Grupo
        [HttpPost]
        public int Create([FromBody]Grupo grup )
        {
            return objgrupo.addGrupo(grup);
        }

        // PUT: api/Grupo/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Grupo grupo)
        {
            return objgrupo.UpdateGrupo(id, grupo);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objgrupo.DeleteGrupo(id);
        }
    }
}
