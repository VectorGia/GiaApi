using AppGia.Models;
using AppGia.Util.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace AppGia.Controllers
{
    public class ETLCuentasDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand comP = new NpgsqlCommand();

        OdbcConnection odbcCon;
        OdbcCommand cmdETL = new OdbcCommand();
        char cod = '"';
        DSNConfig dsnConfig = new DSNConfig();

        public ETLCuentasDataAccessLayer (){

            con = conex.ConnexionDB();
        }

        public List<Cuentas> obtenerCuentasSybase(int id_compania)
        {
            DSN dsn = new DSN();
            dsn = dsnConfig.crearDSN(id_compania);

            if (dsn.creado)
            {
                /// obtener conexion de Odbc creado 
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);
            }

                try
            {

                string consulta = " SELECT "
                    + " c.cta cta,"
                    + " c.scta scta,"
                    + " c.sscta sscta,"
                    + " c.descripcion descripcion "

                    + "  FROM cat_cuenta c ";
               

                OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                odbcCon.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();

                List<Cuentas> listaCuentas = new List<Cuentas>();
                while (rdr.Read())
                {
                    Cuentas etlCuentas = new Cuentas();
                    etlCuentas.CHAR_CTA = Convert.ToString(rdr["cta"]);
                    etlCuentas.CHAR_SUB_CTA = Convert.ToString(rdr["scta"]);
                    etlCuentas.CHAR_SUB_SUB_CTA = Convert.ToString(rdr["sscta"]);
                    etlCuentas.TEXT_DESCRIPCION = Convert.ToString(rdr["descripcion"]);
                    etlCuentas.INT_ID_COMPANIA_F = id_compania;
      
                    listaCuentas.Add(etlCuentas);
                }
                return listaCuentas;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
            finally
            {
                odbcCon.Close();
            }
        }


        public int insertarCuentasPg(int id_compania)
        {
            List<Cuentas> listaCuentas = new List<Cuentas>();
            listaCuentas = obtenerCuentasSybase(id_compania);

            //// por borrar 
            ////Cuentas pruebaCuenta = new Cuentas();
            ////pruebaCuenta.CHAR_CTA = "99";
            ////pruebaCuenta.CHAR_SUB_CTA = "88";
            ////pruebaCuenta.CHAR_SUB_SUB_CTA = "77";
            ////pruebaCuenta.INT_ID_COMPANIA_F = id_compania;
            ////pruebaCuenta.TEXT_DESCRIPCION = "delete this ";

            ////listaCuentas.Add(pruebaCuenta);
            //

            con.Open();

            string insercion = "INSERT INTO "
                        + cod + "CAT_CUENTAS" + cod + "("
                        + cod + "CHAR_CTA" + cod + ", "
                        + cod + "CHAR_SUB_CTA" + cod + ", "
                        + cod + "CHAR_SUB_SUB_CTA" + cod + ", "
                        + cod + "TEXT_DESCRIPCION" + cod + ", "
                        + cod + "INT_ID_COMPANIA_F" + cod + ")"

                        + "VALUES "
                        + " (@CHAR_CTA,"
                        + " @CHAR_SUB_CTA,"
                        + " @CHAR_SUB_SUB_CTA,"
                        + " @TEXT_DESCRIPCION,"
                        + " @INT_ID_COMPANIA_F)";
            try
            {
                {
                    int cantFilaAfect = 0;
                    foreach (Cuentas cuenta in listaCuentas)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(insercion, con);
                        //cmd.Parameters.AddWithValue("@INT_IDBALANZA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_IDBALANZA);
                        cmd.Parameters.AddWithValue("@CHAR_CTA", NpgsqlTypes.NpgsqlDbType.Text, cuenta.CHAR_CTA);
                        cmd.Parameters.AddWithValue("@CHAR_SUB_CTA", NpgsqlTypes.NpgsqlDbType.Text, cuenta.CHAR_SUB_CTA);
                        cmd.Parameters.AddWithValue("@CHAR_SUB_SUB_CTA", NpgsqlTypes.NpgsqlDbType.Text, cuenta.CHAR_SUB_SUB_CTA);
                        cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION", NpgsqlTypes.NpgsqlDbType.Text, cuenta.TEXT_DESCRIPCION);
                        cmd.Parameters.AddWithValue("@INT_ID_COMPANIA_F", NpgsqlTypes.NpgsqlDbType.Integer, cuenta.INT_ID_COMPANIA_F);
                        //conP.Open();
                        // int cantFilaAfect = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cantFilaAfect = cantFilaAfect + Convert.ToInt32(cmd.ExecuteNonQuery());
                    }

                    con.Close();
                   // configCorreo.EnviarCorreo("La extracción Semanal se genero correctamente", "ETL Reporte Semanal");
                    return cantFilaAfect;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                //configCorreo.EnviarCorreo("La extracción Semanal se genero incorrectamente", "ETL Reporte Semanal");
                string error = ex.Message;
                throw;
            }




        }
    }
}
