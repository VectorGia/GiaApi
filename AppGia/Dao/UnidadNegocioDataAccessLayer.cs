using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class UnidadNegocioDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public UnidadNegocioDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public List<UnidadNegocio> GetAllUnidadNegocio()
        {
            string consulta = "";
            consulta += " select ";
            consulta += "   id, clave, descripcion, usuario, fec_modif, activo ";
            consulta += "   from unidad_negocio ";
            consulta += "   where activo = 'true' ";

            try
            {
                List<UnidadNegocio> lstUnidadNegocio = new List<UnidadNegocio>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    UnidadNegocio unidadNegocio = new UnidadNegocio();
                    unidadNegocio.id = Convert.ToInt64(rdr["id"]);
                    unidadNegocio.clave = Convert.ToInt32(rdr["id"]);
                    unidadNegocio.descripcion = (rdr["descripcion"]).ToString();
                    unidadNegocio.idusuario = Convert.ToInt64(rdr["usuario"]);
                    unidadNegocio.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                    unidadNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                    lstUnidadNegocio.Add(unidadNegocio);
                }
                return lstUnidadNegocio;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public UnidadNegocio GetUnidadNegocio(int idUnidadNegocio)
        {
            string consulta = "";
            consulta += " select ";
            consulta += "   id, clave, descripcion, usuario, fec_modif, activo ";
            consulta += "   from unidad_negocio ";
            consulta += "   where id = " + idUnidadNegocio;

            try
            {
                UnidadNegocio unidadNegocio = new UnidadNegocio();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    unidadNegocio.id = Convert.ToInt64(rdr["id"]);
                    unidadNegocio.clave = Convert.ToInt32(rdr["id"]);
                    unidadNegocio.descripcion = (rdr["descripcion"]).ToString();
                    unidadNegocio.idusuario = Convert.ToInt64(rdr["usuario"]);
                    unidadNegocio.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                    unidadNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                }

                return unidadNegocio;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int AddUnidadNegocio(UnidadNegocio unidadNegocio)
        {
            DateTime fechaHoy = DateTime.Now;
            string consulta = "";
            consulta += " insert into unidad_negocio ( ";
            consulta += "	 id, clave, descripcion, usuario, fec_modif, activo ";
            consulta += " ) values ( ";
            consulta += "	 nextval('seq_unidad_negocio'), @clave, @descripcion, @usuario, @fec_modif, true ";
            consulta += " ) ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                cmd.Parameters.AddWithValue("@clave", unidadNegocio.clave);
                cmd.Parameters.AddWithValue("@descripcion", unidadNegocio.descripcion);
                cmd.Parameters.AddWithValue("@usuario", unidadNegocio.idusuario);
                cmd.Parameters.AddWithValue("@fec_modif", fechaHoy);

                int regInsert = cmd.ExecuteNonQuery();

                return regInsert;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int UpdateUnidadNegocio(UnidadNegocio unidadNegocio)
        {
            DateTime fechaHoy = DateTime.Now;
            string consulta = "";
            consulta += " update unidad_negocio set ";
            consulta += "   clave = @clave, ";
            consulta += "   descripcion = @descripcion, ";
            consulta += "   usuario = @usuario, ";
            consulta += "   fec_modif = @fec_modif ";
            consulta += " where id = @id ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                cmd.Parameters.AddWithValue("@clave", unidadNegocio.clave);
                cmd.Parameters.AddWithValue("@descripcion", unidadNegocio.descripcion);
                cmd.Parameters.AddWithValue("@usuario", unidadNegocio.idusuario);
                cmd.Parameters.AddWithValue("@fec_modif", fechaHoy);
                cmd.Parameters.AddWithValue("@activo", unidadNegocio.activo);
                cmd.Parameters.AddWithValue("@id", unidadNegocio.id);

                int regActual = cmd.ExecuteNonQuery();

                return regActual;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int DeleteUnidadNegocio(UnidadNegocio unidadNegocio)
        {
            DateTime fechaHoy = DateTime.Now;
            string consulta = "";
            consulta += " update unidad_negocio set ";
            consulta += "   usuario = @usuario, ";
            consulta += "   fec_modif = @fec_modif, ";
            consulta += "   activo = false ";
            consulta += " where id = @id ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                cmd.Parameters.AddWithValue("@usuario", unidadNegocio.idusuario);
                cmd.Parameters.AddWithValue("@fec_modif", fechaHoy);
                cmd.Parameters.AddWithValue("@id", unidadNegocio.id);

                int regActual = cmd.ExecuteNonQuery();

                return regActual;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
