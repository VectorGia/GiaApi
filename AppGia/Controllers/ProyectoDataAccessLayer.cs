﻿using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ProyectoDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Proyecto> GetAllProyectos()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_PROYECTO" + cod + "";
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
                        proyecto.BOOL_ESTATUS_PROYECTO = Convert.ToBoolean(rdr["BOOL_ESTATUS_PROYECTO"]);
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
            string add = "INSERT INTO" + cod + "CAT_PROYECTO" + cod + "("+cod+"STR_IDPROYECTO"+cod+","+ cod + "STR_NOMBRE_PROYECTO" + cod + ","+cod+ "BOOL_ESTATUS_PROYECTO" + cod+","+cod+"STR_RESPONSABLE"+cod+","+cod+ "FEC_MODIF"+cod+") VALUES " +
                "(@STR_IDPROYECTO,@STR_NOMBRE_PROYECTO,@BOOL_ESTATUS_PROYECTO,@STR_RESPONSABLE,@FEC_MODIF)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_IDPROYECTO", proyecto.STR_IDPROYECTO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PROYECTO", proyecto.STR_NOMBRE_PROYECTO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_PROYECTO", proyecto.BOOL_ESTATUS_PROYECTO);
                    cmd.Parameters.AddWithValue("@STR_RESPONSABLE", proyecto.STR_RESPONSABLE);
                    cmd.Parameters.AddWithValue("@FEC_MODIF", DateTime.Now);
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

        public int update(Proyecto proyecto)
        {
            string update = "UPDATE " + cod + "CAT_PROYECTO" + cod + "SET"

          + cod + "INT_IDPANTALLA_F" + cod + " = " + proyecto.INT_IDPANTALLA_F + ","
          + cod + "STR_NOMBRE_PROYECTO" + cod + " = '" + proyecto.STR_NOMBRE_PROYECTO + "' ,"
          + cod + "STR_RESPONSABLE" + cod + " = '" + proyecto.STR_RESPONSABLE + "' ,"
          + cod + "STR_IDPROYECTO" + cod + " = '" + proyecto.STR_IDPROYECTO + "' ,"
          + cod + "BOOL_ESTATUS_PROYECTO" + cod + " = '" + proyecto.BOOL_ESTATUS_PROYECTO + "' ,"
          + cod + "FEC_MODIF" + cod + " = '" + proyecto.FEC_MODIF + "' ,"
          + cod + "BOOL_ESTATUS_LOGICO_PROYECTO" + cod + "= '" + proyecto.BOOL_ESTATUS_LOGICO_PROYECTO + "' "
          + " WHERE" + cod + "INT_IDPROYECTO_P" + cod + "=" + proyecto.INT_IDPROYECTO_P;


            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@INT_IDPANTALLA_F", proyecto.INT_IDPANTALLA_F);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PROYECTO", proyecto.STR_NOMBRE_PROYECTO);
                    cmd.Parameters.AddWithValue("@STR_RESPONSABLE", proyecto.STR_RESPONSABLE);
                    cmd.Parameters.AddWithValue("@STR_IDPROYECTO", proyecto.STR_IDPROYECTO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_PROYECTO", proyecto.BOOL_ESTATUS_PROYECTO);
                    cmd.Parameters.AddWithValue("@FEC_MODIF", proyecto.FEC_MODIF);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PROYECTO", proyecto.BOOL_ESTATUS_LOGICO_PROYECTO);

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

        public int Delete(Proyecto proyecto)
        {

            string delete = "UPDATE " + cod + "CAT_PROYECTO" + cod + "SET" + cod + "BOOL_ESTATUS_LOGICO_COMPANIA" + cod + "='" + proyecto.BOOL_ESTATUS_LOGICO_PROYECTO + "' WHERE" + cod + "INT_IDPROYECTO_P" + cod + "='" + proyecto.INT_IDPROYECTO_P + "'";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", proyecto.BOOL_ESTATUS_LOGICO_PROYECTO);

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
