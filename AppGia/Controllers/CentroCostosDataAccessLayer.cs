using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CentroCostosDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GiaPrueba;Pooling=true;";
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
                        CentroCostos centro = new CentroCostos();
                        centro.id_cc = rdr["id_cc"].ToString();
                        centro.name_cc = rdr["name_cc"].ToString();
                        centro.categoria = rdr["categoria"].ToString();
                        centro.estatus = rdr["estatus"].ToString();
                        centro.gerente = rdr["gerente"].ToString();
                        centro.id_empresa = rdr["id_empresa"].ToString();
                        centro.id_proyecto = rdr["id_proyecto"].ToString();

                        lstcentros.Add(centro);
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

        public int AddCentro(CentroCostos centro)
        {
            string add = "INSERT INTO "+cod+"CentroCostos"+cod+"("+cod+"id_cc"+cod+","+cod+"name_cc"+cod+","+cod+"categoria"+cod+","+cod+"estatus"+cod+","+cod+"gerente"+cod+","+cod+"id_empresa"+cod+","+cod+"id_proyecto"+cod+")"  +
                "VALUES (@id_cc,@name_cc,@categoria,@estatus,@gerente,@id_empresa,@id_proyecto)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
              

                    cmd.Parameters.AddWithValue("@id_cc", centro.id_cc);
                    cmd.Parameters.AddWithValue("@name_cc", centro.name_cc);
                    cmd.Parameters.AddWithValue("@categoria", centro.categoria);
                    cmd.Parameters.AddWithValue("@estatus", centro.estatus);
                    cmd.Parameters.AddWithValue("@gerente", centro.gerente);
                    cmd.Parameters.AddWithValue("@id_empresa", centro.id_empresa);
                    cmd.Parameters.AddWithValue("@id_proyecto", centro.id_proyecto);
                 

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

        public int UpdateCentro(CentroCostos centro)
        {
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spUpdateCentroCostos", con);
                    

                    cmd.Parameters.AddWithValue("@id_cc", centro.id_cc);
                    cmd.Parameters.AddWithValue("@name_cc", centro.name_cc);
                    cmd.Parameters.AddWithValue("@categoria", centro.categoria);
                    cmd.Parameters.AddWithValue("@estatus", centro.categoria);
                    cmd.Parameters.AddWithValue("@gerente", centro.gerente);
                    cmd.Parameters.AddWithValue("@id_empresa", centro.id_empresa);
                    cmd.Parameters.AddWithValue("@id_proyecto", centro.id_proyecto);

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
                    

                    cmd.Parameters.AddWithValue("id_cc", id);

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
