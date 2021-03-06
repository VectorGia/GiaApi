﻿﻿using System;
 using System.Data.Odbc;
 using System.IO;
 using WindowsService1.Util;
 using AppGia.Models.Etl;
 using AppGia.Util;
 using NLog;
 using Npgsql;

 namespace AppGia.Dao.Etl
{
    public class ETLMovPolizaSemanalDataAccessLayer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        NpgsqlConnection conP;
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand comP = new NpgsqlCommand();

        OdbcConnection odbcCon;
        DSNConfig dsnConfig = new DSNConfig();


        public ETLMovPolizaSemanalDataAccessLayer()
        {
            conP = conex.ConnexionDB();
        }
        

        public int importFile(string nombre_archivo, string ruta_archivo)
        {
            int resultado;
            string script_copy = string.Empty;
            script_copy = " copy semanal (" + Constantes.HEADER_SEMANAL_CSV + ") from '" + ruta_archivo +
                          nombre_archivo + "'" + " delimiter ',' csv header ";

            try
            {
                conP.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(script_copy, conP);
                resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            finally
            {
                conP.Close();
            }

            return resultado;
        }


        public string generaCSV(Int64 idEmpresa, string ruta, int anioInicio, int anioFin, int mes)
        {
            string nombreArchivo = string.Empty;
            string registros = string.Empty;
            nombreArchivo = Constantes.NOMBRE_ARCHIVO_POL_SEM + "_" + idEmpresa + DateTime.Now.ToString("ddMMyyyy") +
                            DateTime.Now.ToString("HHmmSS") + ".csv";
            StreamWriter layout;
            layout = File.CreateText(ruta + nombreArchivo);


            try
            {
                DSN dsn = dsnConfig.crearDSN(idEmpresa);
                
                logger.Info("generando conexion odbc {0}",dsn.nombreDSN);
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);
                string consulta = " SELECT "
                                  + " a.year year,"
                                  + " a.mes mes,"
                                  + " a.poliza poliza,"
                                  + " a.tp tp,"
                                  + " a.linea linea,"
                                  + " a.cta cta,"
                                  + " a.scta scta,"
                                  + " a.sscta sscta,"
                                  + " a.concepto concepto,"
                                  + " a.monto monto,"
                                  + " a.folio_imp folio_imp,"
                                  + " a.itm itm,"
                                  + " a.tm tm,"
                                  + " a.numpro NumProveedor,"
                                  + " a.cc CentroCostos,"
                                  + " a.referencia referencia,"
                                  + " a.orden_compra,"
                                  + " b.fechapol,"
                                  + " 0 as idEmpresa,"
                                  + " 1 AS idVersion,"
                                  + " cfd_ruta_pdf,"
                                  + " cfd_ruta_xml,"
                                  + " uuid "
                                  + "  FROM sc_movpol a "
                                  + " INNER join sc_polizas b "
                                  + " on a.year = b.year"
                                  + " and a.mes = b.mes"
                                  + " and a.tp = b.tp"
                                  + " and a.poliza = b.poliza WHERE a.year >0 And Status = 'A'";

                if (anioInicio > 0 && anioFin > 0)
                {
                    consulta += "  where b.year between " + anioInicio + " and " + anioFin;
                }

                if (mes > 0)
                {
                    if (consulta.Contains("where"))
                    {
                        consulta += "  and b.mes = " + mes;
                    }
                    else
                    {
                        consulta += "  where b.mes = " + mes;
                    }
                }
                logger.Info("Consultando sybase {0}",dsn.nombreDSN);
                OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                odbcCon.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    registros = Convert.ToInt32(rdr["year"]) + ","
                                                             + Convert.ToInt32(rdr["mes"]) + ","
                                                             + Convert.ToInt32(rdr["poliza"]) + ","
                                                             + Convert.ToString(rdr["tp"]) + ","
                                                             + Convert.ToInt32(rdr["linea"]) + ","
                                                             + Convert.ToInt32(rdr["cta"]) + ","
                                                             + Convert.ToInt32(rdr["scta"]) + ","
                                                             + Convert.ToInt32(rdr["sscta"]) + ","
                                                             + Convert.ToString(rdr["concepto"])
                                                                 .Replace(",", "").Replace(@"""", "") + ","
                                                             + Convert.ToDouble(rdr["monto"]) +
                                                             ","
                                                             + Convert.ToString(rdr["folio_imp"].ToString()) +
                                                             ","
                                                             + Convert.ToInt32(rdr["itm"]) + ","
                                                             + Convert.ToInt32(rdr["tm"]) + ","
                                                             + Convert.ToString(rdr["NumProveedor"]
                                                                 .ToString()) + ","
                                                             + Convert.ToString(rdr["CentroCostos"]) + ","
                                                             + Convert.ToString(rdr["referencia"]) + ","
                                                             + Convert.ToString(rdr["orden_compra"]
                                                                 .ToString()) + ","
                                                             + Convert.ToDateTime(rdr["fechapol"].ToString()) +
                                                             "," 
                                                             + idEmpresa + ","
                                                             + Convert.ToInt32(rdr["idVersion"]) + ","
                                                             + Convert.ToString(rdr["cfd_ruta_pdf"]) + ","
                                                             + Convert.ToString(rdr["cfd_ruta_xml"]) + ","
                                                             + Convert.ToString(rdr["uuid"]) + ","
                                                             + Constantes.EXTRACCION_MANUAL + ","
                                                             + "'" + DateTime.Now + "'," + "'" + DateTime.Now.ToString("h:mm tt") + "'";
                    layout.WriteLine(registros, Environment.NewLine);
                }

                return nombreArchivo;
            }
            finally
            {
                layout.Close();
                if (odbcCon != null)
                {
                    odbcCon.Close();
                }
            }
        }
    }
}