using AdoNetCore.AseClient;
using AppGia.Models;
using Npgsql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Odbc;
using AppGia.Controllers;
using AppGia.Util.Models;

namespace AppGia.Dao
{
    public class ETLDataAccesLayer
    {
        ConfigCorreoController configCorreo = new ConfigCorreoController();
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand comP = new NpgsqlCommand();

        AseConnection conSc;
        OdbcConnection odbcCon;
        OdbcCommand cmdETL = new OdbcCommand();
        char cod = '"';
        DSNConfig dsnConfig = new DSNConfig();

        SqlConnection conSQLETL = new SqlConnection();
        SqlCommand comSQLETL = new SqlCommand();



        public ETLDataAccesLayer()
        {
            con = conex.ConnexionDB();
            //con.Open();
            //conSc=conex.ConexionSybase();
            //odbcCon = conex.ConexionSybaseodbc();


        }



        /// <summary>
        /// Metodo para obtener los valores de la cadena de conexion de la empresa
        /// </summary>
        /// <param name="compania"></param>
        /// <returns></returns>
        public DataTable CadenaConexionETL(int id_compania)
        {
            string add = "SELECT " + cod + "INT_IDCOMPANIA_P" + cod + ","
                    + cod + "STR_USUARIO_ETL" + cod + ","
                    + cod + "STR_NOMBRE_COMPANIA" + cod + ","
                    + cod + "STR_CONTRASENIA_ETL" + cod + ","
                    + cod + "STR_HOST_COMPANIA" + cod + ","
                    + cod + "STR_PUERTO_COMPANIA" + cod + ","
                    + cod + "STR_BD_COMPANIA" + cod
                    + " FROM " + cod + "CAT_COMPANIA" + cod
                    + " WHERE " + cod + "INT_IDCOMPANIA_P" + cod + " = " + id_compania;
            try
            {
                con.Open();

                comP = new Npgsql.NpgsqlCommand(add, con);

                Npgsql.NpgsqlDataAdapter daP = new Npgsql.NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
            finally
            {
                con.Close();
            }
        }



        /// <summary>
        /// Se convierte el DataTable en una Lista Generica
        /// </summary>
        /// <param name="compania"></param>
        /// <returns>Lista del tipo Compania</returns>
        public List<Empresa> CadenaConexionETL_lst(int id_compania)
        {
            List<Empresa> lst = new List<Empresa>();
            DataTable dt = new DataTable();
            dt = CadenaConexionETL(id_compania);

            foreach (DataRow r in dt.Rows)
            {
                Empresa cia = new Empresa();
                cia.usuario_etl = r["usuario_etl"].ToString();
                cia.contrasenia_etl = r["contrasenia_etl"].ToString();
                cia.host = r["host"].ToString();
                cia.puerto_compania = Convert.ToInt32(r["puerto_compania"]);
                cia.bd_name = r["bd_name"].ToString();
                cia.id = Convert.ToInt32(r["id"]);

                lst.Add(cia);
            }
            return lst;
        }





        /// <summary>
        /// Crea la cadena de conexion segun lo guardado en la tabla compania
        /// realiza el select a la tabla Balanza
        /// </summary>
        /// <param name="compania"></param>
        /// <returns>Extracción del SQL</returns>
        public List<ScSaldoConCc> obtenerSalContCC(int id_compania)
        {
            //string UserID = string.Empty;
            //string Password = string.Empty;
            //string Host = string.Empty;
            //string Port = string.Empty;
            //string DataBase = string.Empty;
            //string Cadena = string.Empty;

            //List<Compania> lstCadena = new List<Compania>();
            //lstCadena = CadenaConexionETL_lst(id_compania);



            //UserID = lstCadena[0].STR_USUARIO_ETL;
            //Password = lstCadena[0].STR_CONTRASENIA_ETL;
            //Host = lstCadena[0].STR_HOST_COMPANIA;
            //Port = lstCadena[0].STR_PUERTO_COMPANIA;
            //DataBase = lstCadena[0].STR_BD_COMPANIA;

            /*Cadena de Postegres
            Cadena = "USER ID=" + UserID + ";" + "Password=" + Password + ";" + "Host=" + Host + ";" + "Port =" + Port + ";" + "DataBase=" + DataBase + ";" + "Pooling=true;";*/
            //conPETL = new NpgsqlConnection(Cadena);

            /*Cadena de SQL*/
            //Cadena = "Data Source =" + Host +","+Port+ ";" + "Initial Catalog=" + DataBase + ";" + "Persist Security Info=True;" + "User ID=" + UserID + ";Password=" + Password;
            //conSQLETL = new SqlConnection(Cadena);

            /// creacion de odbc 
            DSN dsn = new DSN();
            dsn = dsnConfig.crearDSN(id_compania);

            if (dsn.creado)
            {
                /// obtener conexion de Odbc creado 
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);


                try
                {
                    //string add = "SELECT[INT_IDBALANZA], [VARCHAR_CTA],"
                    //            + "[VARCHAR_SCTA], [VARCHAR_SSCTA],"
                    //            + "[INT_YEAR], [DECI_SALINI],"
                    //            + "[DECI_ENECARGOS], [DECI_ENEABONOS],"
                    //            + "[DECI_FEBCARGOS], [DECI_FEBABONOS],"
                    //            + "[DECI_MARCARGOS], [DECI_MARABONOS],"
                    //            + "[DECI_ABRCARGOS], [DECI_ABRABONOS],"
                    //            + "[DECI_MAYCARGOS], [DECI_MAYABONOS],"
                    //            + "[DECI_JUNCARGOS], [DECI_JUNABONOS],"
                    //            + "[DECI_JULCARGOS], [DECI_JULABONOS],"
                    //            + "[DECI_AGOCARGOS], [DECI_AGOABONOS],"
                    //            + "[DECI_SEPCARGOS], [DECI_SEPABONOS],"
                    //            + "[DECI_OCTCARGOS], [DECI_OCTABONOS],"
                    //            + "[DECI_NOVCARGOS], [DECI_NOVABONOS],"
                    //            + "[DECI_DICCARGOS], [DECI_DICABONOS],"
                    //            + "[INT_CC],"
                    //            + "[VARCHAR_DESCRIPCION],"
                    //            + "[VARCHAR_DESCRIPCION2],"
                    //            + "[DECI_INCLUIR_SUMA]"
                    //            + " FROM [TAB_BALANZA_SQL]";


                    //comSQLETL = new SqlCommand(add, conSQLETL);
                    //SqlDataAdapter da = new SqlDataAdapter(comSQLETL);
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);
                    //return dt;

                    ///regresar para emular sql /

                    //// eventual SYBASE GIA EN SITIO 
                    string consulta = " SELECT "
                                + "year,"
                                + "cta,"
                                + "scta,"
                                + "sscta,"
                                + "salini,"
                                + "enecargos,"
                                + "eneabonos,"
                                + "febcargos,"
                                + "febabonos,"
                                + "marcargos,"
                                + "marabonos,"
                                + "abrcargos,"
                                + "abrabonos,"
                                + "maycargos,"
                                + "mayabonos,"
                                + "juncargos,"
                                + "junabonos,"
                                + "julcargos,"
                                + "julabonos,"
                                + "agocargos,"
                                + "agoabonos,"
                                + "sepcargos,"
                                + "sepabonos,"
                                + "octcargos,"
                                + "octabonos,"
                                + "novcargos,"
                                + "novabonos,"
                                + "diccargos,"
                                + "dicabonos,"
                                + "cierreabonos,"
                                + "cierrecargos,"
                                + "acta,"
                                + "cc"
                                + " FROM sc_salcont_cc";


                    OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                    odbcCon.Open();
                    OdbcDataReader rdr = cmd.ExecuteReader();
                    List<ScSaldoConCc> listaSaldo = new List<ScSaldoConCc>();
                    while (rdr.Read())
                    {
                        ScSaldoConCc saldo = new ScSaldoConCc();
                        saldo.year = Convert.ToInt32(rdr["year"]);
                        saldo.cta = Convert.ToInt32(rdr["cta"]);
                        saldo.scta = Convert.ToInt32(rdr["scta"]);
                        saldo.sscta = Convert.ToInt32(rdr["sscta"]);
                        saldo.salini = Convert.ToInt32(rdr["salini"]);
                        saldo.enecargos = Convert.ToInt32(rdr["enecargos"]);
                        saldo.eneabonos = Convert.ToInt32(rdr["eneabonos"]);
                        saldo.febcargos = Convert.ToInt32(rdr["febcargos"]);
                        saldo.febabonos = Convert.ToInt32(rdr["febabonos"]);
                        saldo.marcargos = Convert.ToInt32(rdr["marcargos"]);
                        saldo.marabonos = Convert.ToInt32(rdr["marabonos"]);
                        saldo.abrcargos = Convert.ToInt32(rdr["abrcargos"]);
                        saldo.abrabonos = Convert.ToInt32(rdr["abrabonos"]);
                        saldo.maycargos = Convert.ToInt32(rdr["maycargos"]);
                        saldo.mayabonos = Convert.ToInt32(rdr["mayabonos"]);
                        saldo.juncargos = Convert.ToInt32(rdr["juncargos"]);
                        saldo.junabonos = Convert.ToInt32(rdr["junabonos"]);
                        saldo.julcargos = Convert.ToInt32(rdr["julcargos"]);
                        saldo.julabonos = Convert.ToInt32(rdr["julabonos"]);
                        saldo.agocargos = Convert.ToInt32(rdr["agocargos"]);
                        saldo.agoabonos = Convert.ToInt32(rdr["agoabonos"]);
                        saldo.sepcargos = Convert.ToInt32(rdr["sepcargos"]);
                        saldo.sepabonos = Convert.ToInt32(rdr["sepabonos"]);
                        saldo.octcargos = Convert.ToInt32(rdr["octcargos"]);
                        saldo.octabonos = Convert.ToInt32(rdr["octabonos"]);
                        saldo.novcargos = Convert.ToInt32(rdr["novcargos"]);
                        saldo.novabonos = Convert.ToInt32(rdr["novabonos"]);
                        saldo.diccargos = Convert.ToInt32(rdr["diccargos"]);
                        saldo.dicabonos = Convert.ToInt32(rdr["dicabonos"]);
                        saldo.cierreabonos = Convert.ToInt32(rdr["cierreabonos"]);
                        saldo.cierrecargos = Convert.ToInt32(rdr["cierrecargos"]);
                        saldo.acta = Convert.ToInt32(rdr["acta"]);
                        saldo.cc = Convert.ToString(rdr["cc"]);

                        listaSaldo.Add(saldo);

                    }

                    return listaSaldo;






                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw;
                }
                finally
                {
                    odbcCon.Close();
                    //// regresar emulacion sql 
                    //conSQLETL.Close();
                }
            }
            else
            {
                return null;
            }
        }



