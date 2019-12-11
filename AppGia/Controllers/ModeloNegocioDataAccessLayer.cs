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
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';

        public IEnumerable<ModeloNegocio> GetAllModeloNegocios()
        {
            string consulta = "SELECT * FROM" +cod+ "TAB_MODELO_NEGOCIO" +cod+"";
            try
            {
                List<ModeloNegocio> lstmodelo = new List<ModeloNegocio>();
                using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ModeloNegocio modeloNegocio = new ModeloNegocio();
                        modeloNegocio.STR_NOMBREMODELONEGOCIO = rdr["STR_NOMBREMODELONEGOCIO"].ToString();

                        lstmodelo.Add(modeloNegocio);
                    }
                    con.Close();
                }
                return lstmodelo;
            }
            catch
            {
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
                using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(addModelo, con);
                    cmd.Parameters.AddWithValue("@STR_NOMBREMODELONEGOCIO", modeloNegocio.STR_NOMBREMODELONEGOCIO);
                    cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", modeloNegocio.STR_IDCOMPANIA);
                    cmd.Parameters.AddWithValue("@STR_CUENTASMODELO", modeloNegocio.STR_CUENTASMODELO);
                    cmd.Parameters.AddWithValue("@STR_TIPOMONTO", modeloNegocio.STR_TIPOMONTO);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Update para todos los campos de la tabla Modelo de Negocio
        /// </summary>
        /// <param name="modeloNegocio"></param>
        /// <returns></returns>
        public int UpdateModelo(ModeloNegocio modeloNegocio)
        {
            string add = "UPDATE " + cod + "TAB_MODELO_NEGOCIO" + cod +
                " SET " + cod + "STR_NOMBREMODELONEGOCIO" + cod + "= " + "@STR_NOMBREMODELONEGOCIO" + ","
                + cod + "STR_CUENTASMODELO" + cod + "= " + "@STR_CUENTASMODELO" + ","
                + cod + "STR_TIPOMONTO" + cod + "= " + "@STR_TIPOMONTO" + ","
                + cod + "STR_IDCOMPANIA" + cod + "= " + "@STR_IDCOMPANIA" + ","
                + cod + "INT_COMPANIA_F" + cod + "= " + "@INT_COMPANIA_F"
                + " WHERE " + cod + "INT_IDMODELONEGOCIO_P" + cod + " = " + "@INT_IDMODELONEGOCIO_P";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBREMODELONEGOCIO", Value = modeloNegocio.STR_NOMBREMODELONEGOCIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CUENTASMODELO", Value = modeloNegocio.STR_CUENTASMODELO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_TIPOMONTO", Value = modeloNegocio.STR_TIPOMONTO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_IDCOMPANIA", Value = modeloNegocio.STR_IDCOMPANIA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_COMPANIA_F", Value = modeloNegocio.INT_COMPANIA_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELONEGOCIO_P", Value = modeloNegocio.INT_IDMODELONEGOCIO_P });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch (Exception ex)
            {
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
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELONEGOCIO_P", Value = modeloNegocio.INT_IDMODELONEGOCIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_MODE_NEGO", Value = modeloNegocio.BOOL_ESTATUS_LOGICO_MODE_NEGO });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
    }
}
