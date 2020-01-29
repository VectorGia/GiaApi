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
    public class EmpresaController : ControllerBase
    {
        EmpresaDataAccessLayer objCompania = new EmpresaDataAccessLayer();
        BalanzaDataAccessLayer objBalanza = new BalanzaDataAccessLayer();
        // GET: api/Empresa
        [HttpGet]
        public IEnumerable<Empresa> Get()
        {
            Balanza bal = new Balanza();
            bal.TEXT_CTA = "00000012";
            bal.TEXT_SCTA = "00000013";
            bal.TEXT_SSCTA = "00000014";
            bal.TEXT_DESCRIPCION = "Desc 1";
            bal.TEXT_DESCRIPCION2 = "Desc 2 ";
            // objBalanza.AddBalanza(bal);
            return objCompania.GetAllEmpresas();
        }

        // GET: api/Empresa/5
        [HttpGet("{id}", Name = "Get")]
        public Empresa Details(string id)
        {
            return objCompania.GetEmpresaData(id);
        }

        // POST: api/Empresa
        [HttpPost]
        public int Create([FromBody]Empresa compania)
        {
            return objCompania.AddCompania(compania);
        }

        // PUT: api/Empresa/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Empresa compania)
        {
            return objCompania.Update(id, compania);

            // return objCompania.Update(id, comp);

        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public int Delete(string id)
        {
            return objCompania.Delete(id);
        }
    }
}