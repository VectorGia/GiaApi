using System.Collections.Generic;
using AppGia.Models;
using Npgsql;


namespace AppGia.Controllers
{
    public class PermisoDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Permiso> GetAllPermisos()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PERMISO" + cod + "";
            try
            {
                List<Permiso> lstpermiso = new List<Permiso>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Permiso permiso = new Permiso();
                        permiso.STR_NOMBRE_PERMISO = rdr["STR_NOMBRE_PERMISO"].ToString();

                        lstpermiso.Add(permiso);
                    }
                    con.Close();
                }

                return lstpermiso;
            }
            catch
            {
                throw;
            }
        }

        public int addPermiso(Permiso permiso)
        {
            string add = "INSERT INTO" + cod + "TAB_PERMISO" + cod + "(" + cod + "STR_NOMBRE_PERMISO" + cod + ") VALUES " +
                "(@STR_NOMBRE_PERMISO)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                   
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PERMISO", permiso.STR_NOMBRE_PERMISO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PERM", permiso.BOOL_ESTATUS_LOGICO_PERM);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int UpdatePermiso(Permiso permiso)
        {

            string add = "UPDATE " + cod + "TAB_PERMISO" + cod +
            " SET " + cod + "STR_NOMBRE_PERMISO" + cod + "= " + "@STR_NOMBRE_PERMISO" + ","
            + cod + "INT_IDROL" + cod + " = " + "@INT_IDROL"
            + " WHERE " + cod + "INT_IDPERMISO_P" + cod + " = " + "@INT_IDPERMISO_P";

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_PERMISO", Value = permiso.STR_NOMBRE_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL", Value = permiso.INT_IDROL });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_P", Value = permiso.INT_IDPERMISO_P });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch
            {
               
                throw;
            }
        }

        //public int DeletePermiso(int id)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


        //            cmd.Parameters.AddWithValue("STR_NOMBRE_PERMISO", id);

        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return 1;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public int DeletePermiso(Permiso permiso)
        {
            string add = "UPDATE " + cod + "TAB_PERMISO" + cod
                + " SET " + cod + "BOOL_ESTATUS_LOGICO_PERM" + cod + "= " + "@BOOL_ESTATUS_LOGICO_PERM"
                + " WHERE " + cod + "INT_IDPERMISO_P" + cod + " = " + "@INT_IDPERMISO_P";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_P", Value = permiso.INT_IDPERMISO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_PERM", Value = permiso.BOOL_ESTATUS_LOGICO_PERM });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
