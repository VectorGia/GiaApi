using AppGia.Models;
using AppGia.Util.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace AppGia.Dao
{
    public class ETLCuentasDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand comP = new NpgsqlCommand();

        OdbcConnection odbcCon;
        OdbcCommand cmdETL = new OdbcCommand();
        char cod = '"';
        //DSNConfig dsnConfig = new DSNConfig();

        public ETLCuentasDataAccessLayer()
        {

            con = conex.ConnexionDB();
        }

        public List<Cuentas> obtenerCuentasSybase(int id_compania)
        {
            DSN dsn = new DSN();
            //dsn = dsnConfig.crearDSN(id_compania);

            if (dsn.creado)
            {
                /// obtener conexion de Odbc creado 
                odbcCon = conex.ConexionSybaseodbc(dsn.nombreDSN);
            }

            try
            {

                string consulta = " SELECT id, activo, cta, descripcion, id_companiaf, sub_cta, sub_sub_cta"
                    + "  FROM cuenta c ";

                OdbcCommand cmd = new OdbcCommand(consulta, odbcCon);
                odbcCon.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();

                List<Cuentas> listaCuentas = new List<Cuentas>();
                while (rdr.Read())
                {
                    Cuentas etlCuentas = new Cuentas();
                    etlCuentas.cta = Convert.ToString(rdr["cta"]);
                    etlCuentas.sub_cta = Convert.ToString(rdr["scta"]);
                    etlCuentas.sub_sub_cta = Convert.ToString(rdr["sscta"]);
                    etlCuentas.descripcion = Convert.ToString(rdr["descripcion"]);
                    etlCuentas.id_companiaf = id_compania;

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

            string insercion = "INSERT INTO cuenta ("
                        + " id,"
                        + " activo,"
                        + " cta,"
                        + " descripcion,"
                        + " id_companiaf,"
                        + " sub_cta,"
                        + " sub_sub_cta)"

                        + "VALUES "
                        + " (@nextval(seq_cuenta),"
                        + " @activo,"
                        + " @cta,"
                        + " @descripcion,"
                        + " @id_companiaf,"
                        + " @sub_cta,"
                        + " @sub_sub_cta)";
            try
            {
                {
                    int cantFilaAfect = 0;
                    foreach (Cuentas cuenta in listaCuentas)
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(insercion, con);
                        //cmd.Parameters.AddWithValue("@INT_IDBALANZA", NpgsqlTypes.NpgsqlDbType.Integer, balanza.INT_IDBALANZA);
                        cmd.Parameters.AddWithValue("@activo", NpgsqlTypes.NpgsqlDbType.Text, cuenta.activo);
                        cmd.Parameters.AddWithValue("@cta", NpgsqlTypes.NpgsqlDbType.Text, cuenta.cta);
                        cmd.Parameters.AddWithValue("@descripcion", NpgsqlTypes.NpgsqlDbType.Text, cuenta.descripcion);
                        cmd.Parameters.AddWithValue("@id_companiaf", NpgsqlTypes.NpgsqlDbType.Text, cuenta.id_companiaf);
                        cmd.Parameters.AddWithValue("@sub_cta", NpgsqlTypes.NpgsqlDbType.Integer, cuenta.sub_cta);
                        cmd.Parameters.AddWithValue("@sub_sub_cta", NpgsqlTypes.NpgsqlDbType.Integer, cuenta.sub_sub_cta);
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
            finally
            {
                con.Close();
            }
        }
    }
}
