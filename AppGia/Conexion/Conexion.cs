using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.IO;
using AdoNetCore.AseClient;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace AppGia.Conexion
{
    public class Conexion
    {
        string cadena = "";
        NpgsqlConnection con;
        AseConnection sysCon;
        OdbcConnection odbcCon;
        SqlConnection sqlCon;
        private static IConfigurationRoot _configurationRoot = GetConfiguration();

        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public NpgsqlConnection ConnexionDB()

        {
            // con = new NpgsqlConnection(configuration.GetSection("Data").GetSection("ConnectionString").Value);
            con = new NpgsqlConnection(_configurationRoot.GetSection("Data").GetSection("ConnectionStringLocal").Value);

            return con;
        }

        public AseConnection ConexionSybase(string conectionParams)

        {
            try
            {
                sysCon = new AseConnection(conectionParams);

                return sysCon;
            }
            catch (InvalidOperationException ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public OdbcConnection ConexionSybaseodbc(string DsnName)

        {
            try
            {
                var configuration = GetConfiguration();
                odbcCon = new OdbcConnection("DSN=" + DsnName);
                // odbcCon = new OdbcConnection("DSN=GIAODBCPRUEBAS"); ////ODBCGIA

                return odbcCon;
            }
            catch (InvalidOperationException ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public SqlConnection ConexionSQL()
        {
            sqlCon = new SqlConnection(_configurationRoot.GetSection("Data").GetSection("ConnectionStringSQL").Value);
            return sqlCon;
        }
    }
}