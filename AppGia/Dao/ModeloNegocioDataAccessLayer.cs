using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Controllers;
using AppGia.Models;
using Npgsql;
using NpgsqlTypes;
using static AppGia.Util.Constantes;

namespace AppGia.Dao
{
    public class ModeloNegocioDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        private QueryExecuter _queryExecuter=new QueryExecuter();
        private ModeloUnidadNegocioDataAccessLayer _modeloUnidadNegocioDataAccessLayer = new ModeloUnidadNegocioDataAccessLayer();
        public ModeloNegocioDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Modelo_Negocio> GetAllModeloNegocios()
        {

            string consulta = "select mn.*,tc.clave as nombre_tipo_captura from modelo_negocio mn join tipo_captura tc on mn.tipo_captura_id = tc.id where mn.activo = true order by mn.id desc";

            try
            {
                List<Modelo_Negocio> lstmodelo = new List<Modelo_Negocio>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Modelo_Negocio modeloNegocio = new Modelo_Negocio();
                    modeloNegocio.id = Convert.ToInt64(rdr["id"]);
                    modeloNegocio.nombre = rdr["nombre"].ToString().Trim();
                    modeloNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                    modeloNegocio.nombre_tipo_captura = rdr["nombre_tipo_captura"].ToString();
                    //modeloNegocio.unidad_negocio_id = Convert.ToInt64(rdr["unidad_negocio_id"]);
                    //modeloNegocio.tipo_captura_id = Convert.ToInt64(rdr["tipo_captura_id"]);
                    lstmodelo.Add(modeloNegocio);
                }
                con.Close();

                return lstmodelo;
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
        public Modelo_Negocio GetModelo(string id)
        {
            try
            {
                Modelo_Negocio modeloNegocio = new Modelo_Negocio();
                {
                    string consulta = "select id, activo, nombre, tipo_captura_id from modelo_negocio  where id = " + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        modeloNegocio.id = Convert.ToInt64(rdr["id"]);
                        modeloNegocio.nombre = rdr["nombre"].ToString().Trim();
                        modeloNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                        modeloNegocio.tipo_captura_id = Convert.ToInt64(rdr["tipo_captura_id"]);
                    }
                    con.Close();
                }
                return modeloNegocio;
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

        public int addModeloNegocioContableAndFlujo(Modelo_Negocio modeloNegocio)
        {
            int co;
            if (existeModeloConNombreYTipo(modeloNegocio.nombre, TipoCapturaContable))
            {
                throw new DataException("Ya existe un modelo con ese nombre");
            }
            if (existeModeloConNombreYTipo(modeloNegocio.nombre, TipoCapturaFlujo))
            {
                throw new DataException("Ya existe un modelo con ese nombre");
            }

            string agrupador=Guid.NewGuid().ToString();
            agrupador=agrupador.Substring(agrupador.Length-12);
            modeloNegocio.agrupador = agrupador;
            
            modeloNegocio.tipo_captura_id = TipoCapturaContable;
            co=addModeloNegocio(modeloNegocio);
            modeloNegocio.tipo_captura_id = TipoCapturaFlujo;
            co+=addModeloNegocio(modeloNegocio);
            return co;
        }

        private bool existeModeloConNombreYTipo(string nombreModelo, int tipoCaptura)
        {
            DataTable dt=new QueryExecuter().ExecuteQuery(
                "select 1 as res from modelo_negocio where activo=true and trim(upper(nombre))=trim(upper('"+nombreModelo+"')) and tipo_captura_id="+tipoCaptura);
            return dt.Rows.Count > 0;
        }

        public int addModeloNegocio(Modelo_Negocio modeloNegocio)
        {
            var idModelo=_queryExecuter.ExecuteQueryUniqueresult("select nextval('seq_modelo_neg') as idModelo")["idModelo"];
            modeloNegocio.id = Convert.ToInt64(idModelo);

            string addModelo = "insert into " + "modelo_negocio"
                                              + "("
                                              + "id" + ","
                                              + "nombre" + ","
                                              + "tipo_captura_id, "
                                              + "activo,"
                                              + "agrupador" +
                                              ") " +
                                              "values " +
                                              "(" +
                                              " @id," +
                                              " @nombre,"
                                              + "@tipo_captura_id,"
                                              + "@activo,"
                                              + " @agrupador" +
                                              ")";


            int cantFilas = _queryExecuter.execute(addModelo,
                new NpgsqlParameter("@id", modeloNegocio.id),
                new NpgsqlParameter("@nombre", modeloNegocio.nombre.Trim()),
                new NpgsqlParameter("@tipo_captura_id", modeloNegocio.tipo_captura_id),
                new NpgsqlParameter("@activo", modeloNegocio.activo),
                new NpgsqlParameter("@agrupador", modeloNegocio.agrupador));

            foreach (var idUnidad in modeloNegocio.unidades_negocio_ids)
            {
                ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();
                modeloUnidadNegocio.idModelo = modeloNegocio.id;
                modeloUnidadNegocio.idUnidad = idUnidad;
                _modeloUnidadNegocioDataAccessLayer.Add(modeloUnidadNegocio);
            }

            return cantFilas;

        }

        public int Update(string id, Modelo_Negocio modeloNegocio)
        {
            Object agrupador =
                _queryExecuter.ExecuteQueryUniqueresult("select agrupador from modelo_negocio where id=@id",
                    new NpgsqlParameter("@id", id))["agrupador"];

            DataTable dataTable = _queryExecuter.ExecuteQuery(
                "select mn.id from modelo_negocio mn" +
                " where mn.activo=true and mn.agrupador=@agrupador",
                new NpgsqlParameter("@agrupador", agrupador.ToString()));

            int co = 0;
            foreach (DataRow modeloIdRow in dataTable.Rows)
            {
                Int64 modeloId = Convert.ToInt64(modeloIdRow["id"]);
                co+=UpdateModelo(modeloId, modeloNegocio);
            }

            return co;
        }
        private int UpdateModelo(Int64 id, Modelo_Negocio modeloNegocio)
        {
            modeloNegocio.id = id;
            string add = "update modelo_negocio set "
                 + "nombre = @nombre "
                 + " where id  = " + modeloNegocio.id;
            
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@nombre", Value = modeloNegocio.nombre.Trim() });
                con.Open();
                int cantFilas = cmd.ExecuteNonQuery();
                con.Close();

                _modeloUnidadNegocioDataAccessLayer.deleteAllModelo(modeloNegocio.id);
                foreach (var idUnidad in modeloNegocio.unidades_negocio_ids)
                {
                    ModeloUnidadNegocio modeloUnidadNegocio = new ModeloUnidadNegocio();
                    modeloUnidadNegocio.idModelo = modeloNegocio.id;
                    modeloUnidadNegocio.idUnidad = idUnidad;
                    _modeloUnidadNegocioDataAccessLayer.Add(modeloUnidadNegocio);
                }
                return cantFilas;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
            finally
            {
                con.Close();
            }
        }
        public int Delete(string id)
        {
            Modelo_Negocio modeloNegocio = new Modelo_Negocio();
            bool status = false;
            string delete = "update modelo_negocio set "
                 + "activo = @activo "
                 + " where id  = " + id;
            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Boolean, ParameterName = "@activo", Value = status });
                int cantFilas = cmd.ExecuteNonQuery();
                con.Close();
                return cantFilas;
            }
            catch (Exception ex)
            {
                con.Close();
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
