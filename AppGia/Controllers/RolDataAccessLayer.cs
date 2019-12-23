using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class RolDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Rol> GetAllRoles()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_ROL" + cod + "";
            try
            {
                List<Rol> lstrol = new List<Rol>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Rol rol = new Rol();
                        rol.STR_NOMBRE_ROL = rdr["STR_NOMBRE_ROL"].ToString();
                        rol.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_P"]);

                        lstrol.Add(rol);
                    }
                    con.Close();
                }

                return lstrol;
            }
            catch
            {
                throw;
            }
        }

        public int addRol(Rol rol)
        {
            string add = "INSERT INTO" + cod + "CAT_ROL" + cod + "(" + cod + "STR_NOMBRE_ROL" + cod + ") VALUES " +
                "(@STR_NOMBRE_ROL)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                   
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_ROL", rol.STR_NOMBRE_ROL);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_ROL", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_ROL", rol.BOOL_ESTATUS_LOGICO_ROL);

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

        public int update(Rol rol)
        {

            string update = "UPDATE " + cod + "CAT_ROL" + cod + "SET"

          + cod + "STR_NOMBRE_ROL" + cod + " = '" + rol.STR_NOMBRE_ROL + "' ,"
          + cod + "BOOL_ESTATUS_LOGICO_ROL" + cod + " = '" + rol.BOOL_ESTATUS_LOGICO_ROL + "' ,"
          + cod + "FEC_MODIF_ROL" + cod + " = '" + rol.FEC_MODIF_ROL + "' "
          + " WHERE" + cod + "INT_IDROL_P" + cod + "=" + rol.INT_IDROL_P;


            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);


                    cmd.Parameters.AddWithValue("@STR_NOMBRE_ROL", rol.STR_NOMBRE_ROL);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_ROL", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_ROL", rol.BOOL_ESTATUS_LOGICO_ROL);
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

        public int Delete(Rol rol)
        {

            string delete = "UPDATE " + cod + "CAT_ROL" + cod + "SET" + cod + "BOOL_ESTATUS_ROL" + cod + "='" + rol.BOOL_ESTATUS_LOGICO_ROL + "' WHERE" + cod + "INT_IDROL_P" + cod + "='" + rol.INT_IDROL_P + "'";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", rol.BOOL_ESTATUS_LOGICO_ROL);

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
    }
}
