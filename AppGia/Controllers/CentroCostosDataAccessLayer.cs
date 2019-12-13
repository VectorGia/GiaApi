using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CentroCostosDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<CentroCostos> GetAllCentros()
        {

            string consulta = "SELECT * FROM"+cod+"CAT_CENTROCOSTO"+cod+"";
            try
            {
                List<CentroCostos> lstcentros = new List<CentroCostos>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        CentroCostos centroCC = new CentroCostos();
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
            catch
            {
                throw;
            }
        }

        public int AddCentro(CentroCostos centroCC)
        {
            string add = "INSERT INTO "+cod+ "CAT_CENTROCOSTO" + cod+"("+cod+ "STR_IDCENTROCOSTO" + cod+","+cod+ "STR_NOMBRE_CC" + cod+","+cod+ "STR_CATEGORIA_CC" + cod+","+cod+ "STR_ESTATUS_CC" + cod+","+cod+ "STR_GERENTE_CC" + cod+")"  +
                "VALUES (@STR_IDCENTROCOSTO,@STR_NOMBRE_CC,@STR_CATEGORIA_CC,@STR_ESTATUS_CC,@STR_GERENTE_CC)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);


                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centroCC.STR_IDCENTROCOSTO);
                    cmd.Parameters.AddWithValue("STR_NOMBRE_CC", centroCC.STR_NOMBRE_CC);
                    cmd.Parameters.AddWithValue("STR_CATEGORIA_CC", centroCC.STR_CATEGORIA_CC);
                    cmd.Parameters.AddWithValue("STR_ESTATUS_CC", centroCC.STR_ESTATUS_CC);
                    cmd.Parameters.AddWithValue("STR_GERENTE_CC", centroCC.STR_GERENTE_CC);
                    

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

        public int update(CentroCostos centrocosto)
        {

            string update = "UPDATE " + cod + "CAT_CENTROCOSTO" + cod + "SET"


            + cod + "STR_IDCENTROCOSTO" + cod + " = '" + centrocosto.STR_IDCENTROCOSTO + "' ,"
            + cod + "STR_NOMBRE_CC" + cod + " = '" + centrocosto.STR_NOMBRE_CC + "' ,"
            + cod + "STR_CATEGORIA_CC" + cod + " = '" + centrocosto.STR_CATEGORIA_CC + "' ,"
            + cod + "STR_GERENTE_CC" + cod + " = '" + centrocosto.STR_GERENTE_CC + "' ,"
            + cod + "STR_ESTATUS_CC" + cod + " = '" + centrocosto.STR_ESTATUS_CC + "' ,"
            + cod + "STR_TIPO_CC" + cod + " = '" + centrocosto.STR_TIPO_CC + "' ,"
            + cod + "FEC_MODIF_CC" + cod + " = '" + centrocosto.FEC_MODIF_CC + "' ,"
            + cod + "BOOL_ESTATUS_LOGICO_CENTROCOSTO" + cod + "= '" + centrocosto.BOOL_ESTATUS_LOGICO_CENTROCOSTO + "' "
            + " WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "=" + centrocosto.INT_IDCENTROCOSTO_P;


            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centrocosto.STR_IDCENTROCOSTO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_CC", centrocosto.STR_NOMBRE_CC);
                    cmd.Parameters.AddWithValue("@STR_CATEGORIA_CC", centrocosto.STR_CATEGORIA_CC);
                    cmd.Parameters.AddWithValue("@STR_GERENTE_CC", centrocosto.STR_GERENTE_CC);
                    cmd.Parameters.AddWithValue("@STR_ESTATUS_CC", centrocosto.STR_ESTATUS_CC);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_CENTROCOSTO", centrocosto.BOOL_ESTATUS_LOGICO_CENTROCOSTO);

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

        public int Delete(CentroCostos centrocosto)
        {

            string delete = "UPDATE " + cod + "CAT_CENTROCOSTO" + cod + "SET" + cod + "BOOL_ESTATUS_LOGICO_CENTROCOSTO" + cod + "='" + "' WHERE" + cod + "INT_IDCENTROCOSTO_P" + cod + "='" + centrocosto.INT_IDCENTROCOSTO_P + "'";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", centrocosto.BOOL_ESTATUS_LOGICO_CENTROCOSTO);

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

    }
}
