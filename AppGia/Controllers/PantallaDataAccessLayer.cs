using System.Collections.Generic;
using AppGia.Models;
using Npgsql;


namespace AppGia.Controllers
{
    public class PantallaDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Pantalla> GetAllPantallas()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PANTALLA" + cod + "";
            try
            {
                List<Pantalla> lstpantalla = new List<Pantalla>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Pantalla pantalla = new Pantalla();
                        pantalla.STR_NOMBRE_PANTALLA = rdr["STR_NOMBRE_PANTALLA"].ToString();

                        lstpantalla.Add(pantalla);
                    }
                    con.Close();
                }

                return lstpantalla;
            }
            catch
            {
                throw;
            }
        }

        public int addPantalla(Pantalla pantalla)
        {
            string add = "INSERT INTO" + cod + "TAB_PANTALLA" + cod + "(" + cod + "STR_NOMBRE_PANTALLA" + cod + ") VALUES " +
                "(@STR_NOMBRE_PANTALLA)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PANTALLA", pantalla.STR_NOMBRE_PANTALLA);

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

        public int UpdatePantalla(Pantalla pantalla)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spUpdateCentroCostos", con);

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PANTALLA", pantalla.STR_NOMBRE_PANTALLA);

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

        public int DeletePantalla(int id)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


                    cmd.Parameters.AddWithValue("STR_NOMBRE_PANTALLA", id);

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
