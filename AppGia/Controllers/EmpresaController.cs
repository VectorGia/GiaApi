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
            //Empresa empresa = new Empresa();
            //empresa.abrev = "borrelo";
            //empresa.activo = true;
            //empresa.bd_name = "dba";
            //empresa.contrasenia_etl = "etlCONTRA";
            //empresa.desc_id = "PRUEBACONTRA";
            //empresa.estatus = true;
            //empresa.etl = true;
            //empresa.fec_modif = DateTime.Now;
            //empresa.host = "localhost";
            //empresa.id_centro_costo = 1;
            //empresa.id_modelo_neg = 1;
            //empresa.moneda_id = 1;
            //empresa.nombre = "untouchble";
            //empresa.puerto_compania = 202;
            //empresa.usuario_etl = "dba";

            ////long idEmpresaCurrent = Create(empresa);

            //empresa.contrasenia_etl = "oooooo";
            //Put("80",empresa);
            
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
        public long Create([FromBody]Empresa empresa)
        {
            long idEmpresaCurrent = objCompania.Add(empresa);
            empresa.id = idEmpresaCurrent;
            
            /// encriptandoContraseña 
            if (empresa.contrasenia_etl.Length > 0)
            {
                objCompania.UpdateContrasenia(empresa);
            }
              return idEmpresaCurrent;
           // return objCompania.Add(compania);
        }

        // PUT: api/Empresa/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Empresa empresa)
        {
            int cantPut = objCompania.Update(id, empresa);
            if (empresa.contrasenia_etl.Length > 0)
            {
                empresa.id = Convert.ToInt64(id);
                objCompania.UpdateContrasenia(empresa);
            }
            return cantPut;

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