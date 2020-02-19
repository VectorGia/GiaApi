using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using AppGia.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ETLMovPolizaSemanalController : ControllerBase
    {
        ConfigCorreoController configCorreoDa = new ConfigCorreoController();
        ProcesoDataAccessLayer procesoDa = new ProcesoDataAccessLayer();
        ETLMovPolizaSemanalDataAccessLayer etlMovSemanal = new ETLMovPolizaSemanalDataAccessLayer();
        // GET: api/ETLMovimientoPolizaSemanal

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            inicarEtlMovPolizaCSV(4);
           // etlMovSemanal.insertarReporteSemanalPg(4,"2005","2006");

            return new string[] { "value1", "value2" };
        }

        // GET: api/ETLMovimientoPolizaSemanal/5
        [HttpGet("{id}", Name = "GetMovPolSemanal")]
        public string GetMovPolSemanal(int id)
        {
            return "value";
        }

        // POST: api/ETLMovimientoPolizaSemanal
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ETLMovimientoPolizaSemanal/5
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
        public void inicarEtlMovPolizaCSV(Int64 idEmpresa) {

            string archivo = string.Empty;
            string anio = string.Empty;
            var configRuta = GetConfiguration();
            string ruta = configRuta.GetSection("Paths").GetSection("CSVPathSemanal").Value;
            List<Semanal> lstSemanal = new List<Semanal>();

            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();

            try {

                //archivo = etlMovSemanal.generarScMOV_CSV(idEmpresa,ruta,anio);
                //Prueba 
                archivo = "PruebaSemanal.csv";

                int cantRegAfectados = etlMovSemanal.copy_semanal(archivo, ruta);

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreoDa.EnviarCorreo("La extracción de Movimientos de Polizas Semanal se genero correctamente"
                                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           , "ETL Movimiento de Polizas Semanal Manual ");


                proceso.id_empresa = idEmpresa;
                proceso.tipo = Constantes.TIPO_EXT_MANUAL;
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = Constantes.EST_EXT_FIN;
                proceso.modulo = Constantes.MODULO_SEMANAL;
                proceso.id_etl_prog = 0;
                proceso.mensaje = "";

                procesoDa.AddProceso(proceso);

            }

            catch (Exception ex) {
                DateTime fechaFinalProceso = DateTime.Now;
                configCorreoDa.EnviarCorreo("Ha ocurrido un error en la extracción de Movimientos de Polizas Semanal"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                                           , "ETL Movimiento de Polizas Semanal Manual ");
                string error = ex.Message;
                proceso.id_empresa = idEmpresa;
                proceso.tipo = Constantes.TIPO_EXT_MANUAL;
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = Constantes.EST_EXT_ERR;
                proceso.mensaje = ex.Message;
                proceso.modulo = Constantes.MODULO_SEMANAL;
                proceso.id_etl_prog = 0;
                procesoDa.AddProceso(proceso);

                throw;
            }
           

        }
    }
}