        public List<Balanza> obtenerSalContCCD(int id_compania)
        {

            try
            {
                /// creacion de odbc 
                DSN dsn = new DSN();
                dsn = dsnConfig.crearDSN(id_compania); //regresar
                if (dsn.creado) //regresar// if(true)
                {
                    /// obtener conexion de Odbc creado
                    /// 
                    //dsn.nombreDSN = "2_GRUPO_ INGENIERIA"; /// quitar 
                    //dsn.nombreDSN =  "4_CONSTRUCTORA_Y_EDIFICADORA";///quitar
                    odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);

                    try
                    {

                        string consulta = " SELECT "
                                    + "year,"
                                    + "cta,"
                                    + "scta,"
                                    + "sscta,"
                                    + "salini,"
                                    + "enecargos,"
                                    + "eneabonos,"
                                    + "febcargos,"
                                    + "febabonos,"
                                    + "marcargos,"
                                    + "marabonos,"
                                    + "abrcargos,"
                                    + "abrabonos,"
                                    + "maycargos,"
                                    + "mayabonos,"
                                    + "juncargos,"
                                    + "junabonos,"
                                    + "julcargos,"
                                    + "julabonos,"
                                    + "agocargos,"
                                    + "agoabonos,"
                                    + "sepcargos,"
                                    + "sepabonos,"
                                    + "octcargos,"
                                    + "octabonos,"
                                    + "novcargos,"
                                    + "novabonos,"
                                    + "diccargos,"
                                    + "dicabonos,"
                                    + "cierreabonos,"
                                    + "cierrecargos,"
                                    + "acta,"
                                    + "cc"
                                    + " FROM sc_salcont_cc";


                        OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                        odbcCon.Open();
                        OdbcDataReader rdr = cmd.ExecuteReader();
                        List<Balanza> listaBalanza = new List<Balanza>();
                        while (rdr.Read())
                        {
                            Balanza balanza = new Balanza();
                            balanza.year = Convert.ToInt32(rdr["year"]);
                            balanza.cta = Convert.ToString(rdr["cta"].ToString());
                            balanza.scta = Convert.ToString(rdr["scta"].ToString());
                            balanza.sscta = Convert.ToString(rdr["sscta"].ToString());
                            balanza.salini = Convert.ToDouble(rdr["salini"]);
                            balanza.enecargos = Convert.ToDouble(rdr["enecargos"]);
                            balanza.eneabonos = Convert.ToDouble(rdr["eneabonos"]);
                            balanza.febcargos = Convert.ToDouble(rdr["febcargos"]);
                            balanza.febabonos = Convert.ToDouble(rdr["febabonos"]);
                            balanza.marcargos = Convert.ToDouble(rdr["marcargos"]);
                            balanza.marabonos = Convert.ToDouble(rdr["marabonos"]);
                            balanza.abrcargos = Convert.ToDouble(rdr["abrcargos"]);
                            balanza.abrabonos = Convert.ToDouble(rdr["abrabonos"]);
                            balanza.maycargos = Convert.ToDouble(rdr["maycargos"]);
                            balanza.mayabonos = Convert.ToDouble(rdr["mayabonos"]);
                            balanza.juncargos = Convert.ToDouble(rdr["juncargos"]);
                            balanza.junabonos = Convert.ToDouble(rdr["junabonos"]);
                            balanza.julcargos = Convert.ToDouble(rdr["julcargos"]);
                            balanza.julabonos = Convert.ToDouble(rdr["julabonos"]);
                            balanza.agocargos = Convert.ToDouble(rdr["agocargos"]);
                            balanza.agoabonos = Convert.ToDouble(rdr["agoabonos"]);
                            balanza.sepcargos = Convert.ToDouble(rdr["sepcargos"]);
                            balanza.sepabonos = Convert.ToDouble(rdr["sepabonos"]);
                            balanza.octcargos = Convert.ToDouble(rdr["octcargos"]);
                            balanza.octabonos = Convert.ToDouble(rdr["octabonos"]);
                            balanza.novcargos = Convert.ToDouble(rdr["novcargos"]);
                            balanza.novabonos = Convert.ToDouble(rdr["novabonos"]);
                            balanza.diccargos = Convert.ToDouble(rdr["diccargos"]);
                            balanza.dicabonos = Convert.ToDouble(rdr["dicabonos"]);
                            balanza.cierre_abonos = Convert.ToDouble(rdr["cierreabonos"]);
                            balanza.cierre_cargos = Convert.ToDouble(rdr["cierrecargos"]);
                            balanza.acta = Convert.ToInt32(rdr["acta"]);
                            balanza.cc = Convert.ToString(rdr["cc"]);

                            listaBalanza.Add(balanza);

                        }

                        return listaBalanza;

                        ////OdbcCommand cmd = new OdbcCommand(consulta , odbcCon);
                        ////odbcCon.Open();
                        // OdbcDataReader rdr = cmd.ExecuteReader();
                        //OdbcDataAdapter da = new OdbcDataAdapter(cmdETL);
                        //DataTable dt = new DataTable();
                        //DataSet saldos = new DataSet();
                        //da.Fill(saldos,"sc_saldos_cc");

                        //return dt;


                        ////




                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        throw;
                    }
                    finally
                    {
                        odbcCon.Close();
                        //// regresar emulacion sql 
                        //conSQLETL.Close();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;

            }
        }

        public List<String> obtenerAniosPolizasSybase(int idCompania, string anioInicio, string anioFin)
        {
            odbcCon = conex.ConexionSybaseodbc("4_CONSTRUCTORA_Y_EDIFICADORA");
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

        public List<Semanal> obtenerReporteSemanalPolizasSybaseD(int idCompania, String anio)
        {

            //empresa 2  odbcCon = conex.ConexionSybaseodbc("2_GRUPO_ INGENIERIA");
            ///empresa 4
            odbcCon = conex.ConexionSybaseodbc("4_CONSTRUCTORA_Y_EDIFICADORA");
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

        public List<ETLSemanal> obtenerReporteSemanalPolizasSybase(int id_compania)
        {

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
                    + " and a.mes = b.mes"
                    + " and a.tp = b.tp"
                    + " and a.poliza = b.poliza WHERE a.year >0 And Status = 'A'";

                OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                odbcCon.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();

                List<ETLSemanal> listaEtlSem = new List<ETLSemanal>();
                while (rdr.Read())
                {
                    ETLSemanal etlSem = new ETLSemanal();
                    etlSem.year = Convert.ToInt32(rdr["year"]);
                    etlSem.mes = Convert.ToInt32(rdr["mes"]);
                    etlSem.poliza = Convert.ToInt32(rdr["poliza"]);
                    etlSem.tp = Convert.ToString(rdr["tp"]);
                    etlSem.linea = Convert.ToInt32(rdr["linea"]);
                    etlSem.cta = Convert.ToInt32(rdr["cta"]);
                    etlSem.scta = Convert.ToInt32(rdr["scta"]);
                    etlSem.sscta = Convert.ToInt32(rdr["sscta"]);
                    etlSem.concepto = Convert.ToString(rdr["concepto"]);
                    etlSem.monto = Convert.ToInt32(rdr["monto"]);
                    etlSem.folio_imp = Convert.ToString(rdr["folio_imp"].ToString());///
                    etlSem.itm = Convert.ToInt32(rdr["itm"]);
                    etlSem.tm = Convert.ToInt32(rdr["tm"]);
                    etlSem.NumProveedor = Convert.ToString(rdr["NumProveedor"].ToString());///
                    etlSem.CentroCostos = Convert.ToString(rdr["CentroCostos"]);
                    etlSem.referencia = Convert.ToString(rdr["referencia"]);
                    etlSem.orden_compra = Convert.ToString(rdr["orden_compra"].ToString());//
                    etlSem.fechapol = Convert.ToDateTime(rdr["fechapol"]);
                    etlSem.idEmpresa = Convert.ToInt32(rdr["idEmpresa"]);
                    etlSem.idVersion = Convert.ToInt32(rdr["idVersion"]);
                    etlSem.cfd_ruta_pdf = Convert.ToString(rdr["cfd_ruta_pdf"]);
                    etlSem.cfd_ruta_xml = Convert.ToString(rdr["cfd_ruta_xml"]);
                    etlSem.uuid = Convert.ToString(rdr["uuid"]);


                    listaEtlSem.Add(etlSem);

                }


                return listaEtlSem;

                ////OdbcCommand cmd = new OdbcCommand(consulta , odbcCon);
                ////odbcCon.Open();
                // OdbcDataReader rdr = cmd.ExecuteReader();
                //OdbcDataAdapter da = new OdbcDataAdapter(cmdETL);
                //DataTable dt = new DataTable();
                //DataSet saldos = new DataSet();
                //da.Fill(saldos,"sc_saldos_cc");

                //return dt;


                ////




            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
            finally
            {
                odbcCon.Close();
                //// regresar emulacion sql 
                //conSQLETL.Close();
            }
        }

        public List<Semanal> convertirReporteSemabalSybaseToPg(int id_compania)
        {
            List<Semanal> lstSemanal = new List<Semanal>();


            List<ETLSemanal> lstEtlSem = new List<ETLSemanal>();
            lstEtlSem = obtenerReporteSemanalPolizasSybase(id_compania);

            foreach (ETLSemanal etlSem in lstEtlSem)
            {
                Semanal semanal = new Semanal();

                semanal.year = etlSem.year;
                semanal.mes = etlSem.mes;
                semanal.poliza = etlSem.poliza;
                semanal.tp = etlSem.tp;
                semanal.linea = etlSem.linea;
                semanal.cta = etlSem.cta;
                semanal.scta = etlSem.scta;
                semanal.sscta = etlSem.sscta;
                semanal.concepto = etlSem.concepto;
                semanal.monto = etlSem.monto;
                semanal.folio_imp = etlSem.folio_imp;
                semanal.itm = etlSem.itm;
                semanal.tm = etlSem.tm;
                semanal.numpro = etlSem.NumProveedor;
                semanal.cc = etlSem.CentroCostos;
                semanal.referencia = etlSem.referencia;
                semanal.orden_compra = etlSem.orden_compra;
                semanal.fechapol = etlSem.fechapol.ToString("dd/MM/yyyy");
                semanal.id_empresa = etlSem.idEmpresa;
                semanal.id_version = etlSem.idVersion;
                semanal.cfd_ruta_pdf = etlSem.cfd_ruta_pdf;
                semanal.cfd_ruta_xml = etlSem.cfd_ruta_xml;
                semanal.uuid = etlSem.uuid;

                lstSemanal.Add(semanal);


            }

            return lstSemanal;
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

                            cmd.Parameters.AddWithValue("@NUM_YEAR", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.year);
                            cmd.Parameters.AddWithValue("@NUM_MES", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.mes);
                            cmd.Parameters.AddWithValue("@NUM_POLIZA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.poliza);
                            cmd.Parameters.AddWithValue("@TEXT_TP", NpgsqlTypes.NpgsqlDbType.Text, semmanal.tp);
                            cmd.Parameters.AddWithValue("@NUM_LINEA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.linea);
                            cmd.Parameters.AddWithValue("@NUM_CTA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.cta);
                            cmd.Parameters.AddWithValue("@NUM_SCTA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.scta);
                            cmd.Parameters.AddWithValue("@NUM_SSCTA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.sscta);
                            cmd.Parameters.AddWithValue("@TEXT_CONCEPTO", NpgsqlTypes.NpgsqlDbType.Text, semmanal.concepto);
                            cmd.Parameters.AddWithValue("@TEXT_MONTO", NpgsqlTypes.NpgsqlDbType.Text, semmanal.monto);
                            cmd.Parameters.AddWithValue("@TEXT_FOLIO_IMP", NpgsqlTypes.NpgsqlDbType.Text, semmanal.folio_imp);
                            cmd.Parameters.AddWithValue("@NUM_ITM", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.itm);
                            cmd.Parameters.AddWithValue("@NUM_TM", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.tm);
                            cmd.Parameters.AddWithValue("@TEXT_NUMPRO", NpgsqlTypes.NpgsqlDbType.Text, semmanal.numpro);
                            cmd.Parameters.AddWithValue("@TEXT_CC", NpgsqlTypes.NpgsqlDbType.Text, semmanal.cc);
                            cmd.Parameters.AddWithValue("@TEXT_REFERENCIA", NpgsqlTypes.NpgsqlDbType.Text, semmanal.referencia);
                            cmd.Parameters.AddWithValue("@TEXT_ORDEN_COMPRA", NpgsqlTypes.NpgsqlDbType.Text, semmanal.orden_compra);
                            cmd.Parameters.AddWithValue("@TEXT_FECHAPOL", NpgsqlTypes.NpgsqlDbType.Text, semmanal.fechapol);
                            cmd.Parameters.AddWithValue("@INT_IDEMPRESA", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.id_empresa);
                            cmd.Parameters.AddWithValue("@INT_IDVERSION", NpgsqlTypes.NpgsqlDbType.Integer, semmanal.id_version);
                            cmd.Parameters.AddWithValue("@TEXT_CFD_RUTA_PDF", NpgsqlTypes.NpgsqlDbType.Text, semmanal.cfd_ruta_pdf);
                            cmd.Parameters.AddWithValue("@TEXT_CFD_RUTA_XML", NpgsqlTypes.NpgsqlDbType.Text, semmanal.cfd_ruta_xml);
                            cmd.Parameters.AddWithValue("@TEXT_UUID", NpgsqlTypes.NpgsqlDbType.Text, semmanal.uuid);
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

        public List<Balanza> convertirTabBalanza(int id_compania)
        {
            List<Balanza> lstBalanza = new List<Balanza>();

            //DataTable dt = new DataTable();
            //dt = cadena_conexion_extrac(id_compania);
            //foreach (DataRow r in dt.Rows)
            //{
            //    Balanza balanza = new Balanza();
            //    balanza.INT_IDBALANZA = Convert.ToInt32(r["INT_IDBALANZA"]);
            //    balanza.TEXT_CTA = r["VARCHAR_CTA"].ToString();
            //    balanza.TEXT_SCTA = r["VARCHAR_SCTA"].ToString();
            //    balanza.TEXT_SSCTA = r["VARCHAR_SSCTA"].ToString();
            //    balanza.INT_YEAR = Convert.ToInt32(r["INT_YEAR"]);
            //    balanza.DECI_SALINI = Convert.ToDouble(r["DECI_SALINI"]);
            //    balanza.DECI_ENECARGOS = Convert.ToDouble(r["DECI_ENECARGOS"]);
            //    balanza.DECI_ENEABONOS = Convert.ToDouble(r["DECI_ENEABONOS"]);
            //    balanza.DECI_FEBCARGOS = Convert.ToDouble(r["DECI_FEBCARGOS"]);
            //    balanza.DECI_FEBABONOS = Convert.ToDouble(r["DECI_FEBABONOS"]);
            //    balanza.DECI_MARCARGOS = Convert.ToDouble(r["DECI_MARCARGOS"]);
            //    balanza.DECI_MARABONOS = Convert.ToDouble(r["DECI_MARABONOS"]);
            //    balanza.DECI_ABRCARGOS = Convert.ToDouble(r["DECI_ABRCARGOS"]);
            //    balanza.DECI_ABRABONOS = Convert.ToDouble(r["DECI_ABRABONOS"]);
            //    balanza.DECI_MAYCARGOS = Convert.ToDouble(r["DECI_ABRABONOS"]);
            //    balanza.DECI_MAYABONOS = Convert.ToDouble(r["DECI_MAYABONOS"]);
            //    balanza.DECI_JUNCARGOS = Convert.ToDouble(r["DECI_JUNCARGOS"]);
            //    balanza.DECI_JUNABONOS = Convert.ToDouble(r["DECI_JUNABONOS"]);
            //    balanza.DECI_JULCARGOS = Convert.ToDouble(r["DECI_JULCARGOS"]);
            //    balanza.DECI_JULABONOS = Convert.ToDouble(r["DECI_JULABONOS"]);
            //    balanza.DECI_AGOCARGOS = Convert.ToDouble(r["DECI_AGOCARGOS"]);
            //    balanza.DECI_AGOABONOS = Convert.ToDouble(r["DECI_AGOABONOS"]);
            //    balanza.DECI_SEPCARGOS = Convert.ToDouble(r["DECI_AGOABONOS"]);
            //    balanza.DECI_SEPABONOS = Convert.ToDouble(r["DECI_SEPABONOS"]);
            //    balanza.DECI_OCTCARGOS = Convert.ToDouble(r["DECI_OCTCARGOS"]);
            //    balanza.DECI_OCTABONOS = Convert.ToDouble(r["DECI_OCTABONOS"]);
            //    balanza.DECI_NOVCARGOS = Convert.ToDouble(r["DECI_NOVCARGOS"]);
            //    balanza.DECI_NOVABONOS = Convert.ToDouble(r["DECI_NOVABONOS"]);
            //    balanza.DECI_DICCARGOS = Convert.ToDouble(r["DECI_DICCARGOS"]);
            //    balanza.DECI_DICABONOS = Convert.ToDouble(r["DECI_DICABONOS"]);
            //    balanza.INT_CC = Convert.ToInt32(r["INT_CC"]);
            //    balanza.TEXT_DESCRIPCION = r["VARCHAR_DESCRIPCION"].ToString();
            //    balanza.TEXT_DESCRIPCION2 = r["VARCHAR_DESCRIPCION2"].ToString();
            //    balanza.INT_INCLUIR_SUMA = Convert.ToInt32(r["DECI_INCLUIR_SUMA"]);
            //    lstBalanza.Add(balanza);
            //}

            List<ScSaldoConCc> lstSaldoCC = new List<ScSaldoConCc>();
            lstSaldoCC = obtenerSalContCC(id_compania);

            foreach (ScSaldoConCc saldoCC in lstSaldoCC)
            {
                Balanza balanza = new Balanza();
                balanza.year = saldoCC.year;
                balanza.cta = saldoCC.cta.ToString();
                balanza.scta = saldoCC.scta.ToString();
                balanza.sscta = saldoCC.sscta.ToString();
                balanza.salini = saldoCC.salini;
                balanza.enecargos = saldoCC.enecargos;
                balanza.eneabonos = saldoCC.eneabonos;
                balanza.febabonos = saldoCC.febabonos;
                balanza.febcargos = saldoCC.febcargos;
                balanza.marabonos = saldoCC.marabonos;
                balanza.marcargos = saldoCC.marcargos;
                balanza.abrabonos = saldoCC.abrabonos;
                balanza.abrcargos = saldoCC.abrcargos;
                balanza.mayabonos = saldoCC.mayabonos;
                balanza.maycargos = saldoCC.maycargos;
                balanza.junabonos = saldoCC.junabonos;
                balanza.juncargos = saldoCC.juncargos;
                balanza.julabonos = saldoCC.julabonos;
                balanza.julcargos = saldoCC.julcargos;
                balanza.agoabonos = saldoCC.agoabonos;
                balanza.agocargos = saldoCC.agocargos;
                balanza.sepabonos = saldoCC.sepabonos;
                balanza.sepcargos = saldoCC.sepcargos;
                balanza.octabonos = saldoCC.octabonos;
                balanza.octcargos = saldoCC.octcargos;
                balanza.novabonos = saldoCC.novabonos;
                balanza.novcargos = saldoCC.novcargos;
                balanza.dicabonos = saldoCC.dicabonos;
                balanza.diccargos = saldoCC.diccargos;
                balanza.cierre_abonos = saldoCC.cierreabonos;
                balanza.cierre_cargos = saldoCC.cierrecargos;
                balanza.acta = saldoCC.acta;
                balanza.cc = saldoCC.cc;


                lstBalanza.Add(balanza);


            }


            return lstBalanza;
        }

        //public int buscarSaldoCC()
        //{


        //    string busqueda = "SELECT COUNT(*) FROM  SC_SALCONT_CC";
        //    try
        //    {
        //        AseCommand cmd = new AseCommand(busqueda, conSc);
        //        conSc.Open();
        //        int cantFilas = cmd.ExecuteNonQuery();
        //        conSc.Close();
        //        return cantFilas;
        //    }
        //    catch (Exception ex ) {
        //        string error = ex.Message;
        //        string trace = ex.StackTrace;
        //            return 0;
        //    }
        //}

        public int buscarSaldoCCODBC()
        {

            string numReg = "";
            string busqueda = "SELECT COUNT(*) AS CONTADOR FROM  SC_SALCONT_CC";
            try
            {
                OdbcCommand cmd = new OdbcCommand(busqueda, odbcCon);
                odbcCon.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    numReg = rdr["CONTADOR"].ToString().Trim();
                }
                // int cantFilas = cmd.ExecuteNonQuery();       
                int cantFilas = rdr.FieldCount;
                odbcCon.Close();

                return cantFilas;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                string trace = ex.StackTrace;
                return 0;
            }
        }



        public int insertarTabBalanza(int id_compania)
        {
            con.Open();
            DateTime fechaInicioProceso = DateTime.Now;
            var transaction = con.BeginTransaction();
            List<Balanza> lstBala = new List<Balanza>();


            try
            {
                lstBala = obtenerSalContCCD(id_compania);
                //lstBala = convertirTabBalanza(id_compania);

                string addBalanza = "insert into"
                 + cod + "balanza" + cod + "("
                 //+ cod + "idbalanza" + cod + ","
                 + cod + "cta" + cod + ","
                 + cod + "scta" + cod + ","
                 + cod + "sscta" + cod + ","
                 + cod + "year" + cod + ","
                 + cod + "salini" + cod + ","
                 + cod + "enecargos" + cod + ","
                 + cod + "eneabonos" + cod + ","
                 + cod + "febcargos" + cod + ","
                 + cod + "febabonos" + cod + ","
                 + cod + "marcargos" + cod + ","
                 + cod + "marabonos" + cod + ","
                 + cod + "abrcargos" + cod + ","
                 + cod + "abrabonos" + cod + ","
                 + cod + "maycargos" + cod + ","
                 + cod + "mayabonos" + cod + ","
                 + cod + "juncargos" + cod + ","
                 + cod + "junabonos" + cod + ","
                 + cod + "julcargos" + cod + ","
                 + cod + "julabonos" + cod + ","
                 + cod + "agocargos" + cod + ","
                 + cod + "agoabonos" + cod + ","
                 + cod + "sepcargos" + cod + ","
                 + cod + "sepabonos" + cod + ","
                 + cod + "octcargos" + cod + ","
                 + cod + "octabonos" + cod + ","
                 + cod + "novcargos" + cod + ","
                 + cod + "novabonos" + cod + ","
                 + cod + "diccargos" + cod + ","
                 + cod + "dicabonos" + cod + ","
                 //+ cod + "cc" + cod + ","
                 //+ cod + "descripcion" + cod + ","
                 //+ cod + "descripcion2" + cod + ","
                 + cod + "incluir_suma" + cod + ","
                 + cod + "tipo_extraccion" + cod + ","
                 + cod + "fech_extr" + cod + ","
                 + cod + "hora" + cod + ","
                 + cod + "id_empresa" + cod + ","
                 + cod + "cierre_cargos" + cod + ","
                 + cod + "cierre_abonos" + cod + ","
                 + cod + "acta" + cod + ","
                 + cod + "cc" + cod + ")"


                     + "values "
                         //+ "(@IDBALANZA,"
                         + "(@CTA,"
                         + "@SCTA,"
                         + "@SSCTA,"
                         + "@YEAR,"
                         + "@SALINI,"
                         + "@ENECARGOS,"
                         + "@ENEABONOS,"
                         + "@FEBCARGOS,"
                         + "@FEBABONOS,"
                         + "@MARCARGOS,"
                         + "@MARABONOS,"
                         + "@ABRCARGOS,"
                         + "@ABRABONOS,"
                         + "@MAYCARGOS,"
                         + "@MAYABONOS,"
                         + "@JUNCARGOS,"
                         + "@JUNABONOS,"
                         + "@JULCARGOS,"
                         + "@JULABONOS,"
                         + "@AGOCARGOS,"
                         + "@AGOABONOS,"
                         + "@SEPCARGOS,"
                         + "@SEPABONOS,"
                         + "@OCTCARGOS,"
                         + "@OCTABONOS,"
                         + "@NOVCARGOS,"
                         + "@NOVABONOS,"
                         + "@DICCARGOS,"
                         + "@DICABONOS,"
                         //+ "@CC,"
                         //+ "@DESCRIPCION,"
                         //+ "@DESCRIPCION2,"
                         + "@INCLUIR_SUMA,"
                         + "@TIPO_EXTRACCION,"
                         + "@FECH_EXTR,"
                         + "@HORA,"
                         + "@ID_EMPRESA,"
                         + "@CIERRE_CARGOS,"
                         + "@CIERRE_ABONOS,"
                         + "@ACTA,"
                         + "@CC)";

                {

                    int cantFilaAfect = 0;
                    foreach (Balanza balanza in lstBala)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(addBalanza, con);
                        //cmd.Parameters.AddWithValue("@INT_IDBALANZA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_IDBALANZA);
                        cmd.Parameters.AddWithValue("@CTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.cta);
                        cmd.Parameters.AddWithValue("@SCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.scta);
                        cmd.Parameters.AddWithValue("@SSCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.sscta);
                        cmd.Parameters.AddWithValue("@YEAR", NpgsqlTypes.NpgsqlDbType.Integer, balanza.year);
                        cmd.Parameters.AddWithValue("@SALINI", NpgsqlTypes.NpgsqlDbType.Double, balanza.salini);
                        cmd.Parameters.AddWithValue("@ENECARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.enecargos);
                        cmd.Parameters.AddWithValue("@ENEABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.eneabonos);
                        cmd.Parameters.AddWithValue("@FEBCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.febcargos);
                        cmd.Parameters.AddWithValue("@FEBABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.febabonos);
                        cmd.Parameters.AddWithValue("@MARCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.marcargos);
                        cmd.Parameters.AddWithValue("@MARABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.marabonos);
                        cmd.Parameters.AddWithValue("@ABRCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.abrcargos);
                        cmd.Parameters.AddWithValue("@ABRABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.abrabonos);
                        cmd.Parameters.AddWithValue("@MAYCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.maycargos);
                        cmd.Parameters.AddWithValue("@MAYABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.mayabonos);
                        cmd.Parameters.AddWithValue("@JUNCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.juncargos);
                        cmd.Parameters.AddWithValue("@JUNABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.junabonos);
                        cmd.Parameters.AddWithValue("@JULCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.julcargos);
                        cmd.Parameters.AddWithValue("@JULABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.julabonos);
                        cmd.Parameters.AddWithValue("@AGOCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.agocargos);
                        cmd.Parameters.AddWithValue("@AGOABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.agoabonos);
                        cmd.Parameters.AddWithValue("@SEPCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.sepcargos);
                        cmd.Parameters.AddWithValue("@SEPABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.sepabonos);
                        cmd.Parameters.AddWithValue("@OCTCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.octcargos);
                        cmd.Parameters.AddWithValue("@OCTABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.octabonos);
                        cmd.Parameters.AddWithValue("@NOVCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.novcargos);
                        cmd.Parameters.AddWithValue("@NOVABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.novabonos);
                        cmd.Parameters.AddWithValue("@DICCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.diccargos);
                        cmd.Parameters.AddWithValue("@DICABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.dicabonos);
                        //cmd.Parameters.AddWithValue("@CC", NpgsqlTypes.NpgsqlDbType.Integer, balanza.CC);
                        //cmd.Parameters.AddWithValue("@DESCRIPCION", NpgsqlTypes.NpgsqlDbType.Text, balanza.DESCRIPCION);
                        //cmd.Parameters.AddWithValue("@DESCRIPCION2", NpgsqlTypes.NpgsqlDbType.Text, balanza.DESCRIPCION2);
                        cmd.Parameters.AddWithValue("@INCLUIR_SUMA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.incluir_suma);
                        cmd.Parameters.AddWithValue("@TIPO_EXTRACCION", NpgsqlTypes.NpgsqlDbType.Integer, 1);
                        cmd.Parameters.AddWithValue("@FECH_EXTR", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.AddWithValue("@HORA", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("h:mm tt"));
                        cmd.Parameters.AddWithValue("@ID_EMPRESA", NpgsqlTypes.NpgsqlDbType.Integer, id_compania);
                        cmd.Parameters.AddWithValue("@CIERRE_CARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.cierre_cargos);
                        cmd.Parameters.AddWithValue("@CIERRE_ABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.cierre_abonos);
                        cmd.Parameters.AddWithValue("@ACTA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.acta);
                        cmd.Parameters.AddWithValue("@CC", NpgsqlTypes.NpgsqlDbType.Text, balanza.cc);



                        //con.Open();
                        // int cantFilaAfect = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cantFilaAfect = cantFilaAfect + Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                    transaction.Commit();
                    con.Close();
                    DateTime fechaFinalProceso = DateTime.Now;
                    configCorreo.EnviarCorreo("La extracción de Balanza se genero correctamente"
                                               + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " + fechaFinalProceso
                                               + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                               , "ETL Balanza");
                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                DateTime fechaFinalProceso = DateTime.Now;
                configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
                                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                                           , "ETL Balanza");
                string error = ex.Message;
                throw;
            }
        }
    }
}
