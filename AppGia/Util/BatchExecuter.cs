using System;
using System.Collections.Generic;
using System.Data.Common;
using Npgsql;
using static AppGia.Util.QueryExecuter;

namespace AppGia.Util
{
    public class BatchExecuter
    {
        private NpgsqlConnection con;
        private Conexion.Conexion conex = new Conexion.Conexion();
        private List<NpgsqlCommand> _commands = new List<NpgsqlCommand>();
        private string query;

        public BatchExecuter(String query)
        {
            con = conex.ConnexionDB();
            this.query = query;
        }

        public int executeCommands()
        {
            DbTransaction transaction = null;
            try
            {
                int cantFilAfec = 0;
                con.Open();
                transaction = con.BeginTransaction();
                foreach (var cmd in _commands)
                {
                    cantFilAfec += cmd.ExecuteNonQuery();
                }

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

        public void addCommand(params object[] parametros)
        {
            addCommand(query, parametros);
        }
        
        public void addCommand(String query,params object[] parametros)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(query, con);
            foreach (var parametro in parametros)
            {
                cmd.Parameters.Add(parametro);
            }

            _commands.Add(cmd);
        }
        
    }
}