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
       
        // GET: api/Empresa
        [HttpGet]
        public IEnumerable<Empresa> Get()
        {

            return objCompania.GetAllEmpresas();
        }

        // GET: api/Empresa/5
        [HttpGet("{id}", Name = "Get")]
        public Empresa Details(int id)
        {
            return objCompania.GetEmpresaData(id);
        }

        // POST: api/Empresa
        [HttpPost]
        public int Create([FromBody]Empresa compania)
        {
            return objCompania.Add(compania);
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