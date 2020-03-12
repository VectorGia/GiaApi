
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Dao
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
            string cadena = "SELECT * FROM " + cod + "TAB_ETL_PROG";
            try
            {
                List<ETLProg> lstETL = new List<ETLProg>();
                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ETLProg etl = new ETLProg();
                    etl.id = Convert.ToInt32(rdr["INT_ID_ETL_PROG"]);         
                    etl.fecha_extraccion = rdr["TEXT_FECH_EXTR"].ToString().Trim();
                    etl.hora_extraccion = rdr["TEXT_HORA_EXTR"].ToString().Trim();

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
            finally
            {
                con.Close();
            }
        }

        public int AddEtlprog(ETLProg etl)
        {
                 string add = "INSERT INTO" + cod +
                "TAB_ETL_PROG" + cod +
                "(" + cod + "TEXT_FECH_EXTR" + cod +
                "," + cod + "TEXT_HORA_EXTR" + cod +
                ") VALUES " +
                "(@TEXT_FECH_EXTR,@TEXT_HORA_EXTR)";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                string fechaDia = Convert.ToDateTime(etl.fecha_extraccion).Date.ToShortDateString();
                string horaDia = Convert.ToDateTime(etl.hora_extraccion).ToLongTimeString();
                cmd.Parameters.AddWithValue("@TEXT_FECH_EXTR", fechaDia);
                cmd.Parameters.AddWithValue("@TEXT_HORA_EXTR", horaDia.Trim());
                int cantFilAfec = cmd.ExecuteNonQuery();
                con.Close();
                return cantFilAfec;
            }
            catch
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int UpdateEtlprog(ETLProg etl)
        {
            string update = "UPDATE " + cod + "TAB_ETL_PROG" + cod + " SET "
                + cod + "TEXT_FECH_EXTR" + cod + "= " + "@TEXT_HORA_EXTR"
                + " WHERE " + cod + "ID_ETL_PROG" + cod + " = " + "@ID_ETL_PROG";
            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_GRUPO", Value = etl.fecha_extraccion   });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_P", Value = etl.hora_extraccion });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}