using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLBalanzaController : ControllerBase
    {
        ETLBalanzaDataAccessLayer etlBalanza = new ETLBalanzaDataAccessLayer();
        ConfigCorreoController configCorreo = new ConfigCorreoController();
        // GET: api/ETLBalanza
        [HttpGet]
        public IEnumerable<string> Get()
        {
            iniciarETLBalanza(1);
            return new string[] { "value1", "value2" };
        }

        // GET: api/ETLBalanza/5
        [HttpGet("{id}", Name = "GetETLBalanza")]
        public string GetETLBalanza(int id)
        {
            
            return "value";
        }

        // POST: api/ETLBalanza
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ETLBalanza/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [HttpPost]
        public void iniciarETLBalanza(int idCompania) {
            List<Balanza> lstBala = new List<Balanza>();
            
            DateTime fechaInicioProceso = DateTime.Now;
            try
            {
    
                lstBala = etlBalanza.obtenerSalContCCD(idCompania);

                //Balanza bal1 = new Balanza();
                //bal1.cta = "1";
                //bal1.scta = "2";
                //bal1.sscta = "3";
                //bal1.abrabonos = 1;
                //bal1.abrcargos = 2;
                //bal1.acta = 999;
                //bal1.cc = "99";
                //bal1.year = 2050;

                //lstBala.Add(bal1);

                int cantRegAfectados=etlBalanza.generarBalanza(lstBala,idCompania);


                DateTime fechaFinalProceso = DateTime.Now;
                configCorreo.EnviarCorreo("La extracción de Balanza se genero correctamente"
                                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           , "ETL Balanza Manual");


            }
            catch (Exception ex)
            {

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                                           , "ETL Balanza Manual ");
                string error = ex.Message;
                throw;
            }

        }
    }
}
