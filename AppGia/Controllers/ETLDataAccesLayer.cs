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
using AppGia.Util.Models;

namespace AppGia.Controllers
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



        public ETLDataAccesLayer() {
            con = conex.ConnexionDB();
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

            if (dsn.creado) { 
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
            else{
                return null;
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

                semanal.NUM_YEAR = etlSem.year;
                semanal.NUM_MES = etlSem.mes;
                semanal.NUM_POLIZA = etlSem.poliza;
                semanal.TEXT_TP = etlSem.tp;
                semanal.NUM_LINEA = etlSem.linea;
                semanal.NUM_CTA = etlSem.cta;
                semanal.NUM_SCTA = etlSem.scta;
                semanal.NUM_SSCTA = etlSem.sscta;
                semanal.TEXT_CONCEPTO = etlSem.concepto;
                semanal.TEXT_MONTO = etlSem.monto.ToString();
                semanal.TEXT_FOLIO_IMP = etlSem.folio_imp;
                semanal.NUM_ITM = etlSem.itm;
                semanal.NUM_TM = etlSem.tm;
                semanal.TEXT_NUMPRO = etlSem.NumProveedor;
                semanal.TEXT_CC = etlSem.CentroCostos;
                semanal.TEXT_REFERENCIA = etlSem.referencia;
                semanal.TEXT_ORDEN_COMPRA = etlSem.orden_compra;
                semanal.TEXT_FECHAPOL = etlSem.fechapol.ToString("dd/MM/yyyy");
                semanal.INT_IDEMPRESA = etlSem.idEmpresa;
                semanal.INT_IDVERSION = etlSem.idVersion;
                semanal.TEXT_CFD_RUTA_PDF = etlSem.cfd_ruta_pdf;
                semanal.TEXT_CFD_RUTA_XML = etlSem.cfd_ruta_xml;
                semanal.TEXT_UUID = etlSem.uuid;

                lstSemanal.Add(semanal);


            }

            return lstSemanal;
        }

        public int insertarReporteSemanalPg(int id_compania) {
            List<Semanal> lstSemanal = new List<Semanal>();
            lstSemanal = convertirReporteSemabalSybaseToPg(id_compania);
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
                        + cod + "TEXT_UUID" +cod+")"

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
            try
            {
                {
                    int cantFilaAfect = 0;
                    foreach (Semanal semmanal in lstSemanal)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(insercion, con);
                        //cmd.Parameters.AddWithValue("@INT_IDBALANZA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_IDBALANZA);
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
                    configCorreo.EnviarCorreo("La extracción Semanal se genero correctamente", "ETL Reporte Semanal");
                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                configCorreo.EnviarCorreo("La extracción Semanal se genero incorrectamente", "ETL Reporte Semanal");
                string error = ex.Message;
                throw;
            }




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

            foreach (ScSaldoConCc saldoCC in lstSaldoCC) {
                Balanza balanza = new Balanza();
                balanza.INT_YEAR = saldoCC.year;
                balanza.TEXT_CTA = saldoCC.cta.ToString();
                balanza.TEXT_SCTA = saldoCC.scta.ToString();
                balanza.TEXT_SSCTA = saldoCC.sscta.ToString();
                balanza.DECI_SALINI = saldoCC.salini;
                balanza.DECI_ENECARGOS = saldoCC.enecargos;
                balanza.DECI_ENEABONOS = saldoCC.eneabonos;
                balanza.DECI_FEBABONOS = saldoCC.febabonos;
                balanza.DECI_FEBCARGOS = saldoCC.febcargos;
                balanza.DECI_MARABONOS = saldoCC.marabonos;
                balanza.DECI_MARCARGOS = saldoCC.marcargos;
                balanza.DECI_ABRABONOS = saldoCC.abrabonos;
                balanza.DECI_ABRCARGOS = saldoCC.abrcargos;
                balanza.DECI_MAYABONOS = saldoCC.mayabonos;
                balanza.DECI_MAYCARGOS = saldoCC.maycargos;
                balanza.DECI_JUNABONOS = saldoCC.junabonos;
                balanza.DECI_JUNCARGOS = saldoCC.juncargos;
                balanza.DECI_JULABONOS = saldoCC.julabonos;
                balanza.DECI_JULCARGOS = saldoCC.julcargos;
                balanza.DECI_AGOABONOS = saldoCC.agoabonos;
                balanza.DECI_AGOCARGOS = saldoCC.agocargos;
                balanza.DECI_SEPABONOS = saldoCC.sepabonos;
                balanza.DECI_SEPCARGOS = saldoCC.sepcargos;
                balanza.DECI_OCTABONOS = saldoCC.octabonos;
                balanza.DECI_OCTCARGOS = saldoCC.octcargos;
                balanza.DECI_NOVABONOS = saldoCC.novabonos;
                balanza.DECI_NOVCARGOS = saldoCC.novcargos;
                balanza.DECI_DICABONOS = saldoCC.dicabonos;
                balanza.DECI_DICCARGOS = saldoCC.diccargos;
                balanza.TEXT_DESCRIPCION = "";
                balanza.TEXT_DESCRIPCION2 = "";
                balanza.DECI_CIERRE_ABONOS = saldoCC.cierreabonos;
                balanza.DECI_CIERRE_CARGOS = saldoCC.cierrecargos;
                balanza.INT_ACTA = saldoCC.acta;
                balanza.TEXT_CC = saldoCC.cc;


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
                while (rdr.Read()) {

                   numReg= rdr["CONTADOR"].ToString().Trim();
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
            List<Balanza> lstBala = new List<Balanza>();
            lstBala = convertirTabBalanza(id_compania);

                string addBalanza = "INSERT INTO"
                + cod + "TAB_BALANZA" + cod + "("
                //+ cod + "INT_IDBALANZA" + cod + ","
                + cod + "TEXT_CTA" + cod + ","
                + cod + "TEXT_SCTA" + cod + ","
                + cod + "TEXT_SSCTA" + cod + ","
                + cod + "INT_YEAR" + cod + ","
                + cod + "DECI_SALINI" + cod + ","
                + cod + "DECI_ENECARGOS" + cod + ","
                + cod + "DECI_ENEABONOS" + cod + ","
                + cod + "DECI_FEBCARGOS" + cod + ","
                + cod + "DECI_FEBABONOS" + cod + ","
                + cod + "DECI_MARCARGOS" + cod + ","
                + cod + "DECI_MARABONOS" + cod + ","
                + cod + "DECI_ABRCARGOS" + cod + ","
                + cod + "DECI_ABRABONOS" + cod + ","
                + cod + "DECI_MAYCARGOS" + cod + ","
                + cod + "DECI_MAYABONOS" + cod + ","
                + cod + "DECI_JUNCARGOS" + cod + ","
                + cod + "DECI_JUNABONOS" + cod + ","
                + cod + "DECI_JULCARGOS" + cod + ","
                + cod + "DECI_JULABONOS" + cod + ","
                + cod + "DECI_AGOCARGOS" + cod + ","
                + cod + "DECI_AGOABONOS" + cod + ","
                + cod + "DECI_SEPCARGOS" + cod + ","
                + cod + "DECI_SEPABONOS" + cod + ","
                + cod + "DECI_OCTCARGOS" + cod + ","
                + cod + "DECI_OCTABONOS" + cod + ","
                + cod + "DECI_NOVCARGOS" + cod + ","
                + cod + "DECI_NOVABONOS" + cod + ","
                + cod + "DECI_DICCARGOS" + cod + ","
                + cod + "DECI_DICABONOS" + cod + ","
                + cod + "INT_CC" + cod + ","
                + cod + "TEXT_DESCRIPCION" + cod + ","
                + cod + "TEXT_DESCRIPCION2" + cod + ","
                + cod + "INT_INCLUIR_SUMA" + cod + ","
                + cod + "INT_TIPO_EXTRACCION" + cod + ","
                + cod+  "TEXT_FECH_EXTR" + cod + ","
                + cod + "TEXT_HORA" + cod + ","
                + cod + "INT_ID_EMPRESA" + cod + ","
                + cod + "DECI_CIERRE_CARGOS" + cod + ","
                + cod + "DECI_CIERRE_ABONOS" + cod + ","
                + cod + "INT_ACTA" + cod + ","
                + cod + "TEXT_CC" + cod + ")"


                    + "VALUES "
                        //+ "(@INT_IDBALANZA,"
                        + "(@TEXT_CTA,"
                        + "@TEXT_SCTA,"
                        + "@TEXT_SSCTA,"
                        + "@INT_YEAR,"
                        + "@DECI_SALINI,"
                        + "@DECI_ENECARGOS,"
                        + "@DECI_ENEABONOS,"
                        + "@DECI_FEBCARGOS,"
                        + "@DECI_FEBABONOS,"
                        + "@DECI_MARCARGOS,"
                        + "@DECI_MARABONOS,"
                        + "@DECI_ABRCARGOS,"
                        + "@DECI_ABRABONOS,"
                        + "@DECI_MAYCARGOS,"
                        + "@DECI_MAYABONOS,"
                        + "@DECI_JUNCARGOS,"
                        + "@DECI_JUNABONOS,"
                        + "@DECI_JULCARGOS,"
                        + "@DECI_JULABONOS,"
                        + "@DECI_AGOCARGOS,"
                        + "@DECI_AGOABONOS,"
                        + "@DECI_SEPCARGOS,"
                        + "@DECI_SEPABONOS,"
                        + "@DECI_OCTCARGOS,"
                        + "@DECI_OCTABONOS,"
                        + "@DECI_NOVCARGOS,"
                        + "@DECI_NOVABONOS,"
                        + "@DECI_DICCARGOS,"
                        + "@DECI_DICABONOS,"
                        + "@INT_CC,"
                        + "@TEXT_DESCRIPCION,"
                        + "@TEXT_DESCRIPCION2,"
                        + "@INT_INCLUIR_SUMA,"
                        + "@INT_TIPO_EXTRACCION,"
                        + "@TEXT_FECH_EXTR,"
                        + "@TEXT_HORA,"
                        + "@INT_ID_EMPRESA,"
                        + "@DECI_CIERRE_CARGOS,"
                        + "@DECI_CIERRE_ABONOS,"
                        + "@INT_ACTA,"
                        + "@TEXT_CC)";

            try
            {
                {
                    int cantFilaAfect =0;
                    foreach (Balanza balanza in lstBala) {
                        NpgsqlCommand cmd = new NpgsqlCommand(addBalanza, con);
                        //cmd.Parameters.AddWithValue("@INT_IDBALANZA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_IDBALANZA);
                        cmd.Parameters.AddWithValue("@TEXT_CTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_CTA);
                        cmd.Parameters.AddWithValue("@TEXT_SCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_SCTA);
                        cmd.Parameters.AddWithValue("@TEXT_SSCTA", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_SSCTA);
                        cmd.Parameters.AddWithValue("@INT_YEAR", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_YEAR);
                        cmd.Parameters.AddWithValue("@DECI_SALINI", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_SALINI);
                        cmd.Parameters.AddWithValue("@DECI_ENECARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ENECARGOS);
                        cmd.Parameters.AddWithValue("@DECI_ENEABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ENEABONOS);
                        cmd.Parameters.AddWithValue("@DECI_FEBCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_FEBCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_FEBABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_FEBABONOS);
                        cmd.Parameters.AddWithValue("@DECI_MARCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MARCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_MARABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MARABONOS);
                        cmd.Parameters.AddWithValue("@DECI_ABRCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ABRCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_ABRABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_ABRABONOS);
                        cmd.Parameters.AddWithValue("@DECI_MAYCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MAYCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_MAYABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_MAYABONOS);
                        cmd.Parameters.AddWithValue("@DECI_JUNCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JUNCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_JUNABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JUNABONOS);
                        cmd.Parameters.AddWithValue("@DECI_JULCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JULCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_JULABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_JULABONOS);
                        cmd.Parameters.AddWithValue("@DECI_AGOCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_AGOCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_AGOABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_AGOABONOS);
                        cmd.Parameters.AddWithValue("@DECI_SEPCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_SEPCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_SEPABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_SEPABONOS);
                        cmd.Parameters.AddWithValue("@DECI_OCTCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_OCTCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_OCTABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_OCTABONOS);
                        cmd.Parameters.AddWithValue("@DECI_NOVCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_NOVCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_NOVABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_NOVABONOS);
                        cmd.Parameters.AddWithValue("@DECI_DICCARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_DICCARGOS);
                        cmd.Parameters.AddWithValue("@DECI_DICABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_DICABONOS);
                        cmd.Parameters.AddWithValue("@INT_CC", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_CC);
                        cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_DESCRIPCION);
                        cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION2", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_DESCRIPCION2);
                        cmd.Parameters.AddWithValue("@INT_INCLUIR_SUMA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_INCLUIR_SUMA);
                        cmd.Parameters.AddWithValue("@INT_TIPO_EXTRACCION", NpgsqlTypes.NpgsqlDbType.Integer, 1);
                        cmd.Parameters.AddWithValue("@TEXT_FECH_EXTR", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.AddWithValue("@TEXT_HORA", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("h:mm tt"));
                        cmd.Parameters.AddWithValue("@INT_ID_EMPRESA", NpgsqlTypes.NpgsqlDbType.Integer, id_compania);
                        cmd.Parameters.AddWithValue("@DECI_CIERRE_CARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_CIERRE_CARGOS);
                        cmd.Parameters.AddWithValue("@DECI_CIERRE_ABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.DECI_CIERRE_ABONOS);
                        cmd.Parameters.AddWithValue("@INT_ACTA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_ACTA);
                        cmd.Parameters.AddWithValue("@TEXT_CC", NpgsqlTypes.NpgsqlDbType.Text, balanza.TEXT_CC);



                        //conP.Open();
                        // int cantFilaAfect = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cantFilaAfect = cantFilaAfect + Convert.ToInt32(cmd.ExecuteNonQuery()); 
                    }
                    
                    con.Close();
                    configCorreo.EnviarCorreo("La extracción para se genero correctamente", "ETL");
                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                configCorreo.EnviarCorreo("La extracción para se genero incorrectamente", "ETL");
                string error = ex.Message;
                throw;
            }
        }
    }
}
