using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Util
{
    public class QueryExecuterSQL
    {
        private SqlConnection con;
        private Conexion.Conexion conex = new Conexion.Conexion();

        public QueryExecuterSQL()
        {
            //Constructor
            con = conex.ConexionSQL();
        }

        public DataTable ExecuteQuerySQL(String qry, params object[] parametros)
        {
            string consulta = qry;

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(consulta, con);
                foreach (var parametro in parametros)
                {
                    cmd.Parameters.Add(parametro);
                }

                SqlDataAdapter daP = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            finally
            {
                closeConection(con);
            }
        }
        
        public DataRow ExecuteQueryUniqueresultSQL(String qry, params object[] parametros)
        {
            DataTable dataTable = ExecuteQuerySQL(qry, parametros);
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
        
        public static void closeConection(SqlConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}