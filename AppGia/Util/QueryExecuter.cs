using System;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace AppGia.Controllers
{
    public class QueryExecuter
    {
        private NpgsqlConnection con;
        private Conexion.Conexion conex = new Conexion.Conexion();

        public QueryExecuter()
        {
            con = conex.ConnexionDB();
        }

        public int execute(String query, params object[] parametros)
        {
            DbTransaction transaction = null;
            try
            {
                con.Open();
                transaction = con.BeginTransaction();
                NpgsqlCommand cmd = new NpgsqlCommand(query, con);
                foreach (var parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }

                int cantFilAfec = cmd.ExecuteNonQuery();
                transaction.Commit();
                return cantFilAfec;
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                throw;
            }
            finally
            {
                closeConection(con);
            }
        }
        
        public static void closeConection( NpgsqlConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }   
        }

        public DataTable ExecuteQuery(String qry, params object[] parametros)
        {
            string consulta = qry;

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                foreach (var parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }

                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            finally
            {
                closeConection(con);
            }
        }

        public DataRow ExecuteQueryUniqueresult(String qry, params object[] parametros)
        {
            DataTable dataTable = ExecuteQuery(qry, parametros);
            if (dataTable.Rows.Count == 1)
            {
                return dataTable.Rows[0];
            }

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            throw new DataException("Se esperaba un resultado pero se obtuvieron " + dataTable.Rows.Count);
        }
    }
}