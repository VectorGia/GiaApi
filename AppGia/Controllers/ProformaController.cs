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
    public class ProformaController : ControllerBase
    {
        ProformaDataAccessLayer objProforma = new ProformaDataAccessLayer();
        ProformaDetalleDataAccessLayer objProformaDetalle = new ProformaDetalleDataAccessLayer();
        // GET: api/Proforma
        [HttpGet]
        public List<ProformaDetalle> Get(int idProforma)
        {
            ProformaDataAccessLayer objProforma = new ProformaDataAccessLayer();
            int idCentroCosto = 12;
            int anio = 2020;
            int idTipoProforma = 1;
            //int idEmpresa = 4;
            //int mes = 1;
            //int idModeloNegocio = 20;
            //int idProyecto = 51;
            //int idRubro = 7;
            //idProforma = 6;
            //bool activo = true;

            // Ya funciona el select de la proforma con el enumerable Proforma
            // Ya funciona el select de la proforma calculada con el enumarable ProformaDetalle
            //return ObtieneProfCalc(idCentroCosto, mes, idEmpresa, idModeloNegocio, idProyecto, idRubro, anio, idTipoCaptura);
            // Ya funciona el update de la proforma (solo el campo activo)
            //objProforma.UpdateProforma(5, activo, 1);
            //Proforma insertProforma = new Proforma();
            //insertProforma.modelo_negocio_id = 21;
            //insertProforma.tipo_captura_id = 1;
            //insertProforma.tipo_proforma_id = 2;
            //insertProforma.centro_costo_id = 5;
            //insertProforma.activo = true;
            //insertProforma.usuario = 1;
            //insertProforma.fecha_captura = DateTime.Now;
            //objProforma.AddProforma(insertProforma);
            return objProforma.GeneraProforma(idCentroCosto, anio, idTipoProforma);
        }

        // GET: api/Proforma/5
        [HttpGet("{id}", Name = "GetProforma")]
        public string GetProforma(int id)
        {
            return "value";
        }

        // POST: api/Proforma
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        public int Create([FromBody]Proforma proforma)
        {
            return objProforma.AddProforma(proforma);
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

        public IEnumerable<ProformaDetalle> ObtieneProfCalc(int mes, int idEmpresa, int idModeloNegocio, int idProyecto, int anio, int idTipoCaptura)
        {
            //ProformaDetalle listaProf = new ProformaDetalle();
            return objProformaDetalle.GetProformaCalculada(mes, idEmpresa, idModeloNegocio, idProyecto, anio, idTipoCaptura);
        }
    }
}
