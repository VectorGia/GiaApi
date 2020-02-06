using System.Collections.Generic;
using AppGia.Models;
using Npgsql;
using System;

namespace AppGia.Controllers
{
    public class PermisoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
       
        public PermisoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        char cod = '"';
        public IEnumerable<Permiso> GetAllPermisos()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PERMISO" + cod + "";
            try
            {
                List<Permiso> lstpermiso = new List<Permiso>();

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                   
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Permiso permiso = new Permiso();
                        permiso.STR_NOMBRE_PERMISO = rdr["STR_NOMBRE_PERMISO"].ToString().Trim();
                    permiso.INT_IDPERMISO_P = Convert.ToInt32(rdr["INT_IDPERMISO_P"]);
                        lstpermiso.Add(permiso);
                    }
                      con.Close();

                return lstpermiso;
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

        public int addPermiso(Permiso permiso)
        {
            string add = "INSERT INTO" + cod + "TAB_PERMISO" + cod + "(" + cod + "STR_NOMBRE_PERMISO" + cod + ") VALUES " +
                "(@STR_NOMBRE_PERMISO)";
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PERMISO", permiso.STR_NOMBRE_PERMISO.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PERM", permiso.BOOL_ESTATUS_LOGICO_PERM);

                    con.Open();
                    int cantFila = cmd.ExecuteNonQuery();
                    con.Close();

                    return cantFila;
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

        public int UpdatePermiso(Permiso permiso)
        {

            string add = "UPDATE " + cod + "TAB_PERMISO" + cod +
            " SET " + cod + "STR_NOMBRE_PERMISO" + cod + "= " + "@STR_NOMBRE_PERMISO" 
            + " WHERE " + cod + "INT_IDPERMISO_P" + cod + " = " + "@INT_IDPERMISO_P";

            try
            {

                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_PERMISO", Value = permiso.STR_NOMBRE_PERMISO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_P", Value = permiso.INT_IDPERMISO_P });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
            }
            catch
            {
                conex.ConnexionDB().Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }



        public int DeletePermiso(Permiso permiso)
        {
            string add = "UPDATE " + cod + "TAB_PERMISO" + cod
                + " SET " + cod + "BOOL_ESTATUS_LOGICO_PERM" + cod + "= " + "@BOOL_ESTATUS_LOGICO_PERM"
                + " WHERE " + cod + "INT_IDPERMISO_P" + cod + " = " + "@INT_IDPERMISO_P";
            try
            {

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_P", Value = permiso.INT_IDPERMISO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_PERM", Value = permiso.BOOL_ESTATUS_LOGICO_PERM });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
             
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
