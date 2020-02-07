using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLBalanzaController : ControllerBase
    {
        ETLBalanzaDataAccessLayer etlBalanzaDa = new ETLBalanzaDataAccessLayer();
        ConfigCorreoController configCorreoDa = new ConfigCorreoController();
        ProcesoDataAccessLayer procesoDa = new ProcesoDataAccessLayer();

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        // GET: api/ETLBalanza
        [HttpGet]
        public IEnumerable<string> Get()
        {
           // iniciarETLBalanzaCSV(1);
            //iniciarETLBalanza(4);
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
        public void iniciarETLBalanza(Int64 idEmpresa) {
            List<Balanza> lstBala = new List<Balanza>();
            
            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();
            try
            {
    
                lstBala = etlBalanzaDa.obtenerSalContCCD(idEmpresa);

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

                int cantRegAfectados=etlBalanzaDa.generarBalanza(lstBala,idEmpresa);


                DateTime fechaFinalProceso = DateTime.Now;
                configCorreoDa.EnviarCorreo("La extracción de Balanza se genero correctamente"
                                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           , "ETL Balanza Manual");

                
                proceso.id_empresa = idEmpresa;
                proceso.tipo = "Manual";
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = "finalizado";
        
                procesoDa.AddProceso(proceso);

                etlBalanzaDa.UpdateCuentaUnificada(idEmpresa);// concatencacion de cuentas  



            }
            catch (Exception ex)
            {

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreoDa.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                                           , "ETL Balanza Manual ");
                string error = ex.Message;
                proceso.id_empresa = idEmpresa;
                proceso.tipo = "Manual";
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = "finalizado";
                proceso.mensaje = ex.Message;

                procesoDa.AddProceso(proceso);
                //etlBalanza.UpdateCuentaUnificada();// concatencacion de cuentas 
                throw;
            }

        }



        [HttpPost]
        public void iniciarETLBalanzaCSV(Int64 idEmpresa)
        {
            string archivo = string.Empty;
            var configRuta = GetConfiguration();
            string ruta = configRuta.GetSection("Paths").GetSection("CSVPathBalanza").Value;
            List<Balanza> lstBala = new List<Balanza>();

            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();
            try
            {
                archivo =  etlBalanzaDa.generarSalContCC_CSV(idEmpresa,ruta);
                //prueba
                //archivo = "PruebaBalanzaRecrotado.csv";

                int cantRegAfectados = etlBalanzaDa.copy_balanza(archivo, ruta);

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreoDa.EnviarCorreo("La extracción de Balanza se genero correctamente"
                                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           , "ETL Balanza Manual");


                proceso.id_empresa = idEmpresa;
                proceso.tipo = "Manual";
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = "finalizado";
                proceso.mensaje = "";

                procesoDa.AddProceso(proceso);

                etlBalanzaDa.UpdateCuentaUnificada(idEmpresa);// concatencacion de cuentas 


            }
            catch (Exception ex)
            {

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreoDa.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                                           , "ETL Balanza Manual ");
                string error = ex.Message;
                proceso.id_empresa = idEmpresa;
                proceso.tipo = "Manual";
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = "con error";
                proceso.mensaje = ex.Message;
                procesoDa.AddProceso(proceso);
                //etlBalanza.UpdateCuentaUnificada();// concatencacion de cuentas 
                throw;
            }

        }


    }
}
