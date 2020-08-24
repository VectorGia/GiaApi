using System;
using AppGia.Dao;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodoController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        PeriodoDataAccessLayer objperiodo = new PeriodoDataAccessLayer();

        // GET: api/Periodo
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(objperiodo.GetAllPeriodos());
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en GetAllPeriodos");
                return BadRequest(e.Message);
            }
        }

        // GET: api/Periodo/5
        [HttpGet("{id}", Name = "GetPeriodo")]
        public IActionResult Get(string id)
        {
            try
            {
                return Ok(objperiodo.GetPeriodoData(id));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en GetPeriodoData");
                return BadRequest(e.Message);
            }
        }

        // POST: api/Periodo
        [HttpPost]
        public IActionResult Post([FromBody] Periodo periodo)
        {
            try
            {
                objperiodo.validateNoDuplicatePeridodos(periodo);
                return Ok(objperiodo.AddPeriodo(periodo));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en AddPeriodo");
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Periodo/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Periodo periodo)
        {
            try
            {
                return Ok(objperiodo.updatePeriodo(id, periodo));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en updatePeriodo");
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                return Ok(objperiodo.deletePeriodo(id));
            }
            catch (Exception e)
            {
                logger.Error(e, "Error en deletePeriodo");
                return BadRequest(e.Message);
            }
        }
    }
}