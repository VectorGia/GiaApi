using System.Collections.Generic;
using AppGia.Models;
using Npgsql;
using System;


namespace AppGia.Controllers
{
    public class PantallaDataAccessLayer
    {
        //private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public PantallaDataAccessLayer() {
            con = conex.ConnexionDB();
        }

        char cod = '"';
        public IEnumerable<Pantalla> GetAllPantallas()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PANTALLA" + cod + "";
            try
            {
                List<Pantalla> lstpantalla = new List<Pantalla>();

                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Pantalla pantalla = new Pantalla();
                        pantalla.STR_NOMBRE_PANTALLA = rdr["STR_NOMBRE_PANTALLA"].ToString().Trim();

                        lstpantalla.Add(pantalla);
                    }
                conex.ConnexionDB().Close();
                //}

                return lstpantalla;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int addPantalla(Pantalla pantalla)
        {
            string add = "INSERT INTO" + cod + "TAB_PANTALLA" + cod + "(" + cod + "STR_NOMBRE_PANTALLA" + cod + ") VALUES " +
                "(@STR_NOMBRE_PANTALLA)";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PANTALLA", pantalla.STR_NOMBRE_PANTALLA.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF_PANTALLA", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PANT", pantalla.BOOL_ESTATUS_LOGICO_PANT);

                    conex.ConnexionDB().Open();
                    int cantFila = cmd.ExecuteNonQuery();
                    conex.ConnexionDB().Close() ;
                    return cantFila;
                //}
                //return 1;
            }
            catch
            {
                conex.ConnexionDB().Close();
                throw;
            }
        }

        public int UpdatePantalla(Pantalla pantalla)
        {

            string add = "UPDATE " + cod + "TAB_PANTALLA" + cod +
            " SET " + cod + "INT_IDROL_F" + cod + "= " + "@INT_IDROL_F" + ","
            + cod + "FEC_MODIF_PANTALLA" + cod + "= " + "@FEC_MODIF_PANTALLA" + ","
            + cod + "STR_NOMBRE_PANTALLA" + cod + "= " + "@STR_NOMBRE_PANTALLA"
            + " WHERE " + cod + "INT_IDPANTALLA_P" + cod + " = " + "@INT_IDPANTALLA_P";

            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = pantalla.INT_IDPANTALLA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_F", Value = pantalla.INT_IDROL_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.TimestampTz, ParameterName = "@FEC_MODIF_PANTALLA", Value = pantalla.FEC_MODIF_PANTALLA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_PANTALLA", Value = pantalla.STR_NOMBRE_PANTALLA.Trim() });

                    conex.ConnexionDB().Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    conex.ConnexionDB().Close();
                    return cantFilas;
                //}
            }
            catch 
            {
                conex.ConnexionDB().Close();
                throw;
            }
        }



        //public int DeletePantalla(int id)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


        //            cmd.Parameters.AddWithValue("STR_NOMBRE_PANTALLA", id);

        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return 1;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public int DeletePantalla(Pantalla pantalla)
        {
            string add = "UPDATE " + cod + "TAB_PANTALLA" + cod
                + " SET " + cod + "BOOL_ESTATUS_LOGICO_PANT" + cod + "= " + "@BOOL_ESTATUS_LOGICO_PANT"
                + " WHERE " + cod + "INT_IDPANTALLA_P" + cod + " = " + "@INT_IDPANTALLA_P";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                //{
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conex.ConnexionDB());
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = pantalla.INT_IDPANTALLA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_PANT", Value = pantalla.BOOL_ESTATUS_LOGICO_PANT });
                    conex.ConnexionDB().Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    conex.ConnexionDB().Close();
                    return cantFilas;
                //}
            }
            catch 
            {
                conex.ConnexionDB().Close();
                throw;
            }
        }

    }
}
