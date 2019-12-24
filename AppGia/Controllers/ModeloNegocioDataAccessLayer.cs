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

        //private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';

        public IEnumerable<ModeloNegocio> GetAllModeloNegocios()
        {
            string consulta = "SELECT * FROM" +cod+ "TAB_MODELO_NEGOCIO" +cod+"";
            try
            {
                List<ModeloNegocio> lstmodelo = new List<ModeloNegocio>();
                //using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ModeloNegocio modeloNegocio = new ModeloNegocio();
                        modeloNegocio.INT_IDMODELONEGOCIO_P = Convert.ToInt32(rdr["INT_IDMODELONEGOCIO_P"]);
                        modeloNegocio.STR_NOMBREMODELONEGOCIO = rdr["STR_NOMBREMODELONEGOCIO"].ToString();
                        modeloNegocio.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString();
                        modeloNegocio.STR_CUENTASMODELO = rdr["STR_CUENTASMODELO"].ToString();
                        modeloNegocio.STR_TIPOMONTO = rdr["STR_TIPOMONTO"].ToString();
                        lstmodelo.Add(modeloNegocio);
                    }
                    // con.Close();
                    conex.ConnexionDB().Close();
                //}
                return lstmodelo;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int addModelo(ModeloNegocio modeloNegocio)
        {
            string addModelo = "INSERT INTO"+cod+"TAB_MODELO_NEGOCIO"+cod+"("+cod+ "STR_NOMBREMODELONEGOCIO" + cod
                +","+cod+"STR_IDCOMPANIA"+cod+","+cod+ "STR_CUENTASMODELO" + cod+","+cod+ "STR_TIPOMONTO" + cod+") VALUES " +
                "(@STR_NOMBREMODELONEGOCIO,@STR_IDCOMPANIA,@STR_CUENTASMODELO,@STR_TIPOMONTO)";

            try
            {
                
               // using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
               // {
                    NpgsqlCommand cmd = new NpgsqlCommand(addModelo, conex.ConnexionDB());
                    cmd.Parameters.AddWithValue("@STR_NOMBREMODELONEGOCIO", modeloNegocio.STR_NOMBREMODELONEGOCIO);
                    cmd.Parameters.AddWithValue("@STR_TIPOMONTO", modeloNegocio.STR_TIPOMONTO);
                    cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", modeloNegocio.STR_IDCOMPANIA);
                    cmd.Parameters.AddWithValue("@STR_CUENTASMODELO", modeloNegocio.STR_CUENTASMODELO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_MODE_NEGO", modeloNegocio.BOOL_ESTATUS_LOGICO_MODE_NEGO);

                    conex.ConnexionDB().Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    conex.ConnexionDB().Close();
                    return cantFilas;
                //}
                //return 1;
            }
            catch
            {
                conex.ConnexionDB().Close();
                throw;
            }
        }

        /// <summary>
        /// Update para todos los campos de la tabla Modelo de Negocio
        /// </summary>
        /// <param name="modeloNegocio"></param>
        /// <returns></returns>
        public int UpdateModelo( ModeloNegocio modeloNegocio)
        {
            string add = "UPDATE " + cod + "TAB_MODELO_NEGOCIO" + cod +
                " SET " + cod + "STR_NOMBREMODELONEGOCIO" + cod + "= " + "@STR_NOMBREMODELONEGOCIO" + ","
                + cod + "STR_CUENTASMODELO" + cod + "= " + "@STR_CUENTASMODELO" + ","
                + cod + "STR_TIPOMONTO" + cod + "= " + "@STR_TIPOMONTO" + ","
                + cod + "STR_IDCOMPANIA" + cod + "= " + "@STR_IDCOMPANIA" 
                + " WHERE " + cod + "INT_IDMODELONEGOCIO_P" + cod + " = " + "@INT_IDMODELONEGOCIO_P";
            try
            {
               // using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBREMODELONEGOCIO", Value = modeloNegocio.STR_NOMBREMODELONEGOCIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CUENTASMODELO", Value = modeloNegocio.STR_CUENTASMODELO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_TIPOMONTO", Value = modeloNegocio.STR_TIPOMONTO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_IDCOMPANIA", Value = modeloNegocio.STR_IDCOMPANIA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELONEGOCIO_P", Value = modeloNegocio.INT_IDMODELONEGOCIO_P });
                    conex.ConnexionDB().Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    conex.ConnexionDB().Close();
                    return cantFilas;

                //}
                //return 1;
            }
            catch (Exception ex)
            {
                conex.ConnexionDB().Close();
                string error = ex.Message;
                throw;
            }
        }

        public int DeleteModelo(ModeloNegocio modeloNegocio)
        {
            string add = "UPDATE " + cod + "TAB_MODELO_NEGOCIO" + cod +
                " SET " + cod + "BOOL_ESTATUS_LOGICO_MODE_NEGO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_MODE_NEGO" +
                " WHERE " + cod + "INT_IDMODELONEGOCIO_P" + cod + " = " + "@INT_IDMODELONEGOCIO_P";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELONEGOCIO_P", Value = modeloNegocio.INT_IDMODELONEGOCIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_MODE_NEGO", Value = modeloNegocio.BOOL_ESTATUS_LOGICO_MODE_NEGO });
                    conex.ConnexionDB().Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    conex.ConnexionDB().Close();
                    return cantFilas;
                //}
                //return 1;
            }
            catch (Exception ex)
            {
                conex.ConnexionDB().Close();
                string error = ex.Message;
                throw;
            }
        }
    }
}
