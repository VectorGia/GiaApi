﻿using System;
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

        public IEnumerable<ModeloNegocio> GetAllModeloNegocios()
        {
            string consulta = "SELECT * FROM" +cod+ "TAB_MODELO_NEGOCIO" +cod+ "WHERE " + cod + "BOOL_ESTATUS_LOGICO_MODE_NEGO" + cod + "=" + true; ;
            try
            {
                List<ModeloNegocio> lstmodelo = new List<ModeloNegocio>();
              
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ModeloNegocio modeloNegocio = new ModeloNegocio();
                        modeloNegocio.INT_IDMODELONEGOCIO_P = Convert.ToInt32(rdr["INT_IDMODELONEGOCIO_P"]);
                        modeloNegocio.STR_NOMBREMODELONEGOCIO = rdr["STR_NOMBREMODELONEGOCIO"].ToString().Trim();
<<<<<<< HEAD
                        modeloNegocio.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString().Trim();
                        modeloNegocio.STR_CUENTASMODELO = rdr["STR_CUENTASMODELO"].ToString().Trim();
                        modeloNegocio.STR_TIPOMONTO = rdr["STR_TIPOMONTO"].ToString().Trim();
                        lstmodelo.Add(modeloNegocio);
                    }
                    // con.Close();
                  
                //}
=======
                        modeloNegocio.STR_CUENTASMODELO = rdr["STR_CUENTASMODELO"].ToString().Trim();
                        modeloNegocio.STR_TIPOMONTO = rdr["STR_TIPOMONTO"].ToString().Trim();
                        modeloNegocio.BOOL_ESTATUS_LOGICO_MODE_NEGO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_MODE_NEGO"]);
                        lstmodelo.Add(modeloNegocio);
                    }
                   con.Close();
                   
>>>>>>> juan
                return lstmodelo;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public ModeloNegocio GetModelo(string id)
        {
            try
            {
                ModeloNegocio negocio = new ModeloNegocio();
                {
                    string consulta = "SELECT * FROM" + cod + "TAB_MODELO_NEGOCIO" + cod + "WHERE" + cod + "INT_IDMODELONEGOCIO_P" + cod + "=" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        negocio.INT_IDMODELONEGOCIO_P = Convert.ToInt32(rdr["INT_IDMODELONEGOCIO_P"]);
                        negocio.STR_NOMBREMODELONEGOCIO = rdr["STR_NOMBREMODELONEGOCIO"].ToString().Trim();
                        negocio.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString().Trim();
                        negocio.STR_CUENTASMODELO = rdr["STR_CUENTASMODELO"].ToString().Trim();
                        negocio.STR_TIPOMONTO = rdr["STR_TIPOMONTO"].ToString().Trim();
                        
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

        public int addModelo(ModeloNegocio modeloNegocio)
        {
            string addModelo = "INSERT INTO"+cod+"TAB_MODELO_NEGOCIO"
                +cod+"("
                +cod+ "STR_NOMBREMODELONEGOCIO"+cod+","
                +cod+ "STR_IDCOMPANIA"         +cod+","
                +cod+ "STR_CUENTASMODELO"      +cod+","
                +cod+ "STR_TIPOMONTO"          +cod+"," 
                +cod+ "BOOL_ESTATUS_LOGICO_MODE_NEGO" +cod+"," 
                +cod+ "FEC_MODIF_MODELONEGOCIO" +cod+ ") " +
                "VALUES " +
                "(@STR_NOMBREMODELONEGOCIO,@STR_IDCOMPANIA," +
                "@STR_CUENTASMODELO,@STR_TIPOMONTO," +
                "@BOOL_ESTATUS_LOGICO_MODE_NEGO, @FEC_MODIF_MODELONEGOCIO)";

            try
            {
<<<<<<< HEAD
                con.Open();
               // using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
               // {
                    NpgsqlCommand cmd = new NpgsqlCommand(addModelo, conex.ConnexionDB());
                    cmd.Parameters.AddWithValue("@STR_NOMBREMODELONEGOCIO", modeloNegocio.STR_NOMBREMODELONEGOCIO.Trim());
                    cmd.Parameters.AddWithValue("@STR_TIPOMONTO", modeloNegocio.STR_TIPOMONTO.Trim());
                    cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", modeloNegocio.STR_IDCOMPANIA.Trim());
=======
                
                    NpgsqlCommand cmd = new NpgsqlCommand(addModelo, con);
                    cmd.Parameters.AddWithValue("@STR_NOMBREMODELONEGOCIO", modeloNegocio.STR_NOMBREMODELONEGOCIO.Trim());
                cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", modeloNegocio.STR_IDCOMPANIA.Trim());
                    cmd.Parameters.AddWithValue("@STR_TIPOMONTO", modeloNegocio.STR_TIPOMONTO.Trim());
>>>>>>> juan
                    cmd.Parameters.AddWithValue("@STR_CUENTASMODELO", modeloNegocio.STR_CUENTASMODELO.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_MODE_NEGO", modeloNegocio.BOOL_ESTATUS_LOGICO_MODE_NEGO);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_MODELONEGOCIO", DateTime.Now);

<<<<<<< HEAD
                    
                    int cantFilas = cmd.ExecuteNonQuery();
                   
=======
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
>>>>>>> juan
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
        public int UpdateModelo(string id, ModeloNegocio modeloNegocio)
        {
            string add = "UPDATE " + cod + "TAB_MODELO_NEGOCIO" + cod +
                " SET " 
                + cod + "STR_NOMBREMODELONEGOCIO" + cod + "= " + "@STR_NOMBREMODELONEGOCIO" + ","
                + cod + "STR_CUENTASMODELO"       + cod + "= " + "@STR_CUENTASMODELO" + ","
                + cod + "STR_TIPOMONTO"           + cod + "= " + "@STR_TIPOMONTO" + ","
                + cod + "BOOL_ESTATUS_LOGICO_MODE_NEGO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_MODE_NEGO" + ","
                + cod + "FEC_MODIF_MODELONEGOCIO" + cod + "= " + "@FEC_MODIF_MODELONEGOCIO" 
                + " WHERE " + cod + "INT_IDMODELONEGOCIO_P" + cod + " = " + id;
            try
            {
               // using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
<<<<<<< HEAD
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBREMODELONEGOCIO", Value = modeloNegocio.STR_NOMBREMODELONEGOCIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CUENTASMODELO", Value = modeloNegocio.STR_CUENTASMODELO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_TIPOMONTO", Value = modeloNegocio.STR_TIPOMONTO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_IDCOMPANIA", Value = modeloNegocio.STR_IDCOMPANIA.Trim() });
=======
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBREMODELONEGOCIO", Value = modeloNegocio.STR_NOMBREMODELONEGOCIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_CUENTASMODELO", Value = modeloNegocio.STR_CUENTASMODELO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_TIPOMONTO", Value = modeloNegocio.STR_TIPOMONTO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "BOOL_ESTATUS_LOGICO_MODE_NEGO", Value = modeloNegocio.BOOL_ESTATUS_LOGICO_MODE_NEGO });
>>>>>>> juan
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELONEGOCIO_P", Value = modeloNegocio.INT_IDMODELONEGOCIO_P });
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
            string add = "UPDATE " + cod + "TAB_MODELO_NEGOCIO" + cod +
                " SET " + cod + "BOOL_ESTATUS_LOGICO_MODE_NEGO" + cod + "= " + status +
                " WHERE " + cod + "INT_IDMODELONEGOCIO_P" + cod + " = " + id;
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
