using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class GrupoDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Grupo> GetAllGrupos()
        {
            string cadena = "SELECT * FROM"+cod+"TAB_GRUPO"+cod+""; 
            try
            {
                List<Grupo> lstgrupo = new List<Grupo>();
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena,con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Grupo grupo = new Grupo();
                        grupo.STR_NOMBRE_GRUPO = rdr["STR_NOMBRE_GRUPO"].ToString();

                        lstgrupo.Add(grupo);
                    }
                    con.Close();
                }

                return lstgrupo;
            }
            catch
            {
                throw;
            }
        }

        public int addGrupo(Grupo grupo)
        {
            string add = "INSERT INTO"+cod+"TAB_GRUPO"+cod+"("+cod+"STR_NOMBRE_GRUPO" +cod+") VALUES " +
                "(@STR_NOMBRE_GRUPO)";
            try
            {
                using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    //cmd.Parameters.AddWithValue("@INT_IDGRUPO", grupo.INT_IDGRUPO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_GRUPO", grupo.STR_NOMBRE_GRUPO);

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
