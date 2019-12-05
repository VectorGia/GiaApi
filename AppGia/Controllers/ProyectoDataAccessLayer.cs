﻿using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ProyectoDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Proyecto> GetAllProyectos()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PROYECTO" + cod + "";
            try
            {
                List<Proyecto> lstProyecto = new List<Proyecto>();
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Proyecto proyecto = new Proyecto();
                      
                        proyecto.STR_IDPROYECTO = rdr["STR_IDPROYECTO"].ToString();
                        proyecto.STR_NOMBRE_PROYECTO = rdr["STR_NOMBRE_PROYECTO"].ToString();
                        proyecto.STR_DESCRIPCION = rdr["STR_DESCRIPCION"].ToString();                
                        proyecto.STR_RESPONSABLE = rdr["STR_RESPONSABLE"].ToString();
                        
                        lstProyecto.Add(proyecto);
                    }
                    con.Close();
                }

                return lstProyecto;
            }
            catch
            {
                throw;
            }
        }

        public int addProyecto(Proyecto proyecto)
        {
            string add = "INSERT INTO" + cod + "TAB_PROYECTO" + cod + "(" + cod + "STR_NOMBRE_PROYECTO" + cod + ") VALUES " +
                "(@STR_NOMBRE_PROYECTO)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    //cmd.Parameters.AddWithValue("@INT_IDGRUPO", grupo.INT_IDGRUPO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PROYECTO", proyecto.STR_NOMBRE_PROYECTO);

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
