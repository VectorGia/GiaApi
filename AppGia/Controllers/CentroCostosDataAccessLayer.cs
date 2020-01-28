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
            //Obtiene todos los centros de costos habilitados "TRUE"
            string consulta = "SELECT * FROM "+cod+"CAT_CENTROCOSTO"+cod+ " WHERE " + cod + "BOOL_ESTATUS_LOGICO_CENTROCOSTO" + cod + "=" + true; ;
            try
            {
                List<CentroCostos> lstcentros = new List<CentroCostos>();
                {

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);

              
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        CentroCostos centroCostos = new CentroCostos();

                        centroCostos.INT_IDCENTROCOSTO_P = Convert.ToInt32(rdr["INT_IDCENTROCOSTO_P"]);
                        centroCostos.STR_TIPO_CC = rdr["STR_TIPO_CC"].ToString().Trim();
                        centroCostos.STR_IDCENTROCOSTO = rdr["STR_IDCENTROCOSTO"].ToString().Trim();
                        centroCostos.STR_NOMBRE_CC = rdr["STR_NOMBRE_CC"].ToString().Trim();
                        centroCostos.STR_CATEGORIA_CC = rdr["STR_CATEGORIA_CC"].ToString().Trim();
                        centroCostos.STR_ESTATUS_CC = rdr["STR_ESTATUS_CC"].ToString().Trim();
                        centroCostos.STR_GERENTE_CC = rdr["STR_GERENTE_CC"].ToString().Trim();
                        centroCostos.FEC_MODIF_CC = Convert.ToDateTime(rdr["FEC_MODIF_CC"]);




                        lstcentros.Add(centroCostos);
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

        //Obtiene los centro de costos por identificador unico 
        public CentroCostos GetCentroData(string id)
        {
            string consulta = "SELECT * FROM" + cod + "CAT_CENTROCOSTO" + cod + "WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "=" + id ;
            try
            {
                    CentroCostos centro = new CentroCostos();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta,con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        centro.INT_IDCENTROCOSTO_P = Convert.ToInt32(rdr["INT_IDCENTROCOSTO_P"]);
                        centro.STR_TIPO_CC = rdr["STR_TIPO_CC"].ToString().Trim();
                        centro.STR_IDCENTROCOSTO = rdr["STR_IDCENTROCOSTO"].ToString().Trim();
                        centro.STR_NOMBRE_CC = rdr["STR_NOMBRE_CC"].ToString().Trim();
                        centro.STR_CATEGORIA_CC = rdr["STR_CATEGORIA_CC"].ToString().Trim();
                        centro.STR_ESTATUS_CC = rdr["STR_ESTATUS_CC"].ToString().Trim();
                        centro.STR_GERENTE_CC = rdr["STR_GERENTE_CC"].ToString().Trim();
                        centro.FEC_MODIF_CC = Convert.ToDateTime(rdr["FEC_MODIF_CC"]);

                    }

                    con.Close();
                }
                return centro;
            }
            catch
            {
                con.Close();
                throw;
               
            }
        }
        public int AddCentro(CentroCostos centroCostos)
        {
            string add = "INSERT INTO "+cod+ "CAT_CENTROCOSTO" + cod+"("+cod+ "STR_TIPO_CC"+cod+"," + cod+ "STR_IDCENTROCOSTO" + cod+","+cod+ "STR_NOMBRE_CC" + cod+","+cod+ "STR_CATEGORIA_CC" + cod+","+cod+ "STR_ESTATUS_CC" + cod+","+cod+ "STR_GERENTE_CC" + cod+","+cod+ "FEC_MODIF_CC"+cod+"," + cod+"BOOL_ESTATUS_LOGICO_CENTROCOSTO"+cod+")" +
                "VALUES (@STR_TIPO_CC,@STR_IDCENTROCOSTO,@STR_NOMBRE_CC,@STR_CATEGORIA_CC,@STR_ESTATUS_CC,@STR_GERENTE_CC,@FEC_MODIF_CC,@BOOL_ESTATUS_LOGICO_CENTROCOSTO)";
            try
            {
               
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("STR_TIPO_CC", centroCostos.STR_TIPO_CC.Trim());
                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centroCostos.STR_IDCENTROCOSTO.Trim());
                    cmd.Parameters.AddWithValue("STR_NOMBRE_CC", centroCostos.STR_NOMBRE_CC.Trim());
                    cmd.Parameters.AddWithValue("STR_CATEGORIA_CC", centroCostos.STR_CATEGORIA_CC.Trim());
                    cmd.Parameters.AddWithValue("STR_ESTATUS_CC", centroCostos.STR_ESTATUS_CC.Trim());
                    cmd.Parameters.AddWithValue("STR_GERENTE_CC", centroCostos.STR_GERENTE_CC.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF_CC", DateTime.Now);
                    cmd.Parameters.AddWithValue("BOOL_ESTATUS_LOGICO_CENTROCOSTO", centroCostos.BOOL_ESTATUS_LOGICO_CENTROCOSTO);


                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int update(string id, CentroCostos centroCostos)
        {

            string update = "UPDATE " + cod + "CAT_CENTROCOSTO" + cod + "SET"


            + cod + "STR_IDCENTROCOSTO" + cod + " = '" + centroCostos.STR_IDCENTROCOSTO + "' ,"
            + cod + "STR_NOMBRE_CC" + cod + " = '" + centroCostos.STR_NOMBRE_CC + "' ,"
            + cod + "STR_CATEGORIA_CC" + cod + " = '" + centroCostos.STR_CATEGORIA_CC + "' ,"
            + cod + "STR_GERENTE_CC" + cod + " = '" + centroCostos.STR_GERENTE_CC + "' ,"
            + cod + "STR_ESTATUS_CC" + cod + " = '" + centroCostos.STR_ESTATUS_CC + "' ,"
            + cod + "FEC_MODIF_CC" + cod + " = " + "@FEC_MODIF_CC" + " ,"
            + cod + "STR_TIPO_CC" + cod + " = '" + centroCostos.STR_TIPO_CC + "'"
 
            + " WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "=" + id;


            try
            {
                {
                    
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centroCostos.STR_IDCENTROCOSTO.Trim());
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_CC", centroCostos.STR_NOMBRE_CC.Trim());
                    cmd.Parameters.AddWithValue("@STR_CATEGORIA_CC", centroCostos.STR_CATEGORIA_CC.Trim());
                    cmd.Parameters.AddWithValue("@STR_GERENTE_CC", centroCostos.STR_GERENTE_CC.Trim());
                    cmd.Parameters.AddWithValue("@STR_ESTATUS_CC", centroCostos.STR_ESTATUS_CC.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF_CC", DateTime.Now);
                    cmd.Parameters.AddWithValue("@STR_TIPO_CC", centroCostos.STR_TIPO_CC.Trim());
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }

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
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }

            }
            catch
            {
                con.Close();
                throw;
            }
        }

    }
}
