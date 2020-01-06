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
            string cadena = "SELECT *FROM " + cod + "TAB_LOGETL" + cod;
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
                        logetl.INT_IDBALANZA = Convert.ToInt32(rdr["INT_IDBALANZA"]);
                        logetl.INT_IDUSUARIO_P = Convert.ToInt32(rdr["INT_IDUSUARIO_P"]);
                        logetl.FEC_ETL = Convert.ToDateTime(rdr["FEC_ETL"]);
                        logetl.INT_ESTATUS_ETL_P = Convert.ToInt32(rdr["INT_ESTATUS_ETL_P"]);
                        logetl.BOOL_ESTATUS_LOGICO_LOGETL = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_LOGETL"]);
                        logetl.FEC_MODIF_LOGETL = Convert.ToDateTime(rdr["FEC_MODIF_RELUSU"]);
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

        //public int update(LogEtl logetl)
        //{
        //    string add = "UPDATE " + cod + "TAB_LOGETL" + cod +
        //    " SET " + cod + "VAR_DESCRIPCION" + cod + "= " + "@VAR_DESCRIPCION" + ","
        //    + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + "= " + "@BOOL_ESTATUS_LOGICO_ESTETL" + ","
        //    + cod + "FEC_MODIF_LOGETL" + cod + "= " + "@FEC_MODIF_LOGETL"
        //    + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

        //    try
        //    {

        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand(add, con);

        //            cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = estatusetl.INT_IDESTATUSETL_P });
        //            cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar, ParameterName = "@VAR_DESCRIPCION", Value = estatusetl.VAR_DESCRIPCION.Trim() });
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
                + cod + "INT_IDBALANZA" + cod + ","
                + cod + "INT_IDUSUARIO_P" + cod + ","
                + cod + "FEC_ETL" + cod + ","
                + cod + "INT_ESTATUS_ETL_P" + cod + ","
                + cod + "BOOL_ESTATUS_LOGICO_LOGETL" + cod + ","
                + cod + "FEC_MODIF_LOGETL" + cod + ") " +
                "VALUES " +
                "(@INT_IDBALANZA," +
                "@INT_IDUSUARIO_P," +
                "@FEC_ETL," +
                "@INT_ESTATUS_ETL_P," +
                "@BOOL_ESTATUS_LOGICO_LOGETL," +
                "@FEC_MODIF_LOGETL)";
            try
            {

                {
                    con.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@INT_IDBALANZA", logetl.INT_IDBALANZA);
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO_P", logetl.INT_IDUSUARIO_P);
                    cmd.Parameters.AddWithValue("@FEC_ETL", logetl.FEC_ETL);
                    cmd.Parameters.AddWithValue("@INT_ESTATUS_ETL_P", logetl.INT_ESTATUS_ETL_P);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_LOGETL", logetl.BOOL_ESTATUS_LOGICO_LOGETL);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_LOGETL", DateTime.Now);
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
