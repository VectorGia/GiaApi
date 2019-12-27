﻿using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CompaniaDataAccessLayer
    {
        //private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public CompaniaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Compania> GetAllCompanias()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_COMPANIA" + cod + "WHERE "+cod+ "BOOL_ESTATUS_LOGICO_COMPANIA" +cod+ "=" + true;
            try
            {
                List<Compania> lstcompania = new List<Compania>();
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena.Trim(), con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        
                        Compania compania = new Compania();
                        compania.INT_IDCOMPANIA_P = Convert.ToInt32(rdr["INT_IDCOMPANIA_P"]);
                        compania.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString().Trim();
                        compania.STR_NOMBRE_COMPANIA = rdr["STR_NOMBRE_COMPANIA"].ToString().Trim();
                        compania.STR_ABREV_COMPANIA = rdr["STR_ABREV_COMPANIA"].ToString().Trim();
                        compania.BOOL_ETL_COMPANIA = Convert.ToBoolean(rdr["BOOL_ETL_COMPANIA"]);
                        compania.STR_HOST_COMPANIA = rdr["STR_HOST_COMPANIA"].ToString().Trim();
                        compania.STR_PUERTO_COMPANIA = rdr["STR_PUERTO_COMPANIA"].ToString().Trim();
                        compania.STR_USUARIO_ETL = rdr["STR_USUARIO_ETL"].ToString().Trim();
                        compania.STR_CONTRASENIA_ETL = rdr["STR_CONTRASENIA_ETL"].ToString().Trim();
                        compania.STR_BD_COMPANIA = rdr["STR_BD_COMPANIA"].ToString().Trim();
                        compania.STR_MONEDA_COMPANIA = rdr["STR_MONEDA_COMPANIA"].ToString().Trim();

                        lstcompania.Add(compania);
                    }
                    con.Close();
                }
                return lstcompania;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public Compania GetCompaniaData(string id)
        {
            try
            {
                Compania compania = new Compania();
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    string consulta = "SELECT * FROM" + cod + "CAT_COMPANIA" + cod + "WHERE" +cod+ "INT_IDCOMPANIA_P"+cod+ "=" +id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        compania.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString().Trim();
                        compania.STR_NOMBRE_COMPANIA = rdr["STR_NOMBRE_COMPANIA"].ToString().Trim();
                        compania.STR_ABREV_COMPANIA = rdr["STR_ABREV_COMPANIA"].ToString().Trim();
                        compania.BOOL_ETL_COMPANIA = Convert.ToBoolean(rdr["BOOL_ETL_COMPANIA"]);
                        compania.STR_HOST_COMPANIA = rdr["STR_HOST_COMPANIA"].ToString().Trim();
                        compania.STR_PUERTO_COMPANIA = rdr["STR_PUERTO_COMPANIA"].ToString().Trim();
                        compania.STR_USUARIO_ETL = rdr["STR_USUARIO_ETL"].ToString().Trim();
                        compania.STR_CONTRASENIA_ETL = rdr["STR_CONTRASENIA_ETL"].ToString().Trim();
                        compania.STR_BD_COMPANIA = rdr["STR_BD_COMPANIA"].ToString().Trim();
                        compania.STR_MONEDA_COMPANIA = rdr["STR_MONEDA_COMPANIA"].ToString().Trim();

                    }
                    con.Close();
                }
                return compania;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int AddCompania(Compania compania)
        {
            string add = "INSERT INTO" + cod +
                "CAT_COMPANIA" + cod +
                "("+cod+"STR_NOMBRE_COMPANIA"+cod+
                ","+cod+"STR_ABREV_COMPANIA"+cod+
                ","+cod+"BOOL_ETL_COMPANIA"+cod+
                ","+cod+"STR_HOST_COMPANIA"+cod+
                ","+cod+"STR_MONEDA_COMPANIA"+cod+
                ","+cod+"STR_IDCOMPANIA"+cod+
                ","+cod+"STR_USUARIO_ETL"+cod+
                ","+cod+"STR_CONTRASENIA_ETL"+cod+
                ","+cod+"STR_PUERTO_COMPANIA"+cod+
                ","+cod+"STR_BD_COMPANIA"+cod+
                ","+cod+"INT_IDCENTROCOSTO_F"+cod+
                ","+cod+"INT_IDPROYECTO_F"+cod+
                ","+cod+ "FEC_MODIF_COMPANIA"+cod+
                "," +cod+"BOOL_ESTATUS_LOGICO_COMPANIA"+cod+
                ") VALUES " +
                "(@STR_NOMBRE_COMPANIA," +
                "@STR_ABREV_COMPANIA," +
                "@BOOL_ETL_COMPANIA," +
                "@STR_HOST_COMPANIA," +
                "@STR_MONEDA_COMPANIA," +
                "@STR_IDCOMPANIA," +
                "@STR_USUARIO_ETL," +
                "@STR_CONTRASENIA_ETL," +
                "@STR_PUERTO_COMPANIA," +
                "@STR_BD_COMPANIA," +
                "@INT_IDCENTROCOSTO_F," +
                "@INT_IDPROYECTO_F," +
                "@FEC_MODIF_COMPANIA,"+
                "@BOOL_ESTATUS_LOGICO_COMPANIA)";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add.Trim(), con);
                    //cmd.Parameters.AddWithValue("@INT_IDGRUPO", grupo.INT_IDGRUPO);
                    cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", compania.STR_IDCOMPANIA.Trim());
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_COMPANIA", compania.STR_NOMBRE_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_ABREV_COMPANIA", compania.STR_ABREV_COMPANIA);
                    cmd.Parameters.AddWithValue("@BOOL_ETL_COMPANIA", compania.BOOL_ETL_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_HOST_COMPANIA", compania.STR_HOST_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_MONEDA_COMPANIA", compania.STR_MONEDA_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_USUARIO_ETL", compania.STR_USUARIO_ETL);
                    cmd.Parameters.AddWithValue("@STR_CONTRASENIA_ETL", compania.STR_CONTRASENIA_ETL);
                    cmd.Parameters.AddWithValue("@STR_PUERTO_COMPANIA", compania.STR_PUERTO_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_BD_COMPANIA", compania.STR_BD_COMPANIA);
                    cmd.Parameters.AddWithValue("@INT_IDCENTROCOSTO_F", compania.INT_IDCENTROCOSTO_F);
                    cmd.Parameters.AddWithValue("@INT_IDPROYECTO_F", compania.INT_IDPROYECTO_F);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_COMPANIA", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", compania.BOOL_ESTATUS_LOGICO_COMPANIA);

                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
                

            }
            catch
            {
                con.Close();
                throw;
            }
        }


        public int Update(string id, Compania compania)
            
        {
            //compania.INT_IDCOMPANIA_P = Convert.ToInt32(id);
            string update = "UPDATE " + cod + "CAT_COMPANIA" 
                + cod + "SET" + cod + "STR_IDCOMPANIA" + cod + "=" + "@STR_IDCOMPANIA" +","
                + cod + "STR_NOMBRE_COMPANIA"+ cod + "= " + "@STR_NOMBRE_COMPANIA" + ","
                + cod + "STR_ABREV_COMPANIA" + cod + "= " + "@STR_ABREV_COMPANIA"  + ","
                + cod + "BOOL_ETL_COMPANIA"  + cod + "= " + "@BOOL_ETL_COMPANIA"   + ","
                + cod + "STR_HOST_COMPANIA"  + cod + "= " + "@STR_HOST_COMPANIA"   + ","
                + cod + "STR_MONEDA_COMPANIA"+ cod + "= " + "@STR_MONEDA_COMPANIA" + ","
                + cod + "STR_USUARIO_ETL"    + cod + "= " + "@STR_USUARIO_ETL"     + ","
                + cod + "STR_CONTRASENIA_ETL"+ cod + "= " + "@STR_CONTRASENIA_ETL" + ","
                + cod + "STR_PUERTO_COMPANIA"+ cod + "= " + "@STR_PUERTO_COMPANIA" + ","
                + cod + "STR_BD_COMPANIA"    + cod + "= " + "@STR_BD_COMPANIA"
                + " WHERE "+cod+"INT_IDCOMPANIA_P"+cod+ "=" + id;

            try
            {
                //using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update.Trim(), con);
                    cmd.Parameters.AddWithValue("STR_IDCOMPANIA", compania.STR_IDCOMPANIA);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_COMPANIA", compania.STR_NOMBRE_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_ABREV_COMPANIA", compania.STR_ABREV_COMPANIA);
                    cmd.Parameters.AddWithValue("@BOOL_ETL_COMPANIA", compania.BOOL_ETL_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_HOST_COMPANIA", compania.STR_HOST_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_MONEDA_COMPANIA", compania.STR_MONEDA_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_USUARIO_ETL", compania.STR_USUARIO_ETL);
                    cmd.Parameters.AddWithValue("@STR_CONTRASENIA_ETL", compania.STR_CONTRASENIA_ETL);
                    cmd.Parameters.AddWithValue("@STR_PUERTO_COMPANIA", compania.STR_PUERTO_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_BD_COMPANIA", compania.STR_BD_COMPANIA);
                    cmd.Parameters.AddWithValue("@INT_IDCENTROCOSTO_F", compania.INT_IDCENTROCOSTO_F);
                    cmd.Parameters.AddWithValue("@INT_IDPROYECTO_F", compania.INT_IDPROYECTO_F);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_COMPANIA", DateTime.Now);

                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }

            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public int Delete(string id)
         {
            string status = "false";
            string delete = "UPDATE " + cod + "CAT_COMPANIA" + cod + "SET" 
                + cod + "BOOL_ESTATUS_LOGICO_COMPANIA" + cod + "='" + status + "' " +
                "WHERE" + cod + "INT_IDCOMPANIA_P" + cod + "='" + id + "'";
           
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete.Trim(), con);


                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }

            }
            catch
            {
                con.Close();
                throw;
            }

            //compania.BOOL_ESTATUS_LOGICO_COMPANIA = true;
            //compania.INT_IDCOMPANIA_P=1;


            //string delete = "UPDATE " + cod + "CAT_COMPANIA" + cod + "SET" + cod + "BOOL_ESTATUS_LOGICO_COMPANIA" + cod + "='" + compania.BOOL_ESTATUS_LOGICO_COMPANIA + "' WHERE"+cod+ "INT_IDCOMPANIA_P" + cod+"='"+ compania.INT_IDCOMPANIA_P + "'";
            //try
            //{
            //    using(NpgsqlConnection con = new NpgsqlConnection(connectionString))
            //    {
            //        NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
            //        cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", compania.BOOL_ESTATUS_LOGICO_COMPANIA);

            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    return 1;
            //}
            //catch
            //{
            //    throw;
            //}
        }
    }
}
