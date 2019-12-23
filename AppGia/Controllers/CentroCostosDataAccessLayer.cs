using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.IO;
using AppGia.Conexion;

namespace AppGia.Controllers
{
    public class CentroCostosDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        public CentroCostosDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        char cod = '"';

        public IEnumerable<CentroCostos> GetAllCentros()
        {
            
            string consulta = "SELECT * FROM"+cod+"CAT_CENTROCOSTO"+cod+ "WHERE " + cod + "BOOL_ESTATUS_LOGICO_CENTROCOSTO" + cod + "=" + true; ;
            try
            {
                List<CentroCostos> lstcentros = new List<CentroCostos>();
                {

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);

              
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        CentroCostos centroCC = new CentroCostos();

                        centroCC.STR_TIPO_CC = rdr["STR_TIPO_CC"].ToString();
                        centroCC.INT_IDCENTROCOSTO_P = Convert.ToInt32(rdr["INT_IDCENTROCOSTO_P"]);
                        centroCC.STR_IDCENTROCOSTO = rdr["STR_IDCENTROCOSTO"].ToString();
                        centroCC.STR_NOMBRE_CC = rdr["STR_NOMBRE_CC"].ToString();
                        centroCC.STR_CATEGORIA_CC = rdr["STR_CATEGORIA_CC"].ToString();
                        centroCC.STR_ESTATUS_CC = rdr["STR_ESTATUS_CC"].ToString();
                        centroCC.STR_GERENTE_CC = rdr["STR_GERENTE_CC"].ToString();

              

                        lstcentros.Add(centroCC);
                    }
                    con.Close();
                }
                return lstcentros;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
        }

        public CentroCostos GetCentroData(int id)
        {
            string consulta = "SELECT * FROM" + cod + "CAT_CENTROCOSTO" + cod + "WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "=" + id; ;
            try
            {
                CentroCostos centroCC = new CentroCostos();

               
                {
                    con.Close();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, conex.ConnexionDB());

                    
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        
                        centroCC.STR_TIPO_CC = rdr["STR_TIPO_CC"].ToString();
                        centroCC.INT_IDCENTROCOSTO_P = Convert.ToInt32(rdr["INT_IDCENTROCOSTO_P"]);
                        centroCC.STR_IDCENTROCOSTO = rdr["STR_IDCENTROCOSTO"].ToString();
                        centroCC.STR_NOMBRE_CC = rdr["STR_NOMBRE_CC"].ToString();
                        centroCC.STR_CATEGORIA_CC = rdr["STR_CATEGORIA_CC"].ToString();
                        centroCC.STR_ESTATUS_CC = rdr["STR_ESTATUS_CC"].ToString();
                        centroCC.STR_GERENTE_CC = rdr["STR_GERENTE_CC"].ToString();
                    }

                    con.Close();
                }
                return centroCC;
            }
            catch
            {
                con.Close();
                throw;
               
            }
        }
        public int AddCentro(CentroCostos centroCC)
        {
            string add = "INSERT INTO "+cod+ "CAT_CENTROCOSTO" + cod+"("+cod+ "STR_TIPO_CC"+cod+"," + cod+ "STR_IDCENTROCOSTO" + cod+","+cod+ "STR_NOMBRE_CC" + cod+","+cod+ "STR_CATEGORIA_CC" + cod+","+cod+ "STR_ESTATUS_CC" + cod+","+cod+ "STR_GERENTE_CC" + cod+","+cod+ "FEC_MODIF_CC"+cod+"," + cod+"BOOL_ESTATUS_LOGICO_CENTROCOSTO"+cod+")" +
                "VALUES (@STR_TIPO_CC,@STR_IDCENTROCOSTO,@STR_NOMBRE_CC,@STR_CATEGORIA_CC,@STR_ESTATUS_CC,@STR_GERENTE_CC,@FEC_MODIF_CC,@BOOL_ESTATUS_LOGICO_CENTROCOSTO)";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());

                    cmd.Parameters.AddWithValue("STR_TIPO_CC", centroCC.STR_TIPO_CC);
                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centroCC.STR_IDCENTROCOSTO);
                    cmd.Parameters.AddWithValue("STR_NOMBRE_CC", centroCC.STR_NOMBRE_CC);
                    cmd.Parameters.AddWithValue("STR_CATEGORIA_CC", centroCC.STR_CATEGORIA_CC);
                    cmd.Parameters.AddWithValue("STR_ESTATUS_CC", centroCC.STR_ESTATUS_CC);
                    cmd.Parameters.AddWithValue("STR_GERENTE_CC", centroCC.STR_GERENTE_CC);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_CC", DateTime.Now);
                    cmd.Parameters.AddWithValue("BOOL_ESTATUS_LOGICO_CENTROCOSTO", centroCC.BOOL_ESTATUS_LOGICO_CENTROCOSTO);
                   
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int update(string id, CentroCostos centrocosto)
        {

            string update = "UPDATE " + cod + "CAT_CENTROCOSTO" + cod + "SET"


            + cod + "STR_IDCENTROCOSTO" + cod + " = '" + centrocosto.STR_IDCENTROCOSTO + "' ,"
            + cod + "STR_NOMBRE_CC" + cod + " = '" + centrocosto.STR_NOMBRE_CC + "' ,"
            + cod + "STR_CATEGORIA_CC" + cod + " = '" + centrocosto.STR_CATEGORIA_CC + "' ,"
            + cod + "STR_GERENTE_CC" + cod + " = '" + centrocosto.STR_GERENTE_CC + "' ,"
            + cod + "STR_ESTATUS_CC" + cod + " = '" + centrocosto.STR_ESTATUS_CC + "' ,"
            + cod + "STR_TIPO_CC" + cod + " = '" + centrocosto.STR_TIPO_CC + "'"
 
            + " WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "=" + id;


            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, conex.ConnexionDB());

                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centrocosto.STR_IDCENTROCOSTO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_CC", centrocosto.STR_NOMBRE_CC);
                    cmd.Parameters.AddWithValue("@STR_CATEGORIA_CC", centrocosto.STR_CATEGORIA_CC);
                    cmd.Parameters.AddWithValue("@STR_GERENTE_CC", centrocosto.STR_GERENTE_CC);
                    cmd.Parameters.AddWithValue("@STR_ESTATUS_CC", centrocosto.STR_ESTATUS_CC);
                    cmd.Parameters.AddWithValue("@STR_TIPO_CC", centrocosto.STR_TIPO_CC);

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int Delete(string id)
        {
            bool status = false;
            string delete = "UPDATE " + cod + "CAT_CENTROCOSTO" + cod + "SET" + cod + "BOOL_ESTATUS_LOGICO_CENTROCOSTO" + cod + "='" +status+ "' WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "='" + id + "'";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, conex.ConnexionDB());
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

    }
}
