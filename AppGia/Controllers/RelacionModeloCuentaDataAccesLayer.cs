using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class RelacionModeloCuentaDataAccesLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = '"';

        public RelacionModeloCuentaDataAccesLayer() {

            con = conex.ConnexionDB();
        }


        public IEnumerable<RelacionModeloCta> GetAllRelacionModeloCta() {

            RelacionModeloCta relacionModeloCta = new RelacionModeloCta();
            string cadena = "SELECT *FROM " + cod + "TAB_RELMODELO_CTA" + cod;
            try
            {
                List<RelacionModeloCta> lstRelacionModeloCta = new List<RelacionModeloCta>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        relacionModeloCta.INT_IDRELMODCTA = Convert.ToInt32(rdr["INT_IDRELMODCTA"]);
                        relacionModeloCta.STR_CONCEPTO = Convert.ToString(rdr["STR_CONCEPTO"]);
                        relacionModeloCta.STR_CTA_INICIO = Convert.ToString(rdr["STR_CTA_INICIO"]);
                        relacionModeloCta.STR_CTA_FIN = Convert.ToString(rdr["STR_CTA_FIN"]);
                        relacionModeloCta.INT_NATURALEZA = Convert.ToInt32(rdr["INT_NATURALEZA"]);
                        relacionModeloCta.INT_IDMODELO = Convert.ToInt32(rdr["INT_IDMODELO"]);
                        relacionModeloCta.INT_TIPO = Convert.ToInt32(rdr["INT_TIPO"]);
                        relacionModeloCta.BOOL_ESTATUS_LOGICO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO"]);
                        relacionModeloCta.FECH_MODIF_RELMODELO = Convert.ToDateTime(rdr["FECH_MODIF_RELMODELO"]);

                        lstRelacionModeloCta.Add(relacionModeloCta);
                    }
                    con.Close();
                }

                return lstRelacionModeloCta;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
             
                throw;
            }

        }

        public int update(RelacionModeloCta relacionModeloCta)
        {
            string add = "UPDATE " + cod + "TAB_RELMODELO_CTA" + cod +
            " SET " + cod + "STR_CONCEPTO" + cod + "= " + "@STR_CONCEPTO" + ","
            + cod + "STR_CTA_INICIO" + cod + "= " + "@STR_CTA_INICIO" + ","
            + cod + "STR_CTA_FIN" + cod + "= " + "@STR_CTA_FIN" + ","
            + cod + "INT_NATURALEZA" + cod + "= " + "@INT_NATURALEZA" + ","
            + cod + "INT_IDMODELO" + cod + "= " + "@INT_IDMODELO" + ","
            + cod + "INT_TIPO" + cod + "= " + "@INT_TIPO" + ","
            + cod + "BOOL_ESTATUS_LOGICO" + cod + "= " + "@BOOL_ESTATUS_LOGICO" + ","
            + cod + "FECH_MODIF_RELMODELO" + cod + "= " + "@FECH_MODIF_RELMODELO"
            + " WHERE " + cod + "INT_IDRELMODCTA" + cod + " = " + "@INT_IDRELMODCTA";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELMODCTA", Value = relacionModeloCta.INT_IDRELMODCTA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CONCEPTO", Value = relacionModeloCta.STR_CONCEPTO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CTA_INICIO", Value = relacionModeloCta.STR_CTA_INICIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CTA_FIN", Value = relacionModeloCta.STR_CTA_FIN });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_NATURALEZA", Value = relacionModeloCta.INT_NATURALEZA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELO", Value = relacionModeloCta.INT_IDMODELO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_TIPO", Value = relacionModeloCta.INT_TIPO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO", Value = relacionModeloCta.BOOL_ESTATUS_LOGICO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FECH_MODIF_RELMODELO", Value = DateTime.Now });



                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }

        }

        public int delete(RelacionModeloCta relacionModeloCta)
        {

            string add = "UPDATE " + cod + "TAB_RELMODELO_CTA" + cod +
            " SET " + cod + "BOOL_ESTATUS_LOGICO" + cod + "= " + "@BOOL_ESTATUS_LOGICO" + ","
            + cod + "FECH_MODIF_RELMODELO" + cod + "= " + "@FECH_MODIF_RELMODELO"
            + " WHERE " + cod + "INT_IDRELMODCTA" + cod + " = " + "@INT_IDRELMODCTA";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELMODCTA", Value = relacionModeloCta.INT_IDRELMODCTA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO", Value = relacionModeloCta.BOOL_ESTATUS_LOGICO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FECH_MODIF_RELMODELO", Value = DateTime.Now });



                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }


            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }

        }


        public int insertarModCta(RelacionModeloCta relacionModeloCta)

        {
            string add = "INSERT INTO" + cod + "TAB_RELMODELO_CTA" + cod + "("
                + cod + "STR_CONCEPTO" + cod + ","
                + cod + "STR_CTA_INICIO" + cod + ","
                + cod + "STR_CTA_FIN" + cod + ","
                + cod + "INT_NATURALEZA" + cod + ","
                + cod + "INT_IDMODELO" + cod + ", "
                + cod + "INT_TIPO" + cod + ","
                + cod + "BOOL_ESTATUS_LOGICO" + cod + ","
                + cod + "FECH_MODIF_RELMODELO" + cod + ") " +
                "VALUES " +
                "(@STR_CONCEPTO,@STR_CTA_INICIO," +
                "@STR_CTA_FIN,@INT_NATURALEZA," +
                "@INT_IDMODELO,@INT_TIPO,@BOOL_ESTATUS_LOGICO,@FECH_MODIF_RELMODELO)";
            try
            {

                {
                    con.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_CONCEPTO", relacionModeloCta.STR_CONCEPTO);
                    cmd.Parameters.AddWithValue("@STR_CTA_INICIO", relacionModeloCta.STR_CTA_INICIO);
                    cmd.Parameters.AddWithValue("@STR_CTA_FIN", relacionModeloCta.STR_CTA_FIN);
                    cmd.Parameters.AddWithValue("@INT_NATURALEZA", relacionModeloCta.INT_NATURALEZA);
                    cmd.Parameters.AddWithValue("@INT_IDMODELO", relacionModeloCta.INT_IDMODELO);
                    cmd.Parameters.AddWithValue("@INT_TIPO", relacionModeloCta.INT_TIPO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO", relacionModeloCta.BOOL_ESTATUS_LOGICO);
                    cmd.Parameters.AddWithValue("@FECH_MODIF_RELMODELO", DateTime.Now);
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }

            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
        }

        public RelacionModeloCta GetRelacionModeloCta(int id) {

            string consulta = "SELECT * FROM" + cod + "TAB_RELMODELO_CTA" + cod + "WHERE" + cod + "INT_IDRELMODCTA" + cod + "=" + id;
            try
            {
                RelacionModeloCta relacionModeloCta = new RelacionModeloCta();


                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);


                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        relacionModeloCta.INT_IDRELMODCTA = Convert.ToInt32(rdr["INT_IDRELMODCTA"]);
                        relacionModeloCta.STR_CONCEPTO = Convert.ToString(rdr["STR_CONCEPTO"]);
                        relacionModeloCta.STR_CTA_INICIO = Convert.ToString(rdr["STR_CTA_INICIO"]);
                        relacionModeloCta.STR_CTA_FIN = Convert.ToString(rdr["STR_CTA_FIN"]);
                        relacionModeloCta.INT_NATURALEZA = Convert.ToInt32(rdr["INT_NATURALEZA"]);
                        relacionModeloCta.INT_IDMODELO = Convert.ToInt32(rdr["INT_IDMODELO"]);
                        relacionModeloCta.INT_TIPO = Convert.ToInt32(rdr["INT_TIPO"]);
                        relacionModeloCta.BOOL_ESTATUS_LOGICO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO"]);
                        relacionModeloCta.FECH_MODIF_RELMODELO = Convert.ToDateTime(rdr["FECH_MODIF_RELMODELO"]);

                    }

                    con.Close();
                }
                return relacionModeloCta;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
        }



    }
}
