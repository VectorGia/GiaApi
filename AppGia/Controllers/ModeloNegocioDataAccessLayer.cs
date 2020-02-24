using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using Npgsql;
using NpgsqlTypes;
using static AppGia.Util.Constantes;

namespace AppGia.Controllers
{
    public class ModeloNegocioDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
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
                    modeloNegocio.unidad_negocio_id = Convert.ToInt64(rdr["unidad_negocio_id"]);
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
                    string consulta = "select id, activo, nombre, tipo_captura_id, modelo_negocio_id from modelo_negocio  where id = " + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        modeloNegocio.id = Convert.ToInt64(rdr["id"]);
                        modeloNegocio.nombre = rdr["nombre"].ToString().Trim();
                        modeloNegocio.activo = Convert.ToBoolean(rdr["activo"]);
                        modeloNegocio.tipo_captura_id = Convert.ToInt64(rdr["tipo_captura_id"]);
                        modeloNegocio.unidad_negocio_id = Convert.ToInt64(rdr["unidad_negocio_id"]);
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

            string addModelo = "insert into " + "modelo_negocio"
                + "("
                + "id" + ","
                + "nombre"+"," 
                + "tipo_captura_id, "
                + "unidad_negocio_id, "
                + "activo," 
                + "agrupador" +
                ") " +
                "values " +
                "(nextval('seq_modelo_neg'),@nombre," 
                + "@tipo_captura_id,"
                + "@unidad_negocio_id,"
                + "@activo,@agrupador)";

            try
            {
    
                NpgsqlCommand cmd = new NpgsqlCommand(addModelo, con);
                cmd.Parameters.AddWithValue("@id", modeloNegocio.id);
                cmd.Parameters.AddWithValue("@nombre", modeloNegocio.nombre.Trim());
                cmd.Parameters.AddWithValue("@tipo_captura_id", modeloNegocio.tipo_captura_id);
                cmd.Parameters.AddWithValue("@unidad_negocio_id", modeloNegocio.unidad_negocio_id);
                cmd.Parameters.AddWithValue("@activo", modeloNegocio.activo);
                cmd.Parameters.AddWithValue("@agrupador", modeloNegocio.agrupador);
                //cmd.Parameters.AddWithValue("@FEC_MODIF_MODELONEGOCIO", DateTime.Now);

                con.Open();
                int cantFilas = cmd.ExecuteNonQuery();
                con.Close();
                return cantFilas;

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
        public int Update(string id, Modelo_Negocio modeloNegocio)
        {

            string add = "update modelo_negocio set "
                 + "nombre = @nombre ,"
                 + "activo = @activo ,"
                 + "tipo_captura_id = @tipo_captura_id, "
                 + "unidad_negocio_id = @unidad_negocio_id "
                 + " where id  = " + id;
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@nombre", Value = modeloNegocio.nombre.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Boolean, ParameterName = "@activo", Value = modeloNegocio.activo });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@tipo_captura_id", Value = modeloNegocio.tipo_captura_id });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@unidad_negocio_id", Value = modeloNegocio.unidad_negocio_id });
                con.Open();
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
