using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Transactions;
using AppGia.Controllers;
using AppGia.Dao;
using AppGia.Dao.Etl;
using AppGia.Models;
using AppGia.Util;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Fluent;
using static System.Convert;
using Proceso = AppGia.Models.Etl.Proceso;
using ProcesoDataAccessLayer = AppGia.Dao.Etl.ProcesoDataAccessLayer;

namespace AppGia.Helpers
{
    public class ETLHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ConfiguracionCorreoDataAccessLayer _configCorreo = new ConfiguracionCorreoDataAccessLayer();
        private ProcesoDataAccessLayer _procesoDataAccessLayer = new ProcesoDataAccessLayer();
        private EmpresaDataAccessLayer _empresaDataAccessLayer = new EmpresaDataAccessLayer();
        private QueryExecuter _queryExecuter = new QueryExecuter();


        public void extraeBalanzaAuto()
        {
            logger.Info("inicio de extraeBalanzaAuto");
            String consultaExistenRegs = "select count(1) as numRegs from balanza";
            bool existenRegs = ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (!existenRegs)
            {
                extraeBalanza(-1, -1);
            }
            else
            {
                int anioActual = new DateTime().Year;
                extraeBalanza(anioActual, anioActual);
            }
        }

        public void extraeBalanza(int anioInicio, int anioFin)
        {
            logger.Info("incio extraeBalanza (anioInicio={0}, anioFin={1}))", anioInicio, anioFin);
            StopWatch sw =
                new StopWatch(String.Format("extraeBalanza (anioInicio={0}, anioFin={1})", anioInicio, anioFin));
            sw.start("GetAllEmpresas ");
            List<Empresa> empresas = _empresaDataAccessLayer.GetAllEmpresas();
            sw.stop();
            foreach (Empresa empresa in empresas)
            {
                Int64 idEmpresa = empresa.id;
                sw.start("cargaBalanzaEmpresa " + idEmpresa);
                try
                {
                    logger.Info("cargaBalanzaEmpresa(idEmpresa={0}, anioInicio={1}, anioFin={2})", idEmpresa,
                        anioInicio, anioFin);
                    cargaBalanzaEmpresa(idEmpresa, anioInicio, anioFin);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error en ejecucion de extraeBalanza");
                    _configCorreo.EnviarCorreo(
                        "Estimado Usuario : \n\n  La extracción(balanza) correspondiente a la compania " + idEmpresa +
                        "." +
                        empresa.nombre + " se genero incorrectamente \n\n Mensaje de Error: \n " + ex,
                        "ETL Extracción Balanza");
                }

                sw.stop();

                logger.Info(sw.prettyPrint());
            }
        }

        public void extraeFlujoAuto()
        {
            logger.Info("inicio de extraeFlujoAuto");
            String consultaExistenRegs = "select count(1) as numRegs from semanal";
            bool existenRegs = ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (!existenRegs)
            {
                extraeFlujo(-1, -1, -1);
            }
            else
            {
                int anioActual = new DateTime().Year;
                int mes = new DateTime().Month;
                extraeFlujo(anioActual, anioActual, mes);
            }
        }

        public void extraeFlujo(int anioInicio, int anioFin, int mes)
        {
            logger.Info("extraeFlujo(anioInicio={0}, anioFin={1}, mes={2})", anioInicio, anioFin, mes);
            StopWatch sw = new StopWatch(String.Format("extraeFlujo(anioInicio={0}, anioFin={1}, mes={2})", anioInicio,
                anioFin, mes));
            List<Empresa> empresas = _empresaDataAccessLayer.GetAllEmpresas();


            Int64 idEmpresa;

            foreach (Empresa empresa in empresas)
            {
                idEmpresa = empresa.id;

                try
                {
                    sw.start("cargaFlujoEmpresa " + idEmpresa);
                    cargaFlujoEmpresa(idEmpresa, anioInicio, anioFin, mes);
                    sw.stop();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error en ejecucion de extraeFlujo");
                    _configCorreo.EnviarCorreo(
                        "Estimado Usuario : \n\n  La extracción(Flujo) correspondiente a la compania " + idEmpresa +
                        "." +
                        empresa.nombre + " se genero incorrectamente \n\n Mensaje de Error: \n " + ex,
                        "ETL Extracción Balanza");
                }
            }

            logger.Info(sw.prettyPrint());
        }

