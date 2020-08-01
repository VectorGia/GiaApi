using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Dao
{
    public class Empresa_ProyectoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public Empresa_ProyectoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Empresa> GetAllEmpresaByProyectoId(Int64 idProyecto)
        {
            string cadena = " select e.id,e.nombre from empresa_proyecto ep join empresa e on ep.empresa_id = e.id" +
                        " where proyecto_id = " + idProyecto + " and ep.activo = true and e.activo = true";
            try
            {
                List<Empresa> ltsEmpresa = new List<Empresa>();

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Empresa empresa = new Empresa();

                        empresa.id = Convert.ToInt32(rdr["id"]);
                        empresa.nombre = rdr["nombre"].ToString();
                        ltsEmpresa.Add(empresa);
                    }
                
                }
                return ltsEmpresa;
            }
            finally
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                con.Close();
                }

            }
        }

        public IEnumerable<Empresa_Proyecto> GetAllEmpresa_Proyecto()
        {
            string cadena = " select id,activo,empresa_id,proyecto_id from empresa_proyecto "
                            + " where activo  = true ";
            try
            {
                List<Empresa_Proyecto> ltsEmpresaProyecto = new List<Empresa_Proyecto>();

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Empresa_Proyecto empresa_proyecto = new Empresa_Proyecto();

                        empresa_proyecto.id = Convert.ToInt32(rdr["id"]);
                        empresa_proyecto.activo = Convert.ToBoolean(rdr["activo"]);
                        empresa_proyecto.empresa_id = Convert.ToInt32(rdr["empresa_id"]);
                        empresa_proyecto.proyecto_id = Convert.ToInt32(rdr["proyecto_id"]);
                        ltsEmpresaProyecto.Add(empresa_proyecto);
                    }
                    con.Close();
                }
                return ltsEmpresaProyecto;
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

        public Empresa_Proyecto GetEmpresa_ProyectoData(string id)
        {
            try
            {
                Empresa_Proyecto empresa_proyecto = new Empresa_Proyecto();

                {
                    string consulta = "select id,activo,empresa_id,proyecto_id from empresa_proyecto" 
                                        + " where  id  = " + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        empresa_proyecto.id = Convert.ToInt32(rdr["id"]);
                        empresa_proyecto.activo = Convert.ToBoolean(rdr["activo"]);
                        empresa_proyecto.empresa_id = Convert.ToInt32(rdr["empresa_id"]);
                        empresa_proyecto.proyecto_id = Convert.ToInt32(rdr["proyecto_id"]);
                    }
                }

                return empresa_proyecto;
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

        public int add(Empresa_Proyecto empresa_proyecto)
        {
            string add = "insert into"
                + "empresa_proyecto" + "("
                + "id" + ","
                + "activo" + ","
                + "empresa_id" + ","
                + "proyecto_id" + ")"
                + " values "
                + "(@nextval(seq_empresa_proyecto)," +
                "@activo," +
                "@estatus," +
                "@empresa_id," +
                "@proyecto_id)";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@activo", empresa_proyecto.activo);
                    cmd.Parameters.AddWithValue("@estatus", empresa_proyecto.empresa_id);
                    cmd.Parameters.AddWithValue("@responsable", empresa_proyecto.proyecto_id);

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

        public int update(string id, Empresa_Proyecto empresa_proyecto)
        {
            string update = "update empresa_proyecto"
            + "set"
            + " activo  =  @activo,"
            + " empresa_id  = @empresa_id ,"
            + " proyecto_id  = @proyecto_id "
            + " WHERE id = " + id;
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);
                    cmd.Parameters.AddWithValue("@activo", empresa_proyecto.activo);
                    cmd.Parameters.AddWithValue("@empresa_id", empresa_proyecto.empresa_id);
                    cmd.Parameters.AddWithValue("@proyecto_id", empresa_proyecto.proyecto_id);

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
            string delete = "update empresa " 
                            + " set "
                            + " activo  ='" + status +"'" 
                            + " where  id ='"+ id +"'";
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
    }
}
