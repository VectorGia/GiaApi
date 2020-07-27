using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
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
        [HttpGet("{id}", Name = "GetEmpresa")]
        public Empresa Details(int id)
        {
            return objCompania.GetEmpresaData(id);
        }

        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        // POST: api/Empresa
        [HttpPost]
        public IActionResult Create([FromBody] Empresa empresa)
        {
            if (empresa.contrasenia_etl.Length > 0)
            {
                empresa.contrasenia_etl=Base64Encode(empresa.contrasenia_etl);
            }

            try
            {
                return Ok(objCompania.Add(empresa));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //empresa.id = idEmpresaCurrent;
 
            /*if (empresa.contrasenia_etl.Length > 0)
            {
                objCompania.UpdateContrasenia(empresa);
            }*/
            
        }

        // PUT: api/Empresa/5
        [HttpPut("{id}")]
        public int Put(string id, [FromBody] Empresa empresa)
        {
            if (empresa.contrasenia_etl.Length > 0)
            {
                empresa.contrasenia_etl=Base64Encode(empresa.contrasenia_etl);
            }
            int cantPut = objCompania.Update(id, empresa); 
            /*if (empresa.contrasenia_etl.Length > 0)
            {
                empresa.id = Convert.ToInt64(id);
                objCompania.UpdateContrasenia(empresa);
            }*/
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