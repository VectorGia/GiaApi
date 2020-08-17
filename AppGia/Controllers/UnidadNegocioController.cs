using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
using Microsoft.Extensions.Logging;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadNegocioController : ControllerBase
    {
        private readonly ILogger<UnidadNegocioController> _logger;

        public UnidadNegocioController(ILogger<UnidadNegocioController> logger)
        {
            _logger = logger;
        }
        UnidadNegocioDataAccessLayer objUnidadNeg = new UnidadNegocioDataAccessLayer();
        // GET: api/UnidadNegocio
        [HttpGet]
        public List<UnidadNegocio> Get()
        {
            
            return objUnidadNeg.GetAllUnidadNegocio();
        }

        // GET: api/UnidadNegocio/5
        [HttpGet("{id}", Name = "GetUnidadNegocio")]
        public UnidadNegocio GetUnidadNegocio(int idUnidadNegocio)
        {
            return objUnidadNeg.GetUnidadNegocio(idUnidadNegocio);
        }
        
        [HttpGet("withModel")]
        public List<UnidadNegocio> GetUnidadesConModelo()
        {
            return objUnidadNeg.GetAllUnidadNegocioWithModelo();
        }
        [HttpGet("unidadCC")]
        public List<Dictionary<string,Int64>> GetUnidadesRelCC()
        {
            return objUnidadNeg.GetUnidadCC();
        }

        // POST: api/UnidadNegocio
        [HttpPost]
        public int Post([FromBody] UnidadNegocio unidadNegocio)
        {
            return objUnidadNeg.UpdateUnidadNegocio(unidadNegocio);
        }

        // PUT: api/UnidadNegocio/5
        [HttpPut("{id}")]
        public int Put([FromBody] UnidadNegocio unidadNegocio)
        {
            return objUnidadNeg.AddUnidadNegocio(unidadNegocio);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
