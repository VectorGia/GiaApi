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
            string cadena = "select * from proyecto " + " where " +  "activo " + " = " + true;
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

                        proyecto.id = Convert.ToInt32(rdr["id"]);    
                        proyecto.id_proyecto = rdr["id_proyecto"].ToString().Trim();
                        proyecto.nombre = rdr["nombre"].ToString().Trim();
                        proyecto.activo = Convert.ToBoolean(rdr["activo"]);
                        proyecto.responsable = rdr["responsable"].ToString().Trim();
                        proyecto.estatus = Convert.ToBoolean(rdr["estatus"]);
                        proyecto.fecha_modificacion = Convert.ToDateTime(rdr["fecha_modificacion"]);

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

        public Proyecto GetProyectoData(string id)
        {
            try
            {
                Proyecto proyecto = new Proyecto();

                {
                    string consulta = "select * from proyecto" + " where " + " id " + "=" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        proyecto.id = Convert.ToInt32(rdr["id"]);
                        proyecto.id_proyecto = rdr["id_proyecto"].ToString().Trim();
                        proyecto.nombre = rdr["nombre"].ToString().Trim();
                        proyecto.activo = Convert.ToBoolean(rdr["activo"]);
                        proyecto.responsable = rdr["responsable"].ToString().Trim();
                        proyecto.estatus = Convert.ToBoolean(rdr["estatus"]);
                        proyecto.fecha_modificacion = Convert.ToDateTime(rdr["fecha_modificacion"]);
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
            string add = "insert into" 
                + "proyecto" + "("
                + "id_proyecto" + ","
                + "nombre" + ","
                + "estatus" + ","
                + "responsable" + ","
                + "fecha_modificacion" + ","
                + "activo" + ") values " +
                "(@id_proyecto,@nombre,@estatus,@responsable,@fecha_modificacion,@activo)";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@id_proyecto", proyecto.id_proyecto.Trim());
                    cmd.Parameters.AddWithValue("@nombre", proyecto.nombre.Trim());
                    cmd.Parameters.AddWithValue("@estatus", proyecto.estatus);
                    cmd.Parameters.AddWithValue("@responsable", proyecto.responsable.Trim());
                    cmd.Parameters.AddWithValue("@fecha_modificacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@activo", proyecto.activo);
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

        public int update(string id, Proyecto proyecto)
        {
            string update = "update proyecto" + "set"

         
          + "nombre" + " = '" + proyecto.nombre + "' ,"
          + "responsable" + " = '" + proyecto.responsable + "' ,"
          + "id_proyecto" + " = '" + proyecto.id_proyecto + "' ,"
          + "fecha_modificacion" + " = " + "@fecha_modificacion" + " ,"
          + "estatus" + " = '" + proyecto.estatus + "'"
          + " WHERE" + "id" + "=" + id;


            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

             
                    cmd.Parameters.AddWithValue("@nombre", proyecto.nombre.Trim());
                    cmd.Parameters.AddWithValue("@responsable", proyecto.responsable.Trim());
                    cmd.Parameters.AddWithValue("@id_proyecto", proyecto.id_proyecto.Trim());
                    cmd.Parameters.AddWithValue("@fecha_modificacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@estatus", proyecto.activo);
                   

            
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

        public int Delete(string id)
        {
            bool status = false;
            string delete = "udate proyecto" + "set" + "activo" + "='" + status + "' where" + "id" + "='" + id + "'";
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
