using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class EstatusETLDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = '"';

        public EstatusETLDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<EstatusETL> GetAllEstatusETL()
        {
            EstatusETL estatusetl = new EstatusETL();
            string cadena = "SELECT *FROM " + cod + "CAT_ESTUSTETL" + cod;
            try
            {
                List<EstatusETL> lstEstatusETL = new List<EstatusETL>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        estatusetl.INT_IDESTATUSETL_P = Convert.ToInt32(rdr["INT_IDESTATUSETL_P"]);
                        estatusetl.VAR_DESCRIPCION = Convert.ToString(rdr["VAR_DESCRIPCION"]).Trim();
                        estatusetl.BOOL_ESTATUS_LOGICO_ESTETL = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_ESTETL"]);
                        estatusetl.FEC_MODIF_LOGETL = Convert.ToDateTime(rdr["FEC_MODIF_RELUSU"]);
                        lstEstatusETL.Add(estatusetl);
                    }
                    con.Close();
                }

                return lstEstatusETL;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int update(EstatusETL estatusetl)
        {
            string add = "UPDATE " + cod + "CAT_ESTUSTETL" + cod +
            " SET " + cod + "VAR_DESCRIPCION" + cod + "= " + "@VAR_DESCRIPCION" + ","
            + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + "= " + "@BOOL_ESTATUS_LOGICO_ESTETL" + ","
            + cod + "FEC_MODIF_LOGETL" + cod + "= " + "@FEC_MODIF_LOGETL"
            + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = estatusetl.INT_IDESTATUSETL_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar, ParameterName = "@VAR_DESCRIPCION", Value = estatusetl.VAR_DESCRIPCION.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_ESTETL", Value = estatusetl.BOOL_ESTATUS_LOGICO_ESTETL });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_LOGETL", Value = DateTime.Now });



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

        public int delete(EstatusETL estatusetl)
        {

            string add = "UPDATE " + cod + "CAT_ESTUSTETL" + cod +
            " SET " + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + "= " + "@BOOL_ESTATUS_LOGICO_ESTETL" + ","
            + cod + "FEC_MODIF_LOGETL" + cod + "= " + "@FEC_MODIF_LOGETL"
            + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = estatusetl.INT_IDESTATUSETL_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_ESTETL", Value = estatusetl.BOOL_ESTATUS_LOGICO_ESTETL });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_LOGETL", Value = DateTime.Now });



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

        public int insert(EstatusETL estatusetl)

        {

            string add = "INSERT INTO" + cod + "CAT_ESTUSTETL" + cod + "("
                + cod + "VAR_DESCRIPCION" + cod + ","
                + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + ","
                + cod + "FEC_MODIF_LOGETL" + cod + ") " +
                "VALUES " +
                "(@VAR_DESCRIPCION,@BOOL_ESTATUS_LOGICO_ESTETL," +
                "@FEC_MODIF_LOGETL)";
            try
            {

                {
                    con.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@VAR_DESCRIPCION", estatusetl.VAR_DESCRIPCION.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_ESTETL", estatusetl.BOOL_ESTATUS_LOGICO_ESTETL);
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
