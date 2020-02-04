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
                        semanal.year = Convert.ToInt32(rdr["year"]);
                        semanal.mes = Convert.ToInt32(rdr["mes"]);
                        semanal.poliza = Convert.ToInt32(rdr["poliza"]);
                        semanal.tp = Convert.ToString(rdr["tp"]);
                        semanal.linea = Convert.ToInt32(rdr["linea"]);
                        semanal.cta = Convert.ToInt32(rdr["cta"]);
                        semanal.scta = Convert.ToInt32(rdr["scta"]);
                        semanal.sscta = Convert.ToInt32(rdr["sscta"]);
                        semanal.concepto = Convert.ToString(rdr["concepto"]);
                        semanal.monto = Convert.ToDouble(rdr["monto"]);/// integer original
                        semanal.folio_imp = Convert.ToString(rdr["folio_imp"].ToString());///
                        semanal.itm = Convert.ToInt32(rdr["itm"]);
                        semanal.tm = Convert.ToInt32(rdr["tm"]);
                        semanal.numpro = Convert.ToString(rdr["NumProveedor"].ToString());///
                        semanal.cc = Convert.ToString(rdr["CentroCostos"]);
                        semanal.referencia = Convert.ToString(rdr["referencia"]);
                        semanal.orden_compra = Convert.ToString(rdr["orden_compra"].ToString());//                  
                        DateTime fechaPol = Convert.ToDateTime(rdr["fechapol"].ToString());//date
                        semanal.fechapol = fechaPol.ToString("dd/MM/yyyy");
                        semanal.id_empresa = idCompania;//Convert.ToInt32(rdr["idEmpresa"]);
                        semanal.id_version = Convert.ToInt32(rdr["idVersion"]);
                        semanal.cfd_ruta_pdf = Convert.ToString(rdr["cfd_ruta_pdf"]);
                        semanal.cfd_ruta_xml = Convert.ToString(rdr["cfd_ruta_xml"]);
                        semanal.uuid = Convert.ToString(rdr["uuid"]);


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
                               + "semanal ("
                               + "year, "
                               + "mes , "
                               + "poliza , "
                               + "tp , "
                               + "linea , "
                               + "cta , "
                               + "scta , "
                               + "sscta , " 
                               + "concepto , "
                               + "monto , "
                               + "folio_imp , "
                               + "itm , "
                               + "tm , "
                               + "numpro , "
                               + "cc , "
                               + "referencia , "
                               + "orden_compra , "
                               + "fechapol , "
                               + "id_empresa , "
                               + "id_version , "
                               + "cfd_ruta_pdf , "
                               + "cfd_ruta_xml , "
                               + "uuid) "
                                + "VALUES "
                                + " (@year,"
                                + " @mes,"
                                + " @poliza,"
                                + " @tp,"
                                + " @linea,"
                                + " @cta,"
                                + " @scta,"
                                + " @sscta,"
                                + " @concepto,"
                                + " @monto,"
                                + " @folio_imp,"
                                + " @itm,"
                                + " @tm,"
                                + " @numpro,"
                                + " @cc,"
                                + " @referencia,"
                                + " @orden_compra,"
                                + " @fechapol,"
                                + " @id_empresa,"
                                + " @id_version,"
                                + " @cfd_ruta_pdf,"
                                + " @cfd_ruta_xml,"
                                + " @uuid)";
                    ////////try
                    ////////{
                    {

                        foreach (Semanal semmanal in lstSemanal)
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand(insercion, con);

                            cmd.Parameters.AddWithValue("@year", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.year);
                            cmd.Parameters.AddWithValue("@mes", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.mes);
                            cmd.Parameters.AddWithValue("@poliza", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.poliza);
                            cmd.Parameters.AddWithValue("@tp", NpgsqlTypes.NpgsqlDbType.Text, semmanal.tp);
                            cmd.Parameters.AddWithValue("@linea", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.linea);
                            cmd.Parameters.AddWithValue("@cta", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.cta);
                            cmd.Parameters.AddWithValue("@scta", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.scta);
                            cmd.Parameters.AddWithValue("@sscta", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.sscta);
                            cmd.Parameters.AddWithValue("@concepto", NpgsqlTypes.NpgsqlDbType.Text, semmanal.concepto);
                            cmd.Parameters.AddWithValue("@monto", NpgsqlTypes.NpgsqlDbType.Text, semmanal.monto);
                            cmd.Parameters.AddWithValue("@folio_imp", NpgsqlTypes.NpgsqlDbType.Text, semmanal.folio_imp);
                            cmd.Parameters.AddWithValue("@itm", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.itm);
                            cmd.Parameters.AddWithValue("@tm", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.tm);
                            cmd.Parameters.AddWithValue("@numpro", NpgsqlTypes.NpgsqlDbType.Text, semmanal.numpro);
                            cmd.Parameters.AddWithValue("@cc", NpgsqlTypes.NpgsqlDbType.Text, semmanal.cc);
                            cmd.Parameters.AddWithValue("@referencia", NpgsqlTypes.NpgsqlDbType.Text, semmanal.referencia);
                            cmd.Parameters.AddWithValue("@orden_compra", NpgsqlTypes.NpgsqlDbType.Text, semmanal.orden_compra);
                            cmd.Parameters.AddWithValue("@fechapol", NpgsqlTypes.NpgsqlDbType.Text, semmanal.fechapol);
                            cmd.Parameters.AddWithValue("@id_empresa", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.id_empresa);
                            cmd.Parameters.AddWithValue("@id_version", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.id_version);
                            cmd.Parameters.AddWithValue("@cfd_ruta_pdf", NpgsqlTypes.NpgsqlDbType.Text, semmanal.cfd_ruta_pdf);
                            cmd.Parameters.AddWithValue("@cfd_ruta_xml", NpgsqlTypes.NpgsqlDbType.Text, semmanal.cfd_ruta_xml);
                            cmd.Parameters.AddWithValue("@uuid", NpgsqlTypes.NpgsqlDbType.Text, semmanal.uuid);
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
            else
            {
                return null;
            }
        }

    }
}
