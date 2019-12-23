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
          string cadena = "";
          NpgsqlConnection con;


        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }


        public NpgsqlConnection ConnexionDB()

        {
            var configuration = GetConfiguration();
            con = new NpgsqlConnection(configuration.GetSection("Data").GetSection("ConnectionString").Value);
            return con;
        }

    }
}

