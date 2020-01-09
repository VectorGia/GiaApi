using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class LogETLDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = '"';

        public LogETLDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<LogEtl> GetAllLogETL()
        {
            LogEtl logetl = new LogEtl();
            string cadena = "SELECT * FROM " + cod + "TAB_LOGETL" + cod;
            try
            {
                List<LogEtl> lstLogETL = new List<LogEtl>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        logetl.INT_IDLOGETL_P = Convert.ToInt32(rdr["INT_IDLOGETL_P"]);
                        logetl.FEC_ETL = Convert.ToDateTime(rdr["FEC_ETL"]);
                        logetl.INT_IDUSUARIO = Convert.ToInt32(rdr["INT_IDUSUARIO"]);
                        logetl.INT_IDESTATUSETL_P = Convert.ToInt32(rdr["INT_IDESTATUSETL_P"]);
                        logetl.INT_IDBALANZA = Convert.ToInt32(rdr["INT_IDBALANZA"]);
                        logetl.INT_TIPOETL = Convert.ToInt32(rdr["INT_TIPOETL"]);
                        lstLogETL.Add(logetl);
                    }
                    con.Close();
                }

                return lstLogETL;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        //Obtiene el id del ultimo registro 
        public LogEtl GetLogETLData()
        {
            string consulta = "SELECT MAX(" + cod + "INT_IDLOGETL_P" + cod + ") " + cod + "INT_IDLOGETL_P" + cod + " FROM " + cod + "TAB_LOGETL" + cod;
            try
            {
                LogEtl logetl = new LogEtl();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        logetl.INT_IDLOGETL_P = Convert.ToInt32(rdr["INT_IDLOGETL_P"]);
                    }

                    con.Close();
                }
                return logetl;
            }
            catch
            {
                con.Close();
                throw;

            }
        }

        public int update(string campo, int valor, int id)
        {
            string add = "UPDATE " + cod + "TAB_LOGETL" + cod +
            " SET " + cod + campo + cod + "= " + "@VAR_VALOR"
            + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@VAR_VALOR", Value = valor });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = id });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch
            {
                con.Close();
                throw;

            }

        }

        //public int delete(EstatusETL estatusetl)
        //{

        //    string add = "UPDATE " + cod + "CAT_ESTUSTETL" + cod +
        //    " SET " + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + "= " + "@BOOL_ESTATUS_LOGICO_ESTETL" + ","
        //    + cod + "FEC_MODIF_LOGETL" + cod + "= " + "@FEC_MODIF_LOGETL"
        //    + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

        //    try
        //    {

        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand(add, con);

        //            cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = estatusetl.INT_IDESTATUSETL_P });
        //            cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_ESTETL", Value = estatusetl.BOOL_ESTATUS_LOGICO_ESTETL });
        //            cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_LOGETL", Value = DateTime.Now });



        //            con.Open();
        //            int cantFilas = cmd.ExecuteNonQuery();
        //            con.Close();
        //            return cantFilas;
        //        }

        //    }

        //    catch
        //    {
        //        con.Close();
        //        throw;

        //    }

        //}

        public int insert(LogEtl logetl)

        {

            string add = "INSERT INTO" + cod + "TAB_LOGETL" + cod + "("
                + cod + "FEC_ETL" + cod + ","
                + cod + "INT_TIPOETL" + cod + ","
                + cod + "INT_IDUSUARIO" + cod + ") " +
                "VALUES " +
                "(@FEC_ETL," +
                "@INT_TIPOETL," +
                "@INT_IDUSUARIO)";
            try
            {

                {
                    con.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@FEC_ETL", DateTime.Now);
                    cmd.Parameters.AddWithValue("@INT_TIPOETL", logetl.INT_TIPOETL);
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO", logetl.INT_IDUSUARIO);
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }

            }
            catch
            {
                con.Close();
                throw;
            }
        }
    }
}
