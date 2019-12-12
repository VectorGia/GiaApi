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
        public IEnumerable<ModeloNegocio> Get()
        {
            return objModelo.GetAllModeloNegocios();
        }

        //// GET: api/ModeloNegocio/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/ModeloNegocio
        [HttpPost]
        public int Create([FromBody] ModeloNegocio negocio)
        {
            return objModelo.addModelo(negocio);
        }

        // PUT: api/ModeloNegocio/5
        [HttpPut("{id}")]
        public int Put([FromBody] ModeloNegocio negocio)
        {
            return objModelo.UpdateModelo(negocio);
        }

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
