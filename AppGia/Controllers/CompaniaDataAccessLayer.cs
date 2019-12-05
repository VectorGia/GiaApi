using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CompaniaDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';

        public IEnumerable<Compania> GetAllCompanias()
        {
            string consulta = "SELECT * FROM" + cod + "CentroCostos" + cod + "";
            try
            {
                List<Compania> lstcompania = new List<Compania>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Compania compania = new Compania();
                        compania.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString();
                        compania.STR_NOMBRE_COMPANIA = rdr["STR_NOMBRE_COMPANIA"].ToString();
                        compania.STR_ABREV_COMPANIA = rdr["STR_ABREV_COMPANIA"].ToString();
                        compania.BOOL_ETL_COMPANIA = Convert.ToBoolean(rdr["BOOL_ETL_COMPANIA"]);
                        compania.STR_HOST_COMPANIA = rdr["STR_HOST_COMPANIA"].ToString();
                        compania.STR_PUERTO_COMPANIA = rdr["STR_PUERTO_COMPANIA"].ToString();
                        compania.STR_USUARIO_ETL = rdr["STR_USUARIO_ETL"].ToString();
                        compania.STR_CONTRASENIA_ETL = rdr["STR_CONTRASENIA_ETL"].ToString();
                        compania.STR_BD_COMPANIA = rdr["STR_BD_COMPANIA"].ToString();
                        compania.STR_MONEDA_COMPANIA = rdr["STR_MONEDA_COMPANIA"].ToString();

                        lstcompania.Add(compania);
                    }
                    con.Close();
                }
                return lstcompania;
            }
            catch
            {
                throw;
            }
        }
    }
}
