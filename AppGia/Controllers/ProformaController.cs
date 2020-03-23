using System;
using System.Collections.Generic;
using AppGia.Dao;
using AppGia.Helpers;
using AppGia.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProformaController : ControllerBase
    {
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
        public List<ProformaDetalle> Post([FromBody] Proforma proforma)
        {
            return objProforma.manageBuildProforma(proforma.centro_costo_id, proforma.anio, proforma.tipo_proforma_id, proforma.tipo_captura_id);
        }
        [HttpPost("ajustes")]
        public List<ProformaDetalle> GetProforma([FromBody] Proforma proforma)
        {
            return new ProformaHelper().getAjustes(proforma.centro_costo_id, proforma.anio,proforma.tipo_captura_id);
        }
        [HttpPost("tipoCambio")]
        public Dictionary<string, double> getFactoresTipoCambioGia([FromBody] Proforma proforma)
        {
            return new TipoCambioHelper().getTiposCambio(proforma.centro_costo_id, proforma.anio,proforma.tipo_captura_id);
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
            return objProforma.GuardaProforma(lstGuardaProforma);
        }
        
        
        
        // PUT: api/Proforma/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] List<ProformaDetalle> proformaDetalles)
        {
            return objProforma.ActualizaProforma(proformaDetalles);
        }

        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objProforma.Delete(id);
        }

    }
}
