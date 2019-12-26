using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ProyectoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public ProyectoDataAccessLayer() 
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Proyecto> GetAllProyectos()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_PROYECTO" + cod + "WHERE " + cod + "BOOL_ESTATUS_LOGICO_PROYECTO" + cod + "=" + true;
            try
            {
                List<Proyecto> lstProyecto = new List<Proyecto>();

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Proyecto proyecto = new Proyecto();

                        proyecto.INT_IDPROYECTO_P = Convert.ToInt32(rdr["INT_IDPROYECTO_P"]);    
                        proyecto.STR_IDPROYECTO = rdr["STR_IDPROYECTO"].ToString().Trim();
                        proyecto.STR_NOMBRE_PROYECTO = rdr["STR_NOMBRE_PROYECTO"].ToString().Trim();
                        proyecto.BOOL_ESTATUS_PROYECTO = Convert.ToBoolean(rdr["BOOL_ESTATUS_PROYECTO"]);
                        proyecto.STR_RESPONSABLE = rdr["STR_RESPONSABLE"].ToString().Trim();
                        proyecto.BOOL_ESTATUS_LOGICO_PROYECTO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_PROYECTO"]);
                        proyecto.FEC_MODIF = Convert.ToDateTime(rdr["FEC_MODIF"]);

                        lstProyecto.Add(proyecto);
                    }
                    con.Close();
                }

                return lstProyecto;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public Proyecto GetProyectoData(Proyecto proyecto)
        {
            try
            {
               

                {
                    string consulta = "SELECT * FROM" + cod + "CAT_PROYECTO" + cod + "WHERE" + cod + "INT_IDPROYECTO_P" + cod + "=" + proyecto.INT_IDPROYECTO_P;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        proyecto.INT_IDPROYECTO_P = Convert.ToInt32(rdr["INT_IDPROYECTO_P"]);
                        proyecto.STR_IDPROYECTO = rdr["STR_IDPROYECTO"].ToString().Trim();
                        proyecto.STR_NOMBRE_PROYECTO = rdr["STR_NOMBRE_PROYECTO"].ToString().Trim();
                        proyecto.BOOL_ESTATUS_PROYECTO = Convert.ToBoolean(rdr["BOOL_ESTATUS_PROYECTO"]);
                        proyecto.STR_RESPONSABLE = rdr["STR_RESPONSABLE"].ToString().Trim();
                        proyecto.BOOL_ESTATUS_LOGICO_PROYECTO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_PROYECTO"]);
                        proyecto.FEC_MODIF = Convert.ToDateTime(rdr["FEC_MODIF"]);
                    }
                }
                return proyecto;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public int addProyecto(Proyecto proyecto)
        {
            string add = "INSERT INTO" 
                +cod+ "CAT_PROYECTO" +cod+ "("
                +cod+"STR_IDPROYECTO"+cod+","
                +cod+"STR_NOMBRE_PROYECTO" + cod + ","
                +cod+ "BOOL_ESTATUS_PROYECTO" + cod+","
                +cod+"STR_RESPONSABLE"+cod+","
                +cod+ "FEC_MODIF"+cod+","
                +cod+ "BOOL_ESTATUS_LOGICO_PROYECTO" + cod+") VALUES " +
                "(@STR_IDPROYECTO,@STR_NOMBRE_PROYECTO,@BOOL_ESTATUS_PROYECTO,@STR_RESPONSABLE,@FEC_MODIF,@BOOL_ESTATUS_LOGICO_PROYECTO)";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_IDPROYECTO", proyecto.STR_IDPROYECTO.Trim());
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PROYECTO", proyecto.STR_NOMBRE_PROYECTO.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_PROYECTO", proyecto.BOOL_ESTATUS_PROYECTO);
                    cmd.Parameters.AddWithValue("@STR_RESPONSABLE", proyecto.STR_RESPONSABLE.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PROYECTO", proyecto.BOOL_ESTATUS_LOGICO_PROYECTO);
                    con.Open();
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

        //cambios

        public int update(Proyecto proyecto)
        {
            string update = "UPDATE " + cod + "CAT_PROYECTO" + cod + "SET"

         
          + cod + "STR_NOMBRE_PROYECTO" + cod + " = '" + proyecto.STR_NOMBRE_PROYECTO + "' ,"
          + cod + "STR_RESPONSABLE" + cod + " = '" + proyecto.STR_RESPONSABLE + "' ,"
          + cod + "STR_IDPROYECTO" + cod + " = '" + proyecto.STR_IDPROYECTO + "' ,"
          + cod + "FEC_MODIF" + cod + " = '" + proyecto.FEC_MODIF + "' ,"
          + cod + "BOOL_ESTATUS_LOGICO_PROYECTO" + cod + " = '" + proyecto.BOOL_ESTATUS_LOGICO_PROYECTO + "' ,"
          + cod + "BOOL_ESTATUS_PROYECTO" + cod + " = '" + proyecto.BOOL_ESTATUS_PROYECTO + "'"
          + " WHERE" + cod + "INT_IDPROYECTO_P" + cod + "=" + proyecto.INT_IDPROYECTO_P;


            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

             
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PROYECTO", proyecto.STR_NOMBRE_PROYECTO.Trim());
                    cmd.Parameters.AddWithValue("@STR_RESPONSABLE", proyecto.STR_RESPONSABLE.Trim());
                    cmd.Parameters.AddWithValue("@STR_IDPROYECTO", proyecto.STR_IDPROYECTO.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_PROYECTO", proyecto.BOOL_ESTATUS_PROYECTO);
                    cmd.Parameters.AddWithValue("@FEC_MODIF", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PROYECTO", proyecto.BOOL_ESTATUS_LOGICO_PROYECTO);

                    con.Open();
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

        public int Delete(Proyecto proyecto)
        {
            bool status = false;
            string delete = "UPDATE " + cod + "CAT_PROYECTO" + cod + "SET" + cod + "BOOL_ESTATUS_LOGICO_PROYECTO" + cod + "='" + status + "' WHERE" + cod + "INT_IDPROYECTO_P" + cod + "='" + proyecto.INT_IDPROYECTO_P + "'";
            try
            {

                {
                
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);

                    con.Open();
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
