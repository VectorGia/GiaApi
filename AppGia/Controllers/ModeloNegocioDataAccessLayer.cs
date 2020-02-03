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

        char cod = '"';

        public IEnumerable<Modelo_Negocio> GetAllModeloNegocios()
        {
            string consulta = "select * from modelo_negocio";
            try
            {
                List<Modelo_Negocio> lstmodelo = new List<Modelo_Negocio>();
              
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Modelo_Negocio modeloNegocio = new Modelo_Negocio();
                        modeloNegocio.id = Convert.ToInt32(rdr["id"]);
                        modeloNegocio.nombre = rdr["nombre"].ToString().Trim();
                        //modeloNegocio.nombrer = rdr["nombrer"].ToString().Trim();
                        //modeloNegocio.rangos_cuentas_incluidas = rdr["rangos_cuentas_incluidas"].ToString().Trim();
                       // modeloNegocio.rango_cuentas_excluidas = rdr["rango_cuentas_excluidas"].ToString().Trim();
                        modeloNegocio.activo = Convert.ToBoolean(rdr["activo"]);
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
                Modelo_Negocio negocio = new Modelo_Negocio();
                {
                    string consulta = "select * from " + "modelo_negocio " + "where " + "id " + "=" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        negocio.id = Convert.ToInt32(rdr["id"]);
                        negocio.nombre = rdr["nombre"].ToString().Trim();
                  
                        
                    }
                    con.Close();
                }
                return negocio;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int addModelo(Modelo_Negocio modeloNegocio)
        {
            string addModelo = "insert into " + "modelo_negocio"
                + "("
                + "nombre"+","
                +"activo" + ") " +
                "values " +
                "(@nombre," + 
                "@activo)";

            try
            {
    
                    NpgsqlCommand cmd = new NpgsqlCommand(addModelo, con);
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

        /// <summary>
        /// Update para todos los campos de la tabla Modelo de Negocio
        /// </summary>
        /// <param name="modeloNegocio"></param>
        /// <returns></returns>
        public int UpdateModelo(string id, Modelo_Negocio modeloNegocio)
        {
            string add = "update " + cod + "modelo_negocio" + cod +
                " set " 
                + "nombre" + "= " + "@nombre" + ","
                + cod + "FEC_MODIF_MODELONEGOCIO" + cod + "= " + "@FEC_MODIF_MODELONEGOCIO" 
                + " where " + "id" +  " = " + id;
            try
            {
               // using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBREMODELONEGOCIO", Value = modeloNegocio.nombre.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELONEGOCIO_P", Value = modeloNegocio.id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_MODELONEGOCIO", Value = DateTime.Now});
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;

                //}
                //return 1;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
        }

        public int DeleteModelo(string  id)
        {
            bool status = false;
            string add = "update " + "modelo_negocio" + 
                " set " + "activo" + "= " + status +
                " where " + "id" + " = " + id;
            try
            {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                 
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
