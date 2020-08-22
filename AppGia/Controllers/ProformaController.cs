using System;
using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Helpers;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        ProformaDataAccessLayer objProforma = new ProformaDataAccessLayer();
        ProformaDetalleDataAccessLayer objProformaDetalle = new ProformaDetalleDataAccessLayer();
        // GET: api/Proforma
        [HttpGet]
        public List<Proforma> Get()
        {
            return objProforma.GetAllProformas(); // objProformaDetalle.GetProformaCalculada(17, 3, 89, 29, 96, 2019, 1);
        }
        
        [HttpGet("{idProforma}")]
        public List<ProformaDetalle> GetProformaDetalle(Int64 idProforma)
        {
            return objProformaDetalle.GetProformaDetalle(idProforma);
        }
        
        // POST: api/Proforma
        [HttpPost]
        public IActionResult Post([FromBody] Proforma proforma)
        {
            try
            {
                List<ProformaDetalle> proformaDetalles =
                    objProforma.manageBuildProforma(proforma.centro_costo_id, proforma.anio,
                        proforma.tipo_proforma_id, proforma.tipo_captura_id);
                proformaDetalles.ForEach(detalle =>
                {
                    detalle.unidad_id = proforma.unidad_id;
                    if (detalle.empresa_id == 0)
                    {
                        detalle.empresa_id = proforma.empresa_id;
                    }
                });
                return Ok(proformaDetalles);
            }
            catch (Exception e)
            {
                logger.Error(e,"Error en proformado");
                return BadRequest(e.Message);
            }
        }
        [HttpPost("ajustes")]
        public List<ProformaDetalle> GetProforma([FromBody] Proforma proforma)
        {
            try
            {
                return new ProformaHelper().getAjustes(proforma.centro_costo_id, proforma.anio,
                    proforma.tipo_captura_id);
            }
            catch (Exception e)
            {
                logger.Error(e,"error en ajustes");
                throw e;
            }
        }
        [HttpPost("tipoCambio")]
        public Dictionary<string, double> getFactoresTipoCambioGia([FromBody] Proforma proforma)
        {
            try
            {
                return new TipoCambioHelper().getTiposCambio(proforma.centro_costo_id, proforma.anio,
                    proforma.tipo_captura_id);
            }
            catch (Exception e)
            {
                logger.Error(e,"error en tipoCambio");
                throw e;
            }
        }
        [HttpGet("anios")]
        public List<int> getAnios()
        {
            List<int> anios=new List<int>();
            int anioActual = DateTime.Now.Year;
            anios.Add(anioActual);
            for (int i = 0; i < 10; i++)
            {
                anios.Add(++anioActual);
            }
                
            return anios;
        }

  
        [HttpPost("save")]
        public int GuardaProforma([FromBody]List<ProformaDetalle> lstGuardaProforma)
        {
            try
            {
                return objProforma.GuardaProforma(lstGuardaProforma);
            }
            catch (Exception e)
            {
                logger.Error(e,"error en gusradar proforma");
                throw e;
            }
        }
        
        
        
        // PUT: api/Proforma/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] List<ProformaDetalle> proformaDetalles)
        {
            try
            {
                return objProforma.ActualizaProforma(proformaDetalles);

            }
            catch (Exception e)
            {
                logger.Error(e, "error en actualizar proforma");
                throw e;
            }
        }

        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objProforma.Delete(id);
        }

    }
}
