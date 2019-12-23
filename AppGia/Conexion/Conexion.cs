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

        public NpgsqlConnection ConnexionDB()

        {
            cadena = "User ID = postgres; Password = omnisys; Host = 192.168.1.78; Port = 5432; Database = GIA; Pooling = true";
            con = new NpgsqlConnection(cadena);
            //cnnP.Open();
            return con;
        }

    }
}

