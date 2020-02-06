using System.Collections.Generic;
using AppGia.Models;
using Npgsql;
using System;


namespace AppGia.Controllers
{
    public class PantallaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public PantallaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        char cod = '"';
        public IEnumerable<Pantalla> GetAllPantallas()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PANTALLA" + cod + "";
            try
            {
                List<Pantalla> lstpantalla = new List<Pantalla>();

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Pantalla pantalla = new Pantalla();
                        pantalla.STR_NOMBRE_PANTALLA = rdr["STR_NOMBRE_PANTALLA"].ToString().Trim();
                    pantalla.INT_IDPANTALLA_P = Convert.ToInt32(rdr["INT_IDPANTALLA_P"]);

                        lstpantalla.Add(pantalla);
                    }
                con.Close();
     
                return lstpantalla;
            }
            catch
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int addPantalla(Pantalla pantalla)
        {
            string add = "INSERT INTO" + cod + "TAB_PANTALLA" + cod + "(" + cod + "STR_NOMBRE_PANTALLA" + cod  + "," + cod + "FEC_MODIF_PANTALLA" + "," + cod + "BOOL_ESTATUS_LOGICO_PANT" + cod + ") VALUES " +
                "(@STR_NOMBRE_PANTALLA,@FEC_MODIF_PANTALLA,@BOOL_ESTATUS_LOGICO_PANT)";

           
            try
            {

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PANTALLA", pantalla.STR_NOMBRE_PANTALLA.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF_PANTALLA", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_PANT", pantalla.BOOL_ESTATUS_LOGICO_PANT);

                    con.Open();
                    int cantFila = cmd.ExecuteNonQuery();
                    con.Close() ;
                    return cantFila;

            }
            catch
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int UpdatePantalla(Pantalla pantalla)
        {

            string add = "UPDATE " + cod + "TAB_PANTALLA" + cod +
            " SET " + cod + "FEC_MODIF_PANTALLA" + cod + "= " + "@FEC_MODIF_PANTALLA" + ","
            + cod + "STR_NOMBRE_PANTALLA" + cod + "= " + "@STR_NOMBRE_PANTALLA"
            + " WHERE " + cod + "INT_IDPANTALLA_P" + cod + " = " + "@INT_IDPANTALLA_P";

            try
            {

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = pantalla.INT_IDPANTALLA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.TimestampTz, ParameterName = "@FEC_MODIF_PANTALLA", Value = pantalla.FEC_MODIF_PANTALLA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_PANTALLA", Value = pantalla.STR_NOMBRE_PANTALLA.Trim() });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;

            }
            catch 
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }


        public int DeletePantalla(Pantalla pantalla)
        {
            string add = "UPDATE " + cod + "TAB_PANTALLA" + cod
                + " SET " + cod + "BOOL_ESTATUS_LOGICO_PANT" + cod + "= " + "@BOOL_ESTATUS_LOGICO_PANT"
                + " WHERE " + cod + "INT_IDPANTALLA_P" + cod + " = " + "@INT_IDPANTALLA_P";
            try
            {
                
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = pantalla.INT_IDPANTALLA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_PANT", Value = pantalla.BOOL_ESTATUS_LOGICO_PANT });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
            }
            catch 
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
