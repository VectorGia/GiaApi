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
    public class ModeloNegocioController : ControllerBase
    {
        ModeloNegocioDataAccessLayer objModelo = new ModeloNegocioDataAccessLayer();
        // GET: api/ModeloNegocio
        [HttpGet]
        public IEnumerable<Modelo_Negocio> Get()
        {
            return objModelo.GetAllModeloNegocios();
        }

        // GET: api/ModeloNegocio/5
        [HttpGet("{id}", Name = "GetModelo")]
        public Modelo_Negocio GetModelo(string id)
        {
            return objModelo.GetModelo(id);
        }

        // POST: api/ModeloNegocio
        [HttpPost]
        public int Create([FromBody] Modelo_Negocio negocio)
        {
            return objModelo.addModeloNegocio(negocio);
        }

        // PUT: api/ModeloNegocio/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Modelo_Negocio negocio)
        {
            return objModelo.Update(id, negocio);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objModelo.Delete(id);
        }
    }
}
