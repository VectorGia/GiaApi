using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class ETLProgDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public ETLProgDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<ETLProg> GetAllETLProg()
        {
            string cadena = "SELECT * FROM " +cod+ "TAB_ETL_PROG";
            try
            {
                List<ETLProg> lstETL = new List<ETLProg>();
                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ETLProg etl = new ETLProg();
                    etl.ID_ETL_PROG = Convert.ToInt32(rdr["INT_ID_ETL_PROG"]);
                    etl.INT_ID_EMPRESA = Convert.ToInt32(rdr["INT_ID_EMPRESA"]);
                    etl.TEXT_FECH_EXTR = rdr["TEXT_FECH_EXTR"].ToString().Trim();
                    etl.TEXT_HORA_EXTR = rdr["TEXT_HORA_EXTR"].ToString().Trim();

                    lstETL.Add(etl);
                }
                con.Close();
                return lstETL;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int AddEtlprog(ETLProg etl)
        {
            string add = "INSERT INTO" + cod +
                "TAB_ETL_PROG" + cod +
                "(" + cod + "INT_ID_EMPRESA" + cod +
                "," + cod + "TEXT_FECH_EXTR" + cod +
                "," + cod + "TEXT_HORA_EXTR" + cod +
                ") VALUES " +
                "(@INT_ID_EMPRESA,@TEXT_FECH_EXTR,@TEXT_HORA_EXTR)";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                cmd.Parameters.AddWithValue("@INT_ID_EMPRESA", etl.INT_ID_EMPRESA);
                cmd.Parameters.AddWithValue("@TEXT_FECH_EXTR", etl.TEXT_FECH_EXTR.Trim());
                cmd.Parameters.AddWithValue("@TEXT_HORA_EXTR", etl.TEXT_HORA_EXTR.Trim());

                int cantFilAfec = cmd.ExecuteNonQuery();
                con.Close();
                return cantFilAfec;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

    }
}
