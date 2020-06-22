﻿using System;
 using System.Data.Odbc;
 using System.IO;
 using WindowsService1.Util;
 using AppGia.Models.Etl;
 using AppGia.Util;
 using NLog;
 using Npgsql;

 namespace AppGia.Dao.Etl
{
    public class ETLBalanzaDataAccessLayer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        Conexion.Conexion conex = new Conexion.Conexion();

        NpgsqlConnection conP = new NpgsqlConnection();

        DSNConfig dsnConfig = new DSNConfig();
        OdbcConnection odbcCon;

        public ETLBalanzaDataAccessLayer()
        {
            conP = conex.ConnexionDB();
        }


        public string generaCSV(Int64 idEmpresa, int anioInicio, int anioFin, string ruta)
        {
            string nombreArchivo = string.Empty;
            string registros = string.Empty;
            nombreArchivo = Constantes.NOMBRE_ARCHIVO_BALANZA + "_" + idEmpresa + DateTime.Now.ToString("ddMMyyyy") +
                            DateTime.Now.ToString("HHmmSS") + ".csv";
            StreamWriter layout = File.CreateText(ruta + nombreArchivo);

            try
            {
                DSN dsn = dsnConfig.crearDSN(idEmpresa);
                logger.Debug("generando conexion odbc {0}",dsn.nombreDSN);
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);
                odbcCon.Open();
                layout.WriteLine(Constantes.HEADER_BALANZA_CSV);
                string consulta = " SELECT " + "year," + "cta," + "scta," + "sscta,"
                                  + "salini," + "enecargos," + "eneabonos,"
                                  + "febcargos," + "febabonos," + "marcargos,"
                                  + "marabonos," + "abrcargos," + "abrabonos,"
                                  + "maycargos," + "mayabonos," + "juncargos," + "junabonos,"
                                  + "julcargos," + "julabonos," + "agocargos,"
                                  + "agoabonos," + "sepcargos," + "sepabonos," + "octcargos,"
                                  + "octabonos," + "novcargos," + "novabonos,"
                                  + "diccargos," + "dicabonos," + "cierreabonos,"
                                  + "cierrecargos," + "acta," + "cc"
                                  + " FROM sc_salcont_cc";

                if (anioInicio > 0 && anioFin > 0)
                {
                    consulta += "  where year between " + anioInicio + " and " + anioFin;
                }
                logger.Debug("Consultando sybase {0}",dsn.nombreDSN);
                OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    registros = Convert.ToString(rdr["cta"].ToString()) + ","
                                                                        + Convert.ToString(rdr["scta"]) + ","
                                                                        + Convert.ToString(rdr["sscta"]) + ","
                                                                        + Convert.ToInt32(rdr["year"]) + ","
                                                                        + Convert.ToDouble(rdr["salini"]) + ","
                                                                        + Convert.ToDouble(rdr["enecargos"]) + ","
                                                                        + Convert.ToDouble(rdr["eneabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["febcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["febabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["marcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["marabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["abrcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["abrabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["maycargos"]) + ","
                                                                        + Convert.ToDouble(rdr["mayabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["juncargos"]) + ","
                                                                        + Convert.ToDouble(rdr["junabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["julcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["julabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["agocargos"]) + ","
                                                                        + Convert.ToDouble(rdr["agoabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["sepcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["sepabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["octcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["octabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["novcargos"]) + ","
                                                                        + Convert.ToDouble(rdr["novabonos"]) + ","
                                                                        + Convert.ToDouble(rdr["diccargos"]) + ","
                                                                        + Convert.ToDouble(rdr["dicabonos"]) + "," +
                                                                        " 0,"
                                                                        + Constantes.EXTRACCION_MANUAL + "," +
                                                                        idEmpresa + ","
                                                                        + Convert.ToDouble(rdr["cierrecargos"]) + ","
                                                                        + Convert.ToDouble(rdr["cierreabonos"]) + ","
                                                                        + Convert.ToInt32(rdr["acta"]) + ","
                                                                        + Convert.ToString(rdr["cc"]) + ","
                                                                        + "'" + DateTime.Now.ToString("HH:mm:ss") +
                                                                        "'" + "'" + DateTime.Now.ToString("dd/MM/yyy");
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

        public int importFile(string nombre_archivo, string ruta_archivo)
        {
            int resultado;
            string script_copy = string.Empty;
            script_copy = " copy balanza (" + Constantes.HEADER_BALANZA_CSV + ") from '" + ruta_archivo +
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

        public int UpdateCuentaUnificada(Int64 idEmpresa)
        {
            string update = "   update balanza "
                            + " set "
                            + " cuenta_unificada=LPAD(cta,4,'0')||LPAD(scta,4,'0')||LPAD(sscta,4,'0') "
                            + " where id_empresa = " + idEmpresa
                            + "  and cuenta_unificada is null";

            try
            {
                conP.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(update, conP);

                int cantFilAfec = cmd.ExecuteNonQuery();
                conP.Close();
                return cantFilAfec;
            }
            finally
            {
                conP.Close();
            }
        }
    }
}