﻿using System.Collections.Generic;
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
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spUpdateCentroCostos", con);

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PERMISO", permiso.STR_NOMBRE_PERMISO);

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

        public int DeletePermiso(int id)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


                    cmd.Parameters.AddWithValue("STR_NOMBRE_PERMISO", id);

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