        private void cargaBalanzaEmpresa(Int64 idEmpresa, int anioInicio, int anioFin)
        {
            StopWatch sw = new StopWatch("cargaBalanzaEmpresa " + idEmpresa);
            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();
            try
            {
                executeExtraccion(String.Format("{0} {1} {2}", idEmpresa, anioInicio, anioFin));

                sw.start("envio correo");
                DateTime fechaFinalProceso = DateTime.Now;

                _configCorreo.EnviarCorreo("La extracción de Balanza se genero correctamente"
                                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " +
                                           fechaFinalProceso
                                           + "\nTiempo de ejecucion : " +
                                           (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                    , Constantes.MSJ_CORREO_ETL_BALANZA);

                proceso.id_empresa = idEmpresa;
                proceso.tipo = Constantes.TIPO_EXT_PROGRAMADA;
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = Constantes.EST_EXT_FIN;
                proceso.mensaje = "";
                sw.stop();
                sw.start("AddProceso");

                _procesoDataAccessLayer.AddProceso(proceso);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en ejecucion de cargaBalanzaEmpresa");
                DateTime fechaFinalProceso = DateTime.Now;
                _configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " +
                                           fechaFinalProceso
                                           + "\nTiempo de ejecucion : " +
                                           (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                    , Constantes.MSJ_CORREO_ETL_BALANZA);
                proceso.id_empresa = idEmpresa;
                proceso.tipo = Constantes.TIPO_EXT_PROGRAMADA;
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = Constantes.EST_EXT_ERR;
                proceso.mensaje = ex.Message;
                _procesoDataAccessLayer.AddProceso(proceso);
                throw;
            }

            logger.Info(sw.prettyPrint());
        }


        private void cargaFlujoEmpresa(Int64 idEmpresa, int anioInicio, int anioFin, int mes)
        {
            StopWatch sw = new StopWatch(String.Format(
                "cargaFlujoEmpresa (idEmpresa={0},  anioInicio={1},  anioFin={2},  mes={3})", idEmpresa, anioInicio,
                anioFin, mes));
            logger.Info("cargaFlujoEmpresa( idEmpresa={0},  anioInicio={1},  anioFin={2},  mes={3})", idEmpresa,
                anioInicio, anioFin, mes);
            ETLMovPolizaSemanalDataAccessLayer etlMovSemanal = new ETLMovPolizaSemanalDataAccessLayer();
            string ruta = Constantes.CSV_PATH_SEMANAL;

            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();

            try
            {
                executeExtraccion(String.Format("{0} {1} {2} {3}", idEmpresa, anioInicio, anioFin, mes));
                DateTime fechaFinalProceso = DateTime.Now;
                _configCorreo.EnviarCorreo("La extracción de Movimientos de Polizas Semanal se genero correctamente"
                                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " +
                                           fechaFinalProceso
                                           + "\nTiempo de ejecucion : " +
                                           (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                    , "ETL Movimiento de Polizas Semanal Manual ");


                proceso.id_empresa = idEmpresa;
                proceso.tipo = Constantes.TIPO_EXT_PROGRAMADA;
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = Constantes.EST_EXT_FIN;
                proceso.mensaje = "";
                sw.start("AddProceso");
                _procesoDataAccessLayer.AddProceso(proceso);
                sw.stop();
            }

            catch (Exception ex)
            {
                logger.Error(ex, "Error en ejecucion de cargaFlujoEmpresa");

                DateTime fechaFinalProceso = DateTime.Now;
                _configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Movimientos de Polizas Semanal"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " +
                                           fechaFinalProceso
                                           + "\nTiempo de ejecucion : " +
                                           (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                    , "ETL Movimiento de Polizas Semanal Manual ");

                proceso.id_empresa = idEmpresa;
                proceso.tipo = Constantes.TIPO_EXT_PROGRAMADA;
                proceso.fecha_inicio = fechaInicioProceso;
                proceso.fecha_fin = fechaFinalProceso;
                proceso.estatus = Constantes.EST_EXT_ERR;
                proceso.mensaje = ex.Message;

                _procesoDataAccessLayer.AddProceso(proceso);
                throw;
            }

            logger.Info(sw.prettyPrint());
        }

        private int executeExtraccion(string arguments)
        {
            var configuration = GetConfiguration();

            //.\jre\bin\java.exe -jar .\extraccion-1.0.jar all -1 -1 -1
            string extraccionPath = configuration.GetSection("Data").GetSection("extraccionPath").Value;
            string javapath = extraccionPath + "\\jre\\bin\\java.exe";
           // string jarPath = extraccionPath + "extraccion-1.0.jar";
            string allArgs = " -jar extraccion-1.0.jar " + arguments;
            string cmd=javapath+ allArgs;
            logger.Info("executing.. '{0}'",cmd);
            var processInfo = new ProcessStartInfo(javapath, allArgs)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = extraccionPath
            };
            Process proc;

            if ((proc = Process.Start(processInfo)) == null)
            {
                throw new InvalidOperationException("No pudo inicializarce el proceso de extraccion");
            }
            using (StreamReader reader = proc.StandardOutput)
            {
                string result = reader.ReadToEnd();
                logger.Info("OUT EXTR: {0}",result);
            }

            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Close();
            if (exitCode != 0)
            {
                throw new Exception("El proceso de extraccion termino con error exitCode='"+exitCode+"' ");
            }
            return exitCode;
        }

        private IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}