﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppGia.Util;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class ProyectoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        private QueryExecuter _queryExecuter=new QueryExecuter();


        public ProyectoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Proyecto> GetAllProyectos()
        {
            string cadena = " select * from proyecto "
                          + "  where  activo  = true order by id desc" ;
            
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

                        DataTable dataTable=_queryExecuter.ExecuteQuery(
                            "select e.desc_id from empresa_proyecto empr join empresa e on empr.empresa_id = e.id " +
                            " where empr.activo=true and e.activo=true and  empr.proyecto_id=@id",
                            new NpgsqlParameter("@id", proyecto.id));
                        String desIdsEmpresas = "";
                        foreach (DataRow row in dataTable.Rows)
                        {
                            desIdsEmpresas += row["desc_id"]+" ";
                        }

                        proyecto.idsempresas = desIdsEmpresas;

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

        public Proyecto GetProyectoData(Int64 id)
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
                        DataTable dataTable = _queryExecuter.ExecuteQuery(
                            "select empresa_id from empresa_proyecto where activo=true and proyecto_id=@proyecto_id",
                            new NpgsqlParameter("@proyecto_id",id));
                        List<Int64> idsempresas=new List<Int64>();
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            idsempresas.Add(Convert.ToInt64(dr["empresa_id"]));             
                        }
                        proyecto.idsempresas =  string.Join(",", idsempresas.ToArray());
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
            int existentes=Convert.ToInt32(_queryExecuter.ExecuteQueryUniqueresult(
                "select count(1) as existentes from proyecto where desc_id=@desc_id and activo=true",
                new NpgsqlParameter("@desc_id", proyecto.desc_id.Trim()))["existentes"]);
            if (existentes > 0)
            {
                throw new DataException("Ya existe un proyecto registrado con el id="+proyecto.desc_id);
            }
            string add = "insert into "
                + " proyecto " + "("
                + "id" + ","
                + "desc_id" + ","
                + "estatus" + ","
                + "nombre" + ","
                + "responsable" + ","
                + "fecha_inicio" + ","
                + "fecha_fin" + ","
                + "fecha_creacion" + ","
                + "fecha_modificacion" + ","
                + "activo" + ") values " +
                "(nextval('seq_proyecto'),@desc_id,@estatus,@nombre,@responsable,@fecha_inicio,@fecha_fin,@fecha_fin,@fecha_modificacion,@activo)RETURNING id ";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@desc_id", proyecto.desc_id.Trim());
                    cmd.Parameters.AddWithValue("@activo", proyecto.activo);
                    cmd.Parameters.AddWithValue("@estatus", proyecto.estatus.Trim());
                    cmd.Parameters.AddWithValue("@nombre", proyecto.nombre.Trim());
                    cmd.Parameters.AddWithValue("@responsable", proyecto.responsable.Trim());
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


        public int update(Int64 id, Proyecto proyecto)
        {
            string update = "update proyecto"
                  + " set "
                  + " nombre  = @nombre ,"
                  + " responsable  = @responsable,"
                  + " desc_id  = @desc_id ,"
                  + " fecha_modificacion  = @fecha_modificacion ,"
                  + " estatus  = @estatus"
                  + " WHERE id = @id" ;
            int res=_queryExecuter.execute(update, new NpgsqlParameter("@id", id),
                new NpgsqlParameter("@nombre", proyecto.nombre.Trim()),
                new NpgsqlParameter("@responsable", proyecto.responsable.Trim()),
                new NpgsqlParameter("@desc_id", proyecto.desc_id.Trim()),
                new NpgsqlParameter("@fecha_modificacion", DateTime.Now),
                new NpgsqlParameter("@estatus", proyecto.estatus)
                );
            if (proyecto.idsempresas != null && proyecto.idsempresas.Length > 0)
            {
                _queryExecuter.execute("update empresa_proyecto set activo = false  where proyecto_id=@proyecto_id",
                    new NpgsqlParameter("@proyecto_id", id));
                addEmpresa_Proyecto(id, proyecto);
            }

            return res;
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

        public void addEmpresa_Proyecto(long id, Proyecto proyecto)
        {
            string idEmpresas = proyecto.idsempresas;
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
                        cmd.Parameters.AddWithValue("@activo", true);
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
