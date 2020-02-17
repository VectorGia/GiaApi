using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.IO;
using AdoNetCore.AseClient;
using System.Data.Odbc;


namespace AppGia.Conexion
{
    public class Conexion
    {
          string cadena = "";
          NpgsqlConnection con;
          AseConnection sysCon;
          OdbcConnection odbcCon;

        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }


        public NpgsqlConnection ConnexionDB()

        {
            var configuration = GetConfiguration();
           // con = new NpgsqlConnection(configuration.GetSection("Data").GetSection("ConnectionString").Value);
            con = new NpgsqlConnection(configuration.GetSection("Data").GetSection("ConnectionStringLocal").Value);
            
            return con;
        }

        public AseConnection ConexionSybase()

        {
            try {
                var configuration = GetConfiguration();

                sysCon = new AseConnection(configuration.GetSection("DataSybase").GetSection("ConnectionString").Value);
                 
                return sysCon;
            }catch (InvalidOperationException ex ){
                string error = ex.Message;
                return null ;
            }
        }

        public OdbcConnection ConexionSybaseodbc(string DsnName)

        {
            try
            {
                var configuration = GetConfiguration();
                odbcCon = new OdbcConnection("DSN="+DsnName);
               // odbcCon = new OdbcConnection("DSN=GIAODBCPRUEBAS"); ////ODBCGIA

                return odbcCon;
            }
            catch (InvalidOperationException ex)
            {
                string error = ex.Message;
                return null;
            }
        }





    }
}

