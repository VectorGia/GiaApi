using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CentroCostosDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=Gia;Pooling=true;";
        char cod = '"';
        public IEnumerable<CentroCostos> GetAllCentros()
        {

            string consulta = "SELECT * FROM"+cod+"CentroCostos"+cod+"";
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

                        centroCC.STR_IDCENTROCOSTO = rdr["STR_IDCENTROCOSTO"].ToString();
                        centroCC.STR_NOMBRE_CC = rdr["STR_NOMBRE_CC"].ToString();
                        centroCC.STR_CATEGORIA_CC = rdr["STR_CATEGORIA_CC"].ToString();
                        centroCC.BOOL_eSTATUS_CC = rdr["BOOL_eSTATUS_CC"].ToString();
                        centroCC.GERENTE__COMPANIA = rdr["GERENTE__COMPANIA"].ToString();

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
            string add = "INSERT INTO "+cod+ "CAT_CENTROCOSTO" + cod+"("+cod+ "STR_IDCENTROCOSTO" + cod+","+cod+ "STR_NOMBRE_CC" + cod+","+cod+ "STR_CATEGORIA_CC" + cod+","+cod+ "BOOL_eSTATUS_CC" + cod+","+cod+ "GERENTE__COMPANIA" + cod+")"  +
                "VALUES (@STR_IDCENTROCOSTO,@STR_NOMBRE_CC,@STR_CATEGORIA_CC,@BOOL_eSTATUS_CC,@GERENTE__COMPANIA)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);


                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centroCC.STR_IDCENTROCOSTO);
                    cmd.Parameters.AddWithValue("STR_NOMBRE_CC", centroCC.STR_NOMBRE_CC);
                    cmd.Parameters.AddWithValue("STR_CATEGORIA_CC", centroCC.STR_CATEGORIA_CC);
                    cmd.Parameters.AddWithValue("BOOL_eSTATUS_CC", centroCC.BOOL_eSTATUS_CC);
                    cmd.Parameters.AddWithValue("GERENTE__COMPANIA", centroCC.GERENTE__COMPANIA);

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

        public int UpdateCentro(CentroCostos centroCC)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spUpdateCentroCostos", con);

                    cmd.Parameters.AddWithValue("@STR_IDCENTROCOSTO", centroCC.STR_IDCENTROCOSTO);
                    cmd.Parameters.AddWithValue("STR_NOMBRE_CC", centroCC.STR_NOMBRE_CC);
                    cmd.Parameters.AddWithValue("STR_CATEGORIA_CC", centroCC.STR_CATEGORIA_CC);
                    cmd.Parameters.AddWithValue("BOOL_eSTATUS_CC", centroCC.BOOL_eSTATUS_CC);
                    cmd.Parameters.AddWithValue("GERENTE__COMPANIA", centroCC.GERENTE__COMPANIA);

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

        public int DeleteCentro(int id)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);
                    

                    cmd.Parameters.AddWithValue("STR_IDCENTROCOSTO", id);

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
