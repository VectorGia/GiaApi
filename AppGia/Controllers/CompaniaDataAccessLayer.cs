﻿using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Threading.Tasks;
=======
>>>>>>> 66851b5e273fa8258515a35d3f2489ba07263c5e
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CompaniaDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
<<<<<<< HEAD

        public IEnumerable<Compania> GetAllCompanias()
        {
            string consulta = "SELECT * FROM" + cod + "CentroCostos" + cod + "";
            try
            {
                List<Compania> lstcompania = new List<Compania>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);

=======
        public IEnumerable<Compania> GetAllCompanias()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_COMPANIA" + cod + "";
            try
            {
                List<Compania> lstCompania = new List<Compania>();
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
>>>>>>> 66851b5e273fa8258515a35d3f2489ba07263c5e
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Compania compania = new Compania();
<<<<<<< HEAD
                        compania.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString();
                        compania.STR_NOMBRE_COMPANIA = rdr["STR_NOMBRE_COMPANIA"].ToString();
                        compania.STR_ABREV_COMPANIA = rdr["STR_ABREV_COMPANIA"].ToString();
                        compania.BOOL_ETL_COMPANIA = Convert.ToBoolean(rdr["BOOL_ETL_COMPANIA"]);
                        compania.STR_HOST_COMPANIA = rdr["STR_HOST_COMPANIA"].ToString();
                        compania.STR_PUERTO_COMPANIA = rdr["STR_PUERTO_COMPANIA"].ToString();
                        compania.STR_USUARIO_ETL = rdr["STR_USUARIO_ETL"].ToString();
                        compania.STR_CONTRASENIA_ETL = rdr["STR_CONTRASENIA_ETL"].ToString();
                        compania.STR_BD_COMPANIA = rdr["STR_BD_COMPANIA"].ToString();
                        compania.STR_MONEDA_COMPANIA = rdr["STR_MONEDA_COMPANIA"].ToString();

                        lstcompania.Add(compania);
                    }
                    con.Close();
                }
                return lstcompania;
=======
                        // id
                        compania.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString();
                        // nombre
                        compania.STR_NOMBRE_COMPANIA = rdr["STR_NOMBRE_COMPANIA"].ToString();
                        //abrev
                        compania.STR_ABREV_COMPANIA = rdr["STR_ABREV_COMPANIA"].ToString();
                        //etl
                        compania.BOOL_ETL_COMPANIA = Convert.ToBoolean(rdr["BOOL_ETL_COMPANIA"]);
                        //host
                        compania.STR_HOST_COMPANIA = rdr["STR_HOST_COMPANIA"].ToString();
                        //moneda
                        compania.STR_MONEDA_COMPANIA = rdr["STR_MONEDA_COMPANIA"].ToString();

                        //public string STR_USUARIO_ETL { get; set; }
                        //public string STR_CONTRASENIA_ETL { get; set; }
                        //public string STR_PUERTO_COMPANIA { get; set; }
                        //public string STR_BD_COMPANIA { get; set; }

                        lstCompania.Add(compania);
                    }
                    con.Close();
                }

                return lstCompania;
            }
            catch
            {
                throw;
            }
        }

        public int addCompania(Compania compania)
        {
            string add = "INSERT INTO" + cod + "TAB_COMPANIA" + cod + "(" + cod + "STR_NOMBRE_COMPANIA" + cod + ") VALUES " +
                "(@STR_NOMBRE_COMPANIA)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    //cmd.Parameters.AddWithValue("@INT_IDGRUPO", grupo.INT_IDGRUPO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_COMPANIA", compania.STR_NOMBRE_COMPANIA);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
>>>>>>> 66851b5e273fa8258515a35d3f2489ba07263c5e
            }
            catch
            {
                throw;
            }
        }
    }
}
