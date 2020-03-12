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

        public DataTable ExecuteQuerySQL(String qry)
        {
            string consulta = qry;

            try
            {
                con.Open();
                SqlCommand comP = new SqlCommand(consulta, con);
                SqlDataAdapter daP = new SqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            finally
            {
                con.Close();
            }
        }

        public DataRow ExecuteQueryUniqueresultSQL(String qry)
        {
            DataTable dataTable = ExecuteQuerySQL(qry);
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
