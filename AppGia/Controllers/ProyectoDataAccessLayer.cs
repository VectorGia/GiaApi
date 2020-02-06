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


        public ProyectoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Proyecto> GetAllProyectos()
        {
            string cadena = " select * from proyecto "
                          + "  where  activo  = " + true;
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

                        proyecto.id = Convert.ToInt64(rdr["id"]);
                        proyecto.desc_id = rdr["desc_id"].ToString().Trim();
                        proyecto.nombre = rdr["nombre"].ToString().Trim();
                        proyecto.activo = Convert.ToBoolean(rdr["activo"]);
                        proyecto.responsable = rdr["responsable"].ToString().Trim();
                        proyecto.estatus = rdr["estatus"].ToString().Trim();
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
            finally
            {
                con.Close();
            }
        }

        public Proyecto GetProyectoData(string id)
        {
            try
            {
                Proyecto proyecto = new Proyecto();
                {
                    string consulta = "  select * from proyecto"
                                     + " where  id  = " + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        proyecto.id = Convert.ToInt64(rdr["id"]);
                        proyecto.desc_id = rdr["desc_id"].ToString().Trim();
                        proyecto.nombre = rdr["nombre"].ToString().Trim();
                        proyecto.activo = Convert.ToBoolean(rdr["activo"]);
                        proyecto.responsable = rdr["responsable"].ToString().Trim();
                        proyecto.estatus = rdr["estatus"].ToString().Trim();
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
            finally
            {
                con.Close();
            }
        }

        public long addProyecto(Proyecto proyecto)
        {
            string add = "insert into "
                + " proyecto " + "("
                + "id" + ","
                + "desc_id" + ","
                + "estatus" + ","
                + "nombre" + ","
                + "responsable" + ","
                + "modelo_negocio_id" + ","
                + "fecha_inicio" + ","
                + "fecha_fin" + ","
                + "fecha_creacion" + ","
                + "fecha_modificacion" + ","
                + "activo" + ") values " +
                "(nextval('seq_proyecto'),@desc_id,@estatus,@nombre,@responsable,@modelo_negocio_id,@fecha_inicio,@fecha_fin,@fecha_fin,@fecha_modificacion,@activo)RETURNING id ";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@desc_id", proyecto.desc_id.Trim());
                    cmd.Parameters.AddWithValue("@activo", proyecto.activo);
                    cmd.Parameters.AddWithValue("@estatus", proyecto.estatus.Trim());
                    cmd.Parameters.AddWithValue("@nombre", proyecto.nombre.Trim());
                    cmd.Parameters.AddWithValue("@responsable", proyecto.responsable.Trim());
                    cmd.Parameters.AddWithValue("@modelo_negocio_id", proyecto.modelo_negocio_id);
                    cmd.Parameters.AddWithValue("@fecha_inicio", proyecto.fecha_inicio);
                    cmd.Parameters.AddWithValue("@fecha_fin", proyecto.fecha_fin);
                    cmd.Parameters.AddWithValue("@fecha_creacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@fecha_modificacion", DateTime.Now);
                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();

                    // obtiene la ultima secuencia usada para la insercion del id
                    cmd.CommandText = "SELECT currval('seq_proyecto') AS lastProyecto";
                    long idproyecto = (long)cmd.ExecuteScalar();
                    con.Close();
                    return idproyecto;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }


        public int update(string id, Proyecto proyecto)
        {
            string update = "update proyecto"
                  + " set "
                  + " nombre  = @nombre ,"
                  + " responsable  = @responsable,"
                  + " desc_id  = @desc_id ,"
                  + " fecha_modificacion  = @fecha_modificacion ,"
                  + " estatus  = @estatus"
                  + " WHERE id = " + id;
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);
                    cmd.Parameters.AddWithValue("@nombre", proyecto.nombre.Trim());
                    cmd.Parameters.AddWithValue("@responsable", proyecto.responsable.Trim());
                    cmd.Parameters.AddWithValue("@desc_id", proyecto.desc_id.Trim());
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
            finally
            {
                con.Close();
            }
        }

        public int Delete(string id)
        {
            bool status = false;
            string delete = " update proyecto set activo = '" + status + "' "
                          + " where id ='" + id + "'";
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
            finally
            {
                con.Close();
            }
        }

        public void addEmpresa_Proyecto(long id, Proyecto proyectos)
        {
            string idEmpresas = proyectos.idsempresas;
            string[] arrIdEmpresas = idEmpresas.Split(',');
            try
            {
                foreach (var ids in arrIdEmpresas)
                {
                    string add = "insert into "
                   + " empresa_proyecto ("
                   + " id ,"
                   + " activo ,"
                   + " empresa_id ,"
                   + " proyecto_id "
                   + " ) values " +
                   "( @nextval('seq_empresa_proy'),@activo,@empresa_id,@proyecto_id)";
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                        Empresa_Proyecto empresa_proyecto = new Empresa_Proyecto();
                        cmd.Parameters.AddWithValue("@activo", empresa_proyecto.activo);
                        cmd.Parameters.AddWithValue("@empresa_id", Convert.ToInt64(ids));//empresa asociada
                        cmd.Parameters.AddWithValue("@proyecto_id", id);
                        con.Open();
                        long cantFilAfec = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
