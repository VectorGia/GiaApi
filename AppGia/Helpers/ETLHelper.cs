using System;
using System.Collections.Generic;
using AppGia.Controllers;
using AppGia.Dao;
using AppGia.Dao.Etl;
using AppGia.Models;
using AppGia.Util;
using NLog;
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
            String consultaExistenRegs = "select count(1) as numRegs from balanza" ;
            bool existenRegs= ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (!existenRegs)
            {
                extraeBalanza(-1, -1);
            }
            else
            {
                int anioActual = new DateTime().Year;
                extraeBalanza(anioActual,anioActual);
            }
            
        }
        public void extraeBalanza(int anioInicio, int anioFin)
        {
            logger.Info("incio extraeBalanza (anioInicio={0}, anioFin={1}))",anioInicio,anioFin);
            StopWatch sw=new StopWatch(String.Format("extraeBalanza (anioInicio={0}, anioFin={1})",anioInicio,anioFin));
            sw.start("GetAllEmpresas ");
            List<Empresa> empresas = _empresaDataAccessLayer.GetAllEmpresas();
            sw.stop();
            foreach (Empresa empresa in empresas)
            {
                Int64 idEmpresa = empresa.id;
                sw.start("cargaBalanzaEmpresa "+idEmpresa);
                try
                {    
                    logger.Info("cargaBalanzaEmpresa(idEmpresa={0}, anioInicio={1}, anioFin={2})",idEmpresa,anioInicio,anioFin);
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
            String consultaExistenRegs = "select count(1) as numRegs from semanal" ;
            bool existenRegs= ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (!existenRegs)
            {
                extraeFlujo(-1, -1,-1);
            }
            else
            {
                int anioActual = new DateTime().Year;
                int mes = new DateTime().Month;
                extraeFlujo(anioActual,anioActual,mes);
            }
        }
        public void extraeFlujo(int anioInicio, int anioFin, int mes)
        {
            logger.Info("extraeFlujo(anioInicio={0}, anioFin={1}, mes={2})", anioInicio,  anioFin,  mes);
            StopWatch sw=new StopWatch(String.Format("extraeFlujo(anioInicio={0}, anioFin={1}, mes={2})", anioInicio,  anioFin,  mes));
            List<Empresa> empresas = _empresaDataAccessLayer.GetAllEmpresas();


            Int64 idEmpresa;

            foreach (Empresa empresa in empresas)
            {
                idEmpresa = empresa.id;
                
                try
                {
                    sw.start("cargaFlujoEmpresa "+idEmpresa);
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
        
        private void deleteBalanzaIfApply(Int64 idEmpresa, int anioInicio, int anioFin)
        {
            String consultaExistenRegs = "select count(1) as numRegs from balanza where id_empresa=" + idEmpresa;
            String deleteRegs = "delete from balanza where id_empresa=" + idEmpresa;

            if (anioInicio > 0 && anioFin > 0)
            {
                consultaExistenRegs += "  and  year between " + anioInicio + " and " + anioFin;
                deleteRegs += "  and  year between " + anioInicio + " and " + anioFin;
            }

            bool existenRegistros =
                ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (existenRegistros)
            {
                _queryExecuter.execute(deleteRegs);
            }
        }

        private void cargaBalanzaEmpresa(Int64 idEmpresa, int anioInicio, int anioFin)
        {
            StopWatch sw=new StopWatch("cargaBalanzaEmpresa "+idEmpresa);
            ETLBalanzaDataAccessLayer etlBalanza = new ETLBalanzaDataAccessLayer();
            string archivo = string.Empty;
            string ruta = Constantes.CSV_PATH_BALANZA;
            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();
            try
            {
                sw.start("generaCSV "+idEmpresa);
                archivo = etlBalanza.generaCSV(idEmpresa, anioInicio, anioFin, ruta);
                sw.stop();
                sw.start("deleteBalanzaIfApply");
                deleteBalanzaIfApply(idEmpresa, anioInicio, anioFin);
                sw.stop();
                sw.start("importFile");
                etlBalanza.importFile(archivo, ruta);
                sw.stop();
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
                sw.start("UpdateCuentaUnificada");

                etlBalanza.UpdateCuentaUnificada(idEmpresa);
                sw.stop();
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


        private void deleteSemanalIfApply(Int64 idEmpresa, int anioInicio, int anioFin,int mes)
        {
            String consultaExistenRegs = "select count(1) as numRegs from semanal where id_empresa=" + idEmpresa;
            String deleteRegs = "delete from semanal where id_empresa=" + idEmpresa;

            if (anioInicio > 0 && anioFin > 0)
            {
                consultaExistenRegs += "  and  year between " + anioInicio + " and " + anioFin;
                deleteRegs += "  and  year between " + anioInicio + " and " + anioFin;
            }
            if (mes > 0)
            {
                consultaExistenRegs += "  and b.mes = " + mes;
            }
            bool existenRegistros =
                ToInt64(_queryExecuter.ExecuteQueryUniqueresult(consultaExistenRegs)["numRegs"]) > 0;
            if (existenRegistros)
            {
                _queryExecuter.execute(deleteRegs);
            }
        }

        private void cargaFlujoEmpresa(Int64 idEmpresa, int anioInicio, int anioFin, int mes)
        {
            StopWatch sw=new StopWatch(String.Format("cargaFlujoEmpresa (idEmpresa={0},  anioInicio={1},  anioFin={2},  mes={3})", idEmpresa,  anioInicio,  anioFin,  mes));
            logger.Info("cargaFlujoEmpresa( idEmpresa={0},  anioInicio={1},  anioFin={2},  mes={3})",idEmpresa, anioInicio, anioFin, mes);
            ETLMovPolizaSemanalDataAccessLayer etlMovSemanal = new ETLMovPolizaSemanalDataAccessLayer();
            string ruta = Constantes.CSV_PATH_SEMANAL;

            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();

            try
            {
                logger.Info("etlMovSemanal.generaCSV(idEmpresa={0}, ruta={1}, anioInicio={2}, anioFin={3}, mes={4})",idEmpresa, ruta, anioInicio, anioFin, mes);
                sw.start("generaCSV");
                string archivo = etlMovSemanal.generaCSV(idEmpresa, ruta, anioInicio, anioFin, mes);
                sw.stop();
                
                logger.Info("deleteSemanalIfApply(idEmpresa={0}, anioInicio={1}, anioFin={2},mes={3})",idEmpresa, anioInicio, anioFin,mes);
                sw.start("deleteSemanalIfApply");
                deleteSemanalIfApply(idEmpresa, anioInicio, anioFin,mes);
                sw.stop();
                
                logger.Info("importFile(archivo={0}, ruta={1})",archivo, ruta);
                sw.start("importFile");
                etlMovSemanal.importFile(archivo, ruta);
                sw.stop();

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
    }
}