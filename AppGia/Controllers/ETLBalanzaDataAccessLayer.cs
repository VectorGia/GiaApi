using AppGia.Models;
using AppGia.Util;
using AppGia.Util.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class ETLBalanzaDataAccessLayer 
    {
        ConfigCorreoController configCorreo = new ConfigCorreoController();
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion(); 
        NpgsqlCommand comP = new NpgsqlCommand();

        OdbcConnection odbcCon;
        OdbcCommand cmdETL = new OdbcCommand();

        DSNConfig dsnConfig = new DSNConfig();

        SqlConnection conSQLETL = new SqlConnection();
        SqlCommand comSQLETL = new SqlCommand();

        public ETLBalanzaDataAccessLayer()
        {
            con = conex.ConnexionDB();
            //con.Open();
            //conSc=conex.ConexionSybase();
            //odbcCon = conex.ConexionSybaseodbc();


        }

        public DataTable EmpresaConexionETL(Int64 idEmpresa)
        {
            string add = "SELECT  id ,"
                    + "  usuario_etl ,"
                    + " nombre ,"
                    + " contrasenia_etl ,"
                    + " host ,"
                    + " puerto_compania ,"
                    + " bd_name ,"
                    + " contra_bytes ,"
                    + " llave ,"
                    + " apuntador "
                    + " FROM empresa" 
                    + " WHERE  id  = " + idEmpresa;
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

        public List<Empresa> EmpresaConexionETL_List(Int64 idEmpresa)
        {
            List<Empresa> lst = new List<Empresa>();
            DataTable dt = new DataTable();
            dt = EmpresaConexionETL(idEmpresa);

            foreach (DataRow r in dt.Rows)
            {
                Empresa cia = new Empresa();
                cia.usuario_etl = r["usuario_etl"].ToString();
                cia.contrasenia_etl = r["contrasenia_etl"].ToString();
                cia.host = r["host"].ToString();
                cia.puerto_compania = Convert.ToInt32(r["puerto_compania"]);
                cia.bd_name = r["bd_name"].ToString();
                cia.id = Convert.ToInt32(r["id"]);
                cia.nombre = Convert.ToString(r["nombre"]);
                cia.contra_bytes = r["contra_bytes"] as byte[];
                cia.llave = r["llave"] as byte[];
                cia.apuntador = r["apuntador"] as byte[];
                lst.Add(cia);
            }
            return lst;
        }

        public List<Balanza> obtenerSalContCCD(Int64 idEmpresa)
        {

            try
            {
                /// creacion de odbc 
                DSN dsn = new DSN(); 
                dsn = dsnConfig.crearDSN(idEmpresa); //regresar
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
            finally {
                odbcCon.Close();
            }
        }


        public int generarBalanza(List<Balanza> lstBala,Int64 idEmpresa) {
            con.Open();
            var transaction = con.BeginTransaction();
            int cantFilaAfect = 0;
            try
            {

                string addBalanza = "insert into "
                 + "balanza("
                 + "id,"
                 + "cta,"
                 + "scta,"
                 + "sscta,"
                 + "year,"
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
                 //+"cc,"
                 //+"descripcion,"
                 //+"descripcion2,"
                 + "incluir_suma,"
                 + "tipo_extraccion,"
                 + "fecha_carga,"
                 + "hora_carga,"
                 + "id_empresa,"
                 + "cierre_cargos,"
                 + "cierre_abonos,"
                 + "acta,"
                 + "cc" + ")"
                     + "values "
                         + "(NEXTVAL('seq_balanza'),"
                         + "@CTA,"
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
                         + "@FECHA_CARGA,"
                         + "@HORA_CARGA,"
                         + "@ID_EMPRESA,"
                         + "@CIERRE_CARGOS,"
                         + "@CIERRE_ABONOS,"
                         + "@ACTA,"
                         + "@CC)";

                {
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
                        cmd.Parameters.AddWithValue("@TIPO_EXTRACCION", NpgsqlTypes.NpgsqlDbType.Integer, Constantes.C_EXTRACCION_MANUAL);
                        ////cmd.Parameters.AddWithValue("@FECHA_CARGA", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.AddWithValue("@FECHA_CARGA", NpgsqlTypes.NpgsqlDbType.Date, DateTime.Now);
                        //cmd.Parameters.AddWithValue("@HORA_CARGA", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("h:mm tt"));
                        cmd.Parameters.AddWithValue("@HORA_CARGA", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("h:mm tt"));
                        cmd.Parameters.AddWithValue("@ID_EMPRESA", NpgsqlTypes.NpgsqlDbType.Bigint, idEmpresa);
                        cmd.Parameters.AddWithValue("@CIERRE_CARGOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.cierre_cargos);
                        cmd.Parameters.AddWithValue("@CIERRE_ABONOS", NpgsqlTypes.NpgsqlDbType.Double, balanza.cierre_abonos);
                        cmd.Parameters.AddWithValue("@ACTA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.acta);
                        cmd.Parameters.AddWithValue("@CC", NpgsqlTypes.NpgsqlDbType.Text, balanza.cc);

                        //con.Open();
                        // int cantFilaAfect = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cantFilaAfect = cantFilaAfect + Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                    transaction.Commit();

                    //////con.Close();
                    //////DateTime fechaFinalProceso = DateTime.Now;
                    //////configCorreo.EnviarCorreo("La extracción de Balanza se genero correctamente"
                    //////                           + "\nFecha Inicio : " + fechaInicioProceso + " \n Fecha Final: " + fechaFinalProceso
                    //////                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                    //////                           , "ETL Balanza");
                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                con.Close();
                //DateTime fechaFinalProceso = DateTime.Now;
                //configCorreo.EnviarCorreo("Ha ocurrido un error en la extracción de Balanza"
                //                           + "\nFecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                //                           + "\nTiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                //                           + "\nError : " + ex.Message
                //                           , "ETL Balanza");
                string error = ex.Message;
                throw;
            }
            finally {
                con.Close();
            }

            //return cantFilaAfect;
        }

        public int insertarBalanza(Int64 idEmpresa)
        {
            con.Open();
            DateTime fechaInicioProceso = DateTime.Now;
            var transaction = con.BeginTransaction();
            List<Balanza> lstBala = new List<Balanza>();

            try
            {
                lstBala = obtenerSalContCCD(idEmpresa);
                //lstBala = convertirTabBalanza(id_compania);

                string addBalanza = "insert into"
                 + " balanza("
                 //+" id,"
                 + "cta,"
                 + "scta,"
                 + "sscta,"
                 + "year,"
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
                 //+"cc,"
                 //+"descripcion,"
                 //+"descripcion2,"
                 + "incluir_suma,"
                 + "tipo_extraccion,"
                 + "fech_extr,"
                 + "hora,"
                 + "id_empresa,"
                 + "cierre_cargos,"
                 + "cierre_abonos,"
                 + "acta,"
                 + "cc" + ")"
                     + "values ("
                        // + "nextval('seq_balanza'),"
                         + "@CTA,"
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
                        cmd.Parameters.AddWithValue("@TIPO_EXTRACCION", NpgsqlTypes.NpgsqlDbType.Integer, Constantes.C_EXTRACCION_MANUAL);
                        cmd.Parameters.AddWithValue("@FECH_EXTR", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("dd/MM/yyyy"));
                        cmd.Parameters.AddWithValue("@HORA", NpgsqlTypes.NpgsqlDbType.Text, DateTime.Now.ToString("h:mm tt"));
                        cmd.Parameters.AddWithValue("@ID_EMPRESA", NpgsqlTypes.NpgsqlDbType.Bigint, idEmpresa);
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
                    configCorreo.EnviarCorreo(" La extracción de Balanza se genero correctamente"
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
                configCorreo.EnviarCorreo(" Ha ocurrido un error en la extracción de Balanza"
                                           + "\n Fecha Inicio : " + fechaInicioProceso + "\nFecha Final: " + fechaFinalProceso
                                           + "\n Tiempo de ejecucion : " + (fechaFinalProceso - fechaInicioProceso).TotalMinutes + " mins"
                                           + "\nError : " + ex.Message
                                           , "ETL Balanza");
                string error = ex.Message;
                throw;
            }
            finally 
            {
                con.Close();
            }
        }


        public string generarSalContCC_CSV(Int64 idEmpresa,string ruta)
        {
            string nombreArchivo = string.Empty;
            string registros = string.Empty;
            string cabecera = string.Empty;
            nombreArchivo = "BalanzaExport" + idEmpresa + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmSS") +".csv";
            StreamWriter layout;
            //layout = File.AppendText(@"C:\Users\Omnisys\Desktop\txtWinConnector\" + "cvsBalanza"+idEmpresa+DateTime.Now + ".csv");
            layout = File.AppendText(ruta + nombreArchivo);
             
            try
            {
                /// creacion de odbc 
                DSN dsn = new DSN();
                dsn = dsnConfig.crearDSN(idEmpresa); //regresar
                if (dsn.creado) //regresar// if(true)
                {
                    /// obtener conexion de Odbc creado
                    /// 
                    //dsn.nombreDSN = "2_GRUPO_ INGENIERIA"; /// quitar 
                    //dsn.nombreDSN =  "4_CONSTRUCTORA_Y_EDIFICADORA";///quitar
                    odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);

                    try
                    {
                        odbcCon.Open();
                        layout.WriteLine(Constantes.headerBalanzaCSV);

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
                        
                        OdbcDataReader rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {

                            registros = 
                             Convert.ToString(rdr["cta"].ToString()) + ","
                            + Convert.ToString(rdr["scta"].ToString()) + ","
                            + Convert.ToString(rdr["sscta"].ToString()) + ","
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
                            + Convert.ToDouble(rdr["dicabonos"]) + ","
                            + Constantes.C_EXTRACCION_MANUAL + ","
                            + idEmpresa + ","
                            + Convert.ToDouble(rdr["cierrecargos"]) + ","
                            + Convert.ToDouble(rdr["cierreabonos"]) + ","
                            + Convert.ToInt32(rdr["acta"]) + ","
                            + Convert.ToString(rdr["cc"]) + ","
                            + "'" + DateTime.Now.ToString("HH:mm:ss") + "'"
                            +"'" + DateTime.Now.ToString("dd/MM/yyy") + "',";

                            layout.WriteLine(registros, Environment.NewLine);

                        }

                        layout.Close();
                        odbcCon.Close();
                        return nombreArchivo;

                    }
                    catch (Exception ex)
                    {
                        string error = ex.Message;
                        layout.Close();
                        odbcCon.Close();
                        throw;
                    }
                    finally
                    {
                        layout.Close();
                        odbcCon.Close();

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
            finally {
                layout.Close();
                odbcCon.Close();
            }
        }

        public int copy_balanza(string nombre_archivo, string ruta_archivo)
        {
            int resultado;
            string script_copy = string.Empty;
            
            // pruebas 
            //script_copy = " copy tmp_balanza (" + Constantes.headerBalanzaCSV + ") from '" + ruta_archivo + nombre_archivo + "'" + " delimiter ',' csv header ";
            script_copy = " copy balanza (" + Constantes.headerBalanzaCSV + ") from '" + ruta_archivo + nombre_archivo + "'" + " delimiter ',' csv header ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(script_copy, con);
                resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                con.Close();
                throw;
            }
            finally {
                con.Close();
            }

            return resultado;
        }

        public int UpdateCuentaUnificada(Int64 idEmpresa)
        {
            string update = "   update balanza "
                            + " set "
                            + " cuenta_unificada=LPAD(cta,4,'0')||LPAD(scta,4,'0')||LPAD(sscta,4,'0') " 
                            + " where id = " + idEmpresa
                            +"  and cuenta_unificada is null";

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally {
                con.Close();
            }
        }


    }
}
