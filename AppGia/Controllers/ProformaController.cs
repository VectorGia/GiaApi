﻿using System;
using System.Collections.Generic;
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

        public List<Proforma> GetAllProformas()
        {
            return objProforma.GetAllProformas();
        }

        public List<Proforma> GetProformaPorId(int idProforma)
        {
            return objProforma.GetProforma(idProforma);
        }

        // GET: api/Proforma/5
        [HttpGet("{id}", Name = "GetProforma")]
        public string GetProforma(int id)
        {
            return "value";
        }

        // POST: api/Proforma
        [HttpPost]
        public List<ProformaDetalle> Post([FromBody] Proforma proforma)
        {
            
            return objProforma.manageBuildProforma(proforma.centro_costo_id, proforma.anio, proforma.tipo_proforma_id, proforma.tipo_captura_id);
        }
        [HttpGet("ajustes")]
        public List<ProformaDetalle> GetProforma(Int64 idCC,int anio,Int64 idTipoCaptura)
        {
            return new ProformaHelper().getAjustes(idCC, anio,idTipoCaptura);
        }

        public int Create([FromBody]List<ProformaDetalle> lstGuardaProforma)
        {
            return objProforma.GuardaProforma(lstGuardaProforma);
        }

        // PUT: api/Proforma/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] Proforma proforma)
        {
            return objProforma.AddProforma(proforma);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public IEnumerable<ProformaDetalle> ObtieneProfCalc(Int64 idCenCos, int mes, int idEmpresa, int idModeloNegocio, int idProyecto, int anio, int idTipoCaptura)
        {
            //ProformaDetalle listaProf = new ProformaDetalle();
            return objProformaDetalle.GetProformaCalculada(idCenCos, mes, idEmpresa, idModeloNegocio, idProyecto, anio, idTipoCaptura);
        }
    }
}
