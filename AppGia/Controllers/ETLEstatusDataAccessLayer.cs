using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class ETLEstatusDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = '"';

        public ETLEstatusDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Etl_Estatus> GetAllEstatusETL()
        {
            Etl_Estatus estatusetl = new Etl_Estatus();
            string cadena = "SELECT *FROM " + cod + "CAT_ESTUSTETL" + cod;

            try
            {
                List<Etl_Estatus> lstEstatusETL = new List<Etl_Estatus>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        estatusetl.id = Convert.ToInt32(rdr["id"]);
                        estatusetl.descripcion = Convert.ToString(rdr["descripcion"]).Trim();
                        estatusetl.activo = Convert.ToBoolean(rdr["activo"]);
                        estatusetl.fech_modificacion = Convert.ToDateTime(rdr["fech_modificacion"]);
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
            finally
            {
                con.Close();
            }
        }

        public int update(Etl_Estatus estatusetl)
        {
            string add = "UPDATE " + cod + "CAT_ESTATUSTETL" + cod +
            " SET " + cod + "VAR_DESCRIPCION" + cod + "= " + "@VAR_DESCRIPCION" + ","
            + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + "= " + "@BOOL_ESTATUS_LOGICO_ESTETL" + ","
            + cod + "FEC_MODIF_LOGETL" + cod + "= " + "@FEC_MODIF_LOGETL"
            + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = estatusetl.id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar, ParameterName = "@VAR_DESCRIPCION", Value = estatusetl.descripcion.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_ESTETL", Value = estatusetl.activo });
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
            finally
            {
                con.Close();
            }

        }

        public int delete(Etl_Estatus estatusetl)
        {

            string add = "UPDATE " + cod + "CAT_ESTATUSTETL" + cod +
            " SET " + cod + "BOOL_ESTATUS_LOGICO_ESTETL" + cod + "= " + "@BOOL_ESTATUS_LOGICO_ESTETL" + ","
            + cod + "FEC_MODIF_LOGETL" + cod + "= " + "@FEC_MODIF_LOGETL"
            + " WHERE " + cod + "INT_IDESTATUSETL_P" + cod + " = " + "@INT_IDESTATUSETL_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDESTATUSETL_P", Value = estatusetl.id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_ESTETL", Value = estatusetl.activo });
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
            finally
            {
                con.Close();
            }

        }

        public int insert(Etl_Estatus estatusetl)

        {

            string add = "INSERT INTO" + cod + "CAT_ESTATUSTETL" + cod + "("
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
                    cmd.Parameters.AddWithValue("@VAR_DESCRIPCION", estatusetl.descripcion.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_ESTETL", estatusetl.activo);
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
            finally
            {
                con.Close();
            }
        }
    }
}
