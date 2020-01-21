using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class CuentasDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public CuentasDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        char cod = '"';

        public IEnumerable<Cuentas> GetAllCuentas()
        {
            //Obtiene todas las Cuentas
            string consulta = "SELECT * FROM" + cod + "CAT_CUENTAS" + cod;
            try
            {
                List<Cuentas> lstCuentas = new List<Cuentas>();
                {

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);


                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Cuentas cuentas = new Cuentas();

                        cuentas.INT_ID_CUENTAS = Convert.ToInt32(rdr["INT_ID_CUENTAS"]);
                        cuentas.CHAR_CTA = rdr["CHAR_CTA"].ToString().Trim();
                        cuentas.CHAR_SUB_CTA = rdr["CHAR_SUB_CTA"].ToString().Trim();
                        cuentas.CHAR_SUB_SUB_CTA = rdr["CHAR_SUB_SUB_CTA"].ToString().Trim();
                        cuentas.TEXT_DESCRIPCION = rdr["TEXT_DESCRIPCION"].ToString().Trim();
                        cuentas.INT_ID_COMPANIA_F = Convert.ToInt32(rdr["INT_ID_COMPANIA_F"]);

                        lstCuentas.Add(cuentas);
                    }
                    con.Close();
                }
                return lstCuentas;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
        }

        //Obtiene las cuentas de cada compañia 
        public List<Cuentas> GetCuentasData(string intIdCompania)
        {
            string consulta = "SELECT * FROM" + cod + "CAT_CUENTAS" + cod + "WHERE" + cod + "INT_ID_COMPANIA_F" + cod + "=" + intIdCompania;
            try
            {
                List<Cuentas> listCuentas = new List<Cuentas>();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        Cuentas cuentas = new Cuentas();
                        cuentas.INT_ID_CUENTAS = Convert.ToInt32(rdr["INT_ID_CUENTAS"]);
                        cuentas.CHAR_CTA = rdr["CHAR_CTA"].ToString().Trim();
                        cuentas.CHAR_SUB_CTA = rdr["CHAR_SUB_CTA"].ToString().Trim();
                        cuentas.CHAR_SUB_SUB_CTA = rdr["CHAR_SUB_SUB_CTA"].ToString().Trim();
                        cuentas.TEXT_DESCRIPCION = rdr["TEXT_DESCRIPCION"].ToString().Trim();
                        cuentas.INT_ID_COMPANIA_F = Convert.ToInt32(rdr["INT_ID_COMPANIA_F"]);
                        listCuentas.Add(cuentas);

                    }

                    con.Close();
                }
                return listCuentas;
            }
            catch
            {
                con.Close();
                throw;

            }
        }
        public int AddCuenta (Cuentas cuentas)
        {
            string add = "INSERT INTO "
                + cod + "CAT_CUENTAS" + cod + "("
                + cod + "CHAR_CTA" + cod + ","
                + cod + "CHAR_SUB_CTA" + cod + ","
                + cod + "CHAR_SUB_SUB_CTA" + cod + ","
                + cod + "TEXT_DESCRIPCION" + cod + ","
                + cod + "INT_ID_COMPANIA_F"
                + cod + ")" +

                "VALUES (" +
                "@CHAR_CTA," +
                "@CHAR_SUB_CTA," +
                "@CHAR_SUB_SUB_CTA," +
                "@TEXT_DESCRIPCION," +
                "@INT_ID_COMPANIA_F)";

            try
            {

                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@CHAR_CTA", cuentas.CHAR_CTA.Trim());
                    cmd.Parameters.AddWithValue("@CHAR_SUB_CTA", cuentas.CHAR_SUB_CTA.Trim());
                    cmd.Parameters.AddWithValue("@CHAR_SUB_SUB_CTA", cuentas.CHAR_SUB_SUB_CTA.Trim());
                    cmd.Parameters.AddWithValue("@TEXT_DESCRIPCION", cuentas.TEXT_DESCRIPCION.Trim());
                    cmd.Parameters.AddWithValue("@INT_ID_COMPANIA_F", cuentas.INT_ID_COMPANIA_F);

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


    }
}
