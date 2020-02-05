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
    public class ProformaDetalleController : ControllerBase
    {
        ProformaDetalleDataAccessLayer ObjProformaDetalle = new ProformaDetalleDataAccessLayer();
        // GET: api/ProformaDetalle
        [HttpGet]
        public IEnumerable<ProformaDetalle> Get(int idProformaDetalle)
        {
            int idCentroCosto = 1;
            int idEmpresa = 4;
            int mes = 1;
            int idModeloNegocio = 20;
            int idProyecto = 51;
            int idRubro = 7;
            int anio = 2020;
            int idTipoCaptura = 1;
            //return ObjProformaDetalle.GetProformaDetalle(idProformaDetalle);
            return ObjProformaDetalle.GetProformaCalculada(idCentroCosto, mes, idEmpresa, idModeloNegocio, idProyecto, idRubro, anio, idTipoCaptura);
        }

        // GET: api/ProformaDetalle/5
        [HttpGet("{id}", Name = "GetProformaDetalle")]
        public string GetProformaDetalle(int idProformaDetalle)
        {
            return "value"; //proforma_detalle.GetProformaDetalle(idProformaDetalle);
        }

        // POST: api/ProformaDetalle
        [HttpPost]
        public void Post([FromBody] ProformaDetalle proformaDetalle)
        {
            //return proforma_detalle.AddProformaDetalle(proformaDetalle);
        }

        // PUT: api/ProformaDetalle/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
