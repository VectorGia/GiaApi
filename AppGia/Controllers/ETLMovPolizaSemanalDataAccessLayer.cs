using AppGia.Models;
using AppGia.Util.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class ETLMovPolizaSemanalDataAccessLayer
    {
        ConfigCorreoController configCorreo = new ConfigCorreoController();
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand comP = new NpgsqlCommand();

        OdbcConnection odbcCon;
        OdbcCommand cmdETL = new OdbcCommand();
        char cod = '"';
        DSNConfig dsnConfig = new DSNConfig();

        SqlConnection conSQLETL = new SqlConnection();
        SqlCommand comSQLETL = new SqlCommand();


        public ETLMovPolizaSemanalDataAccessLayer()
        {
            con = conex.ConnexionDB();
            //con.Open();
            //conSc=conex.ConexionSybase();
            //odbcCon = conex.ConexionSybaseodbc();


        }

        public List<Semanal> obtenerReporteSemanalPolizasSybaseD(int idCompania, String anio)
        {

            //empresa 2  odbcCon = conex.ConexionSybaseodbc("2_GRUPO_ INGENIERIA");
            ///empresa 4
            //odbcCon = conex.ConexionSybaseodbc("4_CONSTRUCTORA_Y_EDIFICADORA");

            DSN dsn = new DSN();
            dsn = dsnConfig.crearDSN(idCompania);
            if (dsn.creado)
            {
                /// obtener conexion de Odbc creado 
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);
                try
                {


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
                        + " and a.year = " + anio
                        + " and a.mes = b.mes"
                        + " and a.tp = b.tp"
                        + " and a.poliza = b.poliza WHERE a.year >0 And Status = 'A'";

                    OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                    odbcCon.Open();
                    OdbcDataReader rdr = cmd.ExecuteReader();

                    List<Semanal> listaSemanal = new List<Semanal>();
                    while (rdr.Read())
                    {
                        Semanal semanal = new Semanal();
                        semanal.NUM_YEAR = Convert.ToInt32(rdr["year"]);
                        semanal.NUM_MES = Convert.ToInt32(rdr["mes"]);
                        semanal.NUM_POLIZA = Convert.ToInt32(rdr["poliza"]);
                        semanal.TEXT_TP = Convert.ToString(rdr["tp"]);
                        semanal.NUM_LINEA = Convert.ToInt32(rdr["linea"]);
                        semanal.NUM_CTA = Convert.ToInt32(rdr["cta"]);
                        semanal.NUM_SCTA = Convert.ToInt32(rdr["scta"]);
                        semanal.NUM_SSCTA = Convert.ToInt32(rdr["sscta"]);
                        semanal.TEXT_CONCEPTO = Convert.ToString(rdr["concepto"]);
                        semanal.TEXT_MONTO = Convert.ToString(rdr["monto"].ToString());/// integer original
                        semanal.TEXT_FOLIO_IMP = Convert.ToString(rdr["folio_imp"].ToString());///
                        semanal.NUM_ITM = Convert.ToInt32(rdr["itm"]);
                        semanal.NUM_TM = Convert.ToInt32(rdr["tm"]);
                        semanal.TEXT_NUMPRO = Convert.ToString(rdr["NumProveedor"].ToString());///
                        semanal.TEXT_CC = Convert.ToString(rdr["CentroCostos"]);
                        semanal.TEXT_REFERENCIA = Convert.ToString(rdr["referencia"]);
                        semanal.TEXT_ORDEN_COMPRA = Convert.ToString(rdr["orden_compra"].ToString());//                  
                        DateTime fechaPol = Convert.ToDateTime(rdr["fechapol"].ToString());//date
                        semanal.TEXT_FECHAPOL = fechaPol.ToString("dd/MM/yyyy");
                        semanal.INT_IDEMPRESA = idCompania;//Convert.ToInt32(rdr["idEmpresa"]);
                        semanal.INT_IDVERSION = Convert.ToInt32(rdr["idVersion"]);
                        semanal.TEXT_CFD_RUTA_PDF = Convert.ToString(rdr["cfd_ruta_pdf"]);
                        semanal.TEXT_CFD_RUTA_XML = Convert.ToString(rdr["cfd_ruta_xml"]);
                        semanal.TEXT_UUID = Convert.ToString(rdr["uuid"]);


                        listaSemanal.Add(semanal);

                    }


                    return listaSemanal;

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw;
                }
                finally
                {
                    odbcCon.Close();
                }
            }
            else { return null; }
        }


        public int insertarReporteSemanalPg(int idCompania, string anioInicio, string anioFin)
        {
            int cantFilaAfect = 0;
            DateTime fechaInicioProceso = DateTime.Now;
            List<Semanal> lstSemanal = new List<Semanal>();
            try
            {
                List<String> listAnios = obtenerAniosPolizasSybase(idCompania, anioInicio, anioFin);
                foreach (String anio in listAnios)
                {
                    fechaInicioProceso = DateTime.Now;

                    lstSemanal = obtenerReporteSemanalPolizasSybaseD(idCompania, anio);
                    //lstSemanal = convertirReporteSemabalSybaseToPg(id_compania);
                    con.Open();
                    string insercion = "INSERT INTO "
                                + cod + "TAB_SEMANAL" + cod + "("
                                + cod + "NUM_YEAR" + cod + ", "
                                + cod + "NUM_MES" + cod + ", "
                                + cod + "NUM_POLIZA" + cod + ", "
                                + cod + "TEXT_TP" + cod + ", "
                                + cod + "NUM_LINEA" + cod + ", "
                                + cod + "NUM_CTA" + cod + ", "
                                + cod + "NUM_SCTA" + cod + ", "
                                + cod + "NUM_SSCTA" + cod + ", "
                                + cod + "TEXT_CONCEPTO" + cod + ", "
                                + cod + "TEXT_MONTO" + cod + ", "
                                + cod + "TEXT_FOLIO_IMP" + cod + ", "
                                + cod + "NUM_ITM" + cod + ", "
                                + cod + "NUM_TM" + cod + ", "
                                + cod + "TEXT_NUMPRO" + cod + ", "
                                + cod + "TEXT_CC" + cod + ", "
                                + cod + "TEXT_REFERENCIA" + cod + ", "
                                + cod + "TEXT_ORDEN_COMPRA" + cod + ", "
                                + cod + "TEXT_FECHAPOL" + cod + ", "
                                + cod + "INT_IDEMPRESA" + cod + ", "
                                + cod + "INT_IDVERSION" + cod + ", "
                                + cod + "TEXT_CFD_RUTA_PDF" + cod + ", "
                                + cod + "TEXT_CFD_RUTA_XML" + cod + ", "
                                + cod + "TEXT_UUID" + cod + ")"

                                + "VALUES "
                                + " (@NUM_YEAR,"
                                + " @NUM_MES,"
                                + " @NUM_POLIZA,"
                                + " @TEXT_TP,"
                                + " @NUM_LINEA,"
                                + " @NUM_CTA,"
                                + " @NUM_SCTA,"
                                + " @NUM_SSCTA,"
                                + " @TEXT_CONCEPTO,"
                                + " @TEXT_MONTO,"
                                + " @TEXT_FOLIO_IMP,"
                                + " @NUM_ITM,"
                                + " @NUM_TM,"
                                + " @TEXT_NUMPRO,"
                                + " @TEXT_CC,"
                                + " @TEXT_REFERENCIA,"
                                + " @TEXT_ORDEN_COMPRA,"
                                + " @TEXT_FECHAPOL,"
                                + " @INT_IDEMPRESA,"
                                + " @INT_IDVERSION,"
                                + " @TEXT_CFD_RUTA_PDF,"
                                + " @TEXT_CFD_RUTA_XML,"
                                + " @TEXT_UUID)";
                    ////////try
                    ////////{
                    {

                        foreach (Semanal semmanal in lstSemanal)
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand(insercion, con);

                            cmd.Parameters.AddWithValue("@NUM_YEAR", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_YEAR);
                            cmd.Parameters.AddWithValue("@NUM_MES", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_MES);
                            cmd.Parameters.AddWithValue("@NUM_POLIZA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_POLIZA);
                            cmd.Parameters.AddWithValue("@TEXT_TP", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_TP);
                            cmd.Parameters.AddWithValue("@NUM_LINEA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_LINEA);
                            cmd.Parameters.AddWithValue("@NUM_CTA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_CTA);
                            cmd.Parameters.AddWithValue("@NUM_SCTA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_SCTA);
                            cmd.Parameters.AddWithValue("@NUM_SSCTA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_SSCTA);
                            cmd.Parameters.AddWithValue("@TEXT_CONCEPTO", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_CONCEPTO);
                            cmd.Parameters.AddWithValue("@TEXT_MONTO", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_MONTO);
                            cmd.Parameters.AddWithValue("@TEXT_FOLIO_IMP", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_FOLIO_IMP);
                            cmd.Parameters.AddWithValue("@NUM_ITM", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_ITM);
                            cmd.Parameters.AddWithValue("@NUM_TM", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.NUM_TM);
                            cmd.Parameters.AddWithValue("@TEXT_NUMPRO", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_NUMPRO);
                            cmd.Parameters.AddWithValue("@TEXT_CC", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_CC);
                            cmd.Parameters.AddWithValue("@TEXT_REFERENCIA", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_REFERENCIA);
                            cmd.Parameters.AddWithValue("@TEXT_ORDEN_COMPRA", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_ORDEN_COMPRA);
                            cmd.Parameters.AddWithValue("@TEXT_FECHAPOL", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_FECHAPOL);
                            cmd.Parameters.AddWithValue("@INT_IDEMPRESA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.INT_IDEMPRESA);
                            cmd.Parameters.AddWithValue("@INT_IDVERSION", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.INT_IDVERSION);
                            cmd.Parameters.AddWithValue("@TEXT_CFD_RUTA_PDF", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_CFD_RUTA_PDF);
                            cmd.Parameters.AddWithValue("@TEXT_CFD_RUTA_XML", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_CFD_RUTA_XML);
                            cmd.Parameters.AddWithValue("@TEXT_UUID", NpgsqlTypes.NpgsqlDbType.Text, semmanal.TEXT_UUID);
                            //conP.Open();
                            // int cantFilaAfect = Convert.ToInt32(cmd.ExecuteNonQuery());
                            cantFilaAfect = cantFilaAfect + Convert.ToInt32(cmd.ExecuteNonQuery());
                        }

                        con.Close();
                        ////////DateTime fechaFinalProceso = DateTime.Now;
                        ////////configCorreo.EnviarCorreo("La extracción Semanal se genero correctamente para el año  "
                        ////////                            + anio + "Fecha Inicio : " + fechaInicioProceso + " Fecha Final: " + fechaFinalProceso
                        ////////                            + "Tiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins", "ETL Reporte Semanal");
                        //return cantFilaAfect;
                    }
                    ////////}
                    ////////catch (Exception ex)
                    ////////{
                    ////////    con.Close();
                    ////////    DateTime fechaFinalProceso = DateTime.Now;
                    ////////    configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción Semanal se genero incorrectamente para el "
                    ////////                                + anio + "Fecha Inicio : " + fechaInicioProceso + " Fecha Final: " + fechaFinalProceso
                    ////////                                + "Tiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                    ////////                                + "\n Ocurrio el siguiente Error \n" + ex.Message, "ETL Reporte Semanal");
                    ////////    string error = ex.Message;
                    ////////    throw;
                    ////////}



                }

                DateTime fechaFinalProceso = DateTime.Now;
                configCorreo.EnviarCorreo("La extracción Semanal se genero correctamente"
                                            + "\nFecha Inicio : " + fechaInicioProceso
                                            + "\nFecha Final: " + fechaFinalProceso
                                            + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins",
                                            "ETL Reporte Semanal");

            }
            catch (Exception ex)
            {
                con.Close();
                DateTime fechaFinalProceso = DateTime.Now;
                configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción Semanal"
                                            + "\nFecha Inicio:" + fechaInicioProceso
                                            + "\nFecha Final:" + fechaFinalProceso
                                            + "\nTiempo de ejecucion: " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                            + "\nOcurrio el siguiente Error: \n" + ex.Message, "ETL Reporte Semanal");
                string error = ex.Message;
                throw;
            }
            return cantFilaAfect;
        }

        public List<String> obtenerAniosPolizasSybase(int idCompania, string anioInicio, string anioFin)
        {
           // odbcCon = conex.ConexionSybaseodbc("4_CONSTRUCTORA_Y_EDIFICADORA");
            DSN dsn = new DSN();
            dsn = dsnConfig.crearDSN(idCompania);
            if (dsn.creado)
            {
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);
                try
                {
                    string anioI = "";
                    string anioF = "";

                    if (anioInicio != null && anioFin != null)
                    {
                        if (anioInicio != "" && anioFin != "")
                        {
                            anioI = " and a.year >= " + anioInicio;
                            anioFin = " and a.year <= " + anioFin;
                        }
                    }

                    string consulta = " SELECT "
                        + "  a.year year,"
                        + "  FROM sc_movpol a "
                        + "  INNER join sc_polizas b "
                        + "  on a.year = b.year"
                        + anioI
                        + anioF
                        + "  and a.mes = b.mes"
                        + "  and a.tp = b.tp"
                        + "  and a.poliza = b.poliza WHERE a.year >0 And Status = 'A'";

                    OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                    odbcCon.Open();
                    OdbcDataReader rdr = cmd.ExecuteReader();

                    List<String> listaAnios = new List<String>();
                    while (rdr.Read())
                    {
                        string year = "";
                        year = Convert.ToString(rdr["year"]);

                        listaAnios.Add(year);

                    }
                    return listaAnios;

                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw;
                }
                finally
                {
                    odbcCon.Close();
                }
            }
            else { 
                return null;
            }
        }

    }
}
