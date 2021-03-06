﻿using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;

namespace AppGia.Dao
{
    public class UnidadNegocioDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        private QueryExecuter queryExecuter= new QueryExecuter();

        public UnidadNegocioDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public List<UnidadNegocio> GetAllUnidadNegocioWithModelo()
        {
            List<UnidadNegocio> lstUnidadNegocio = new List<UnidadNegocio>();
            DataTable dataTable = queryExecuter.ExecuteQuery("select distinct un.* " +
                                       " from unidad_negocio un " +
                                       "    join modelo_unidad mu on un.id = mu.id_unidad and mu.activo = true " +
                                       " join modelo_negocio mn on mu.id_modelo = mn.id and mn.activo = true " +
                                       " join centro_costo cc on mn.id = cc.modelo_negocio_id and cc.activo=true " +
                                       " where un.activo = true");
            foreach (DataRow rdr in dataTable.Rows)
            {
                UnidadNegocio unidadNegocio = new UnidadNegocio();
                unidadNegocio.id = Convert.ToInt64(rdr["id"]);
                unidadNegocio.descripcion = (rdr["descripcion"]).ToString();
                lstUnidadNegocio.Add(unidadNegocio);
            }

            return lstUnidadNegocio;
        }

        public List<Dictionary<string, Int64>> GetUnidadCC()
        {
            List<Dictionary<string, Int64>> relaciones = new List<Dictionary<string, Int64>>();
            DataTable dataTable = queryExecuter.ExecuteQuery("select un.id as unidadid,cc.id as ccid " +
                                                             " from unidad_negocio un " +
                                                             "    join modelo_unidad mu on un.id = mu.id_unidad and mu.activo = true  " +
                                                             " join modelo_negocio mn on mu.id_modelo = mn.id and mn.activo = true  " +
                                                             " join centro_costo cc on mn.id = cc.modelo_negocio_id and cc.activo=true  " +
                                                             " where un.activo = true");
            foreach (DataRow rdr in dataTable.Rows)
            {
                Dictionary<string, Int64> relacion = new Dictionary<string, Int64>();
                relacion["unidadid"] = Convert.ToInt64(rdr["unidadid"]);
                relacion["ccid"] = Convert.ToInt64(rdr["ccid"]);
                relaciones.Add(relacion);
            }

            return relaciones;
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
