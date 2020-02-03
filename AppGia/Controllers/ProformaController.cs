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
        ProformaDataAccessLayer proforma = new ProformaDataAccessLayer();
        ProformaDetalleDataAccessLayer proforma_detalle = new ProformaDetalleDataAccessLayer();
        // GET: api/Proforma
        [HttpGet]
        public IEnumerable<Proforma> Get(int idProforma)
        {
            int idCentroCosto = 1;
            int idEmpresa = 4;
            int mes = 1;
            int idModeloNegocio = 20;
            int idProyecto = 51;
            int idRubro = 7;
            int anio = 2020;
            int idTipoCaptura = 1;
            ObtieneProfCalc(idCentroCosto, mes, idEmpresa, idModeloNegocio, idProyecto, idRubro, anio, idTipoCaptura);
            return proforma.GetProforma(idProforma);
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

        // PUT: api/Proforma/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public void ObtieneProfCalc(int idCentroCosto, int mes, int idEmpresa, int idModeloNegocio, int idProyecto, int idRubro, int anio, int idTipoCaptura)
        {
            ProformaDetalle listaProf = new ProformaDetalle();
            proforma_detalle.GetProformaCalculada(idCentroCosto, mes, idEmpresa, idModeloNegocio, idProyecto, idRubro, anio, idTipoCaptura);
        }
    }
}
