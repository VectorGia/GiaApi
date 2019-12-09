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
            string consulta = "SELECT * FROM" +cod+ "TBL_MODELO" +cod+"";
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
                        modeloNegocio.STR_NOMBRE = rdr["STR_NOMBRE"].ToString();

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
    }
}
