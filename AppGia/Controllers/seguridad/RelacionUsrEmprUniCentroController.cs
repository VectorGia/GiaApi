using System;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelacionUsrEmprUniCentroController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        RelacionUsrEmprUniCentroDataAccessLayer accessLayer = new RelacionUsrEmprUniCentroDataAccessLayer();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(accessLayer.findAll());
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en consulta de relaciones");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRelacion(Int32 id)
        {
            try
            {
                return Ok(accessLayer.findById(id));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en consulta de relacion");
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] RelacionUsrEmprUniCentro relacionUsrEmprUniCentro)
        {
            try
            {
                return Ok(accessLayer.add(relacionUsrEmprUniCentro));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en guardado de relacion");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] RelacionUsrEmprUniCentro value)
        {
            try
            {
                value.id = id;
                return Ok(accessLayer.update(value));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en actualizacion de relacion");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                return Ok(accessLayer.delete(id));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error borrado de relacion");
                return BadRequest(e.Message);
            }
        }
    }
}