using System;
using System.Collections.Generic;
using AppGia.Dao.Etl;
using AppGia.Models.Etl;
using AppGia.Util;

namespace WindowsService1.Helpers
{
    public class ETLHelper
    {
        ConfiguracionCorreoDataAccessLayer configCorreo = new ConfiguracionCorreoDataAccessLayer();
        ProcesoDataAccessLayer _procesoDataAccessLayer = new ProcesoDataAccessLayer();


        public void extraeBalanza()
        {
            ValidaExtraccionDataAccessLayer valiExtr = new ValidaExtraccionDataAccessLayer();
            List<ETLProg> lstExtrProg1 = valiExtr.lstParametros();
            string nombreCompania = "";

            //se para todas las empresas configuradas correr si la fecha y hora asi lo dicta
            foreach (ETLProg etlProg in lstExtrProg1)
            {
                //HNA: etl_prog no tiene asociacion con empresa solo define una fecha y hora lo cual tambien esta mal, ya que deberia definir una expresion cron
                Int64 idEmpresa = etlProg.id_empresa;
                try
                {
                    cargaBalanzaEmpresa(idEmpresa);
                }
                catch (Exception ex)
                {
                    configCorreo.EnviarCorreo(
                        "Estimado Usuario : \n\n  La extracción correspondiente a la compania " + idEmpresa + "." +
                        nombreCompania + " se genero incorrectamente \n\n Mensaje de Error: \n " + ex,
                        "ETL Extracción Balanza");
                }
            }
        }

        private void cargaBalanzaEmpresa(Int64 idEmpresa)
        {
            ETLBalanzaDataAccessLayer etlBalanza = new ETLBalanzaDataAccessLayer();
            string archivo = string.Empty;
            string ruta = Constantes.CSV_PATH_BALANZA;
            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();
            //borrar registros previos???
            try
            {
                archivo = etlBalanza.generaCSV(idEmpresa, ruta);
                etlBalanza.importFile(archivo, ruta);
                DateTime fechaFinalProceso = DateTime.Now;

                configCorreo.EnviarCorreo("La extracción de Balanza se genero correctamente"
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
                configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
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


        public void extraeFlujo()
        {
            ETLMovPolizaSemanalDataAccessLayer etlMovPoliza = new ETLMovPolizaSemanalDataAccessLayer();
            ValidaExtraccionDataAccessLayer valiExtr = new ValidaExtraccionDataAccessLayer();
            List<ETLProg> lstExtrProg1 = new List<ETLProg>();
            lstExtrProg1 = valiExtr.lstParametros();

            Int64 idEmpresa;
            string nombreCompania = "";

            foreach (ETLProg etlProg in lstExtrProg1)
            {
                idEmpresa = etlProg.id_empresa; //este dato no esta en bd


                try
                {
                    cargaFlujoEmpresa(idEmpresa);
                }
                catch (Exception ex)
                {
                    configCorreo.EnviarCorreo(
                        "Estimado Usuario : \n\n  La extracción correspondiente a la compania " + idEmpresa + "." +
                        nombreCompania + " se genero incorrectamente \n\n Mensaje de Error: \n " + ex,
                        "ETL Extracción Balanza");
                }
            }
        }

        private void cargaFlujoEmpresa(Int64 idEmpresa)
        {
            ETLMovPolizaSemanalDataAccessLayer etlMovSemanal = new ETLMovPolizaSemanalDataAccessLayer();
            string archivo = string.Empty;
            string anio = string.Empty;
            string ruta = Constantes.CSV_PATH_SEMANAL;

            DateTime fechaInicioProceso = DateTime.Now;
            Proceso proceso = new Proceso();

            try
            {
                archivo = etlMovSemanal.generaCSV(idEmpresa, ruta, anio);
                etlMovSemanal.importFile(archivo, ruta);

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreo.EnviarCorreo("La extracción de Movimientos de Polizas Semanal se genero correctamente"
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
                configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Movimientos de Polizas Semanal"
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