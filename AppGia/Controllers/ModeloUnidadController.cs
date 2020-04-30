using System;
using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelounidadController : ControllerBase
    {
        private ModeloUnidadNegocioDataAccessLayer objModelo = new ModeloUnidadNegocioDataAccessLayer();

        // GET: api/ModeloUnidad
        [HttpGet]
        public IEnumerable<ModeloUnidadNegocio> Get()
        {
            return objModelo.findAll();
        }

        [HttpGet("{idModelo}/{idUnidad}")]
        public ModeloUnidadNegocio GetModelo(Int64 idModelo, Int64 idUnidad)
        {
            return objModelo.findByIdModeloAndIdUnidad(idModelo, idUnidad);
        }

        // GET: api/ModeloUnidad/5
        [HttpGet("{idModelo}")]
        public List<ModeloUnidadNegocio> GetModelo(Int64 idModelo)
        {
            return objModelo.findByIdModelo(idModelo);
        }

        // POST: api/ModeloUnidad
        [HttpPost]
        public int Create([FromBody] ModeloUnidadNegocio modeloUnidadNegocio)
        {
            return objModelo.Add(modeloUnidadNegocio);
        }


        [HttpDelete("{idModelo}/{idunidad}")]
        public int Delete(string id)
        {
            ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();

            return objModelo.delete(modeloUnidadNegocio);
        }
    }
}