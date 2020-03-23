using System;
using System.Collections.Generic;
using AppGia.Controllers;
using AppGia.Dao;
using AppGia.Dao.Etl;
using AppGia.Models;
using AppGia.Util;
using static System.Convert;
using Proceso = AppGia.Models.Etl.Proceso;
using ProcesoDataAccessLayer = AppGia.Dao.Etl.ProcesoDataAccessLayer;

namespace AppGia.Helpers
{
    public class ETLHelper
    {
        private ConfiguracionCorreoDataAccessLayer _configCorreo = new ConfiguracionCorreoDataAccessLayer();
        private ProcesoDataAccessLayer _procesoDataAccessLayer = new ProcesoDataAccessLayer();
        private EmpresaDataAccessLayer _empresaDataAccessLayer = new EmpresaDataAccessLayer();
        private QueryExecuter _queryExecuter = new QueryExecuter();


        public void extraeBalanzaAuto()
        {
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
            List<Empresa> empresas = _empresaDataAccessLayer.GetAllEmpresas();
            foreach (Empresa empresa in empresas)
            {
                Int64 idEmpresa = empresa.id;
                try
                {
                    cargaBalanzaEmpresa(idEmpresa, anioInicio, anioFin);
                }
                catch (Exception ex)
                {
                    _configCorreo.EnviarCorreo(
                        "Estimado Usuario : \n\n  La extracción(balanza) correspondiente a la compania " + idEmpresa +
                        "." +
                        empresa.nombre + " se genero incorrectamente \n\n Mensaje de Error: \n " + ex,
                        "ETL Extracción Balanza");
                }
            }
        }

        public void extraeFlujoAuto()
        {
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
            List<Empresa> empresas = _empresaDataAccessLayer.GetAllEmpresas();


            Int64 idEmpresa;

            foreach (Empresa empresa in empresas)
            {
                idEmpresa = empresa.id;

                try
                {
                    cargaFlujoEmpresa(idEmpresa, anioInicio, anioFin, mes);
                }
                catch (Exception ex)
                {
                    _configCorreo.EnviarCorreo(
                        "Estimado Usuario : \n\n  La extracción(Flujo) correspondiente a la compania " + idEmpresa +
                        "." +
                        empresa.nombre + " se genero incorrectamente \n\n Mensaje de Error: \n " + ex,
                        "ETL Extracción Balanza");
                }
            }
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
            ETLBalanzaDataAccessLayer etlBalanza = new ETLBalanzaDataAccessLayer();
            string archivo = string.Empty;
            string ruta = Constantes.CSV_PATH_BALANZA;
            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();
            try
            {
                archivo = etlBalanza.generaCSV(idEmpresa, anioInicio, anioFin, ruta);
                deleteBalanzaIfApply(idEmpresa, anioInicio, anioFin);
                etlBalanza.importFile(archivo, ruta);

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

                _procesoDataAccessLayer.AddProceso(proceso);

                etlBalanza.UpdateCuentaUnificada(idEmpresa);
            }
            catch (Exception ex)
            {
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
            ETLMovPolizaSemanalDataAccessLayer etlMovSemanal = new ETLMovPolizaSemanalDataAccessLayer();
            string ruta = Constantes.CSV_PATH_SEMANAL;

            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();

            try
            {
                string archivo = etlMovSemanal.generaCSV(idEmpresa, ruta, anioInicio, anioFin, mes);
                deleteSemanalIfApply(idEmpresa, anioInicio, anioFin,mes);
                etlMovSemanal.importFile(archivo, ruta);

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

                _procesoDataAccessLayer.AddProceso(proceso);
            }

            catch (Exception ex)
            {
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
        }
    }
}