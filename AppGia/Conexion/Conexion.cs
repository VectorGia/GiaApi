using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace AppGia.Conexion
{
    public class Conexion
    {
        //NpgsqlConnection conP;
            string cadena = "";
            NpgsqlConnection cnnP;

        //public Conexion()
        //{
        //    //Constructor
        //    var configuration = GetConfiguration();
        //    conP = new NpgsqlConnection(configuration.GetSection("Data").GetSection("ConnectionString").Value);
        //}

        //public IConfigurationRoot GetConfiguration()
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        //    return builder.Build();
        //}

        public NpgsqlConnection ConnexionDB()
        {
            cadena = "User ID = postgres; Password = omnisys; Host = 192.168.1.78; Port = 5432; Database = GIA; Pooling = true";
            cnnP = new NpgsqlConnection(cadena);
            //cnnP.Open();
            return cnnP;
        }

    }
}

