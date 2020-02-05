using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;
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

            string consulta = "select * from modelo_negocio where activo = " + true + " order by id desc";

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
        }
        public Modelo_Negocio GetModelo(string id)
        {
            try
            {
                Modelo_Negocio modeloNegocio = new Modelo_Negocio();
                {
                    string consulta = "select id,activo,nombre,tipo_captura_id from modelo_negocio  where id = " + id;
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
        }
        public int addModeloNegocio(Modelo_Negocio modeloNegocio)
        {

            string addModelo = "insert into " + "modelo_negocio"
                + "("
                + "id" + ","
                + "nombre"+","
                +"activo" + ") " +
                "values " +
                "(nextval('seq_modelo_neg'),@nombre," + 
                "@activo)";

            try
            {
    
                    NpgsqlCommand cmd = new NpgsqlCommand(addModelo, con);
                cmd.Parameters.AddWithValue("@id", modeloNegocio.id);
                    cmd.Parameters.AddWithValue("@nombre", modeloNegocio.nombre.Trim());
                    cmd.Parameters.AddWithValue("@activo", modeloNegocio.activo);
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
        }
        public int Update(string id, Modelo_Negocio modeloNegocio)
        {

            string add = "update modelo_negocio set "
                 + "nombre = @nombre ,"
                 + "activo = @activo ,"
                 + "tipo_captura_id = @tipo_captura_id "
                 + " where id  = " + id;
            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@nombre", Value = modeloNegocio.nombre.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = modeloNegocio.activo });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@tipo_captura_id", Value = modeloNegocio.tipo_captura_id });
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
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = status });
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
        }
    }
}
