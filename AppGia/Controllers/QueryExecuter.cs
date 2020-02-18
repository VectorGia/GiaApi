using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;
using NpgsqlTypes;

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
        
        public DataTable ExecuteQuery(String qry)
        {
            string consulta = qry;

            try
            {
                con.Open();
                NpgsqlCommand comP = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            finally
            {
                con.Close();
            }
        }
    }
}