using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class TipoCambioDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public TipoCambioDataAccessLayer()
        {

            con = conex.ConnexionDB();
        }

        public IEnumerable<TipoCambio> GetAllTipoCambio()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_TIPOCAMBIO" + cod + "";
            try
            {
                List<TipoCambio> lstTipoCambio = new List<TipoCambio>();


                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        TipoCambio tipoCambio = new TipoCambio();
                        tipoCambio.INT_ID_TIPOCAMBIO_P = Convert.ToInt32(rdr["INT_ID_TIPOCAMBIO_P"]);
                        tipoCambio.DBL_TIPOCAMBIO_OFICIAL = Convert.ToDouble( rdr["DBL_TIPOCAMBIO_OFICIAL"]);
                        tipoCambio.INT_ID_MONEDA_F = Convert.ToInt32(rdr["INT_ID_MONEDA_F"]);
                        tipoCambio.FEC_MODIF_TIPOCAMBIO = DateTime.Now;
                        tipoCambio.DAT__TIPOCAMBIO = DateTime.Now;
                        tipoCambio.BOOL_ESTATUS_TIPOCAMBIO = Convert.ToBoolean(rdr["BOOL_ESTATUS_TIPOCAMBIO"]);


                        lstTipoCambio.Add(tipoCambio);
                    }
                    con.Close();
                }

                return
                        lstTipoCambio;
            }
            catch
            {
                con.Close();
                throw;
            }
        }


        public int insert(TipoCambio  tipoCambio)
        {
            string add = "INSERT INTO " + cod + "CAT_TIPOCAMBIO" + cod
                        + "("
                        + cod + "DBL_TIPOCAMBIO_OFICIAL" + cod + ","
                        + cod + "INT_ID_MONEDA_F" + cod + ","
                        + cod + "FEC_MODIF_TIPOCAMBIO" + cod + ","
                        + cod + "DAT__TIPOCAMBIO" + cod + ","
                        + cod + "BOOL_ESTATUS_TIPOCAMBIO" + cod + ")"
                        + " VALUES ( @DBL_TIPOCAMBIO_OFICIAL" + ","
                        + "@INT_ID_MONEDA_F" + ","
                        + "@FEC_MODIF_TIPOCAMBIO" + ","
                        + "@DAT__TIPOCAMBIO" + ","
                        + "@BOOL_ESTATUS_TIPOCAMBIO" 
                        + ")";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                 
                    cmd.Parameters.AddWithValue("@DBL_TIPOCAMBIO_OFICIAL", tipoCambio.DBL_TIPOCAMBIO_OFICIAL);
                    cmd.Parameters.AddWithValue("@INT_ID_MONEDA_F", tipoCambio.INT_ID_MONEDA_F);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_TIPOCAMBIO", tipoCambio.FEC_MODIF_TIPOCAMBIO);
                    cmd.Parameters.AddWithValue("@DAT__TIPOCAMBIO", tipoCambio.DAT__TIPOCAMBIO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_TIPOCAMBIO", tipoCambio.BOOL_ESTATUS_TIPOCAMBIO);

                    con.Open();
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

        public int update(TipoCambio tipoCambio)
        {

            string update = "UPDATE " + cod + "CAT_TIPOCAMBIO" + cod + "SET"

          + cod + "DBL_TIPOCAMBIO_OFICIAL" + cod + " = '" + tipoCambio.DBL_TIPOCAMBIO_OFICIAL + "' ,"
          + cod + "INT_ID_MONEDA_F" + cod + " = '" + tipoCambio.INT_ID_MONEDA_F + "' ,"
          + cod + "FEC_MODIF_TIPOCAMBIO" + cod + " = '" + tipoCambio.FEC_MODIF_TIPOCAMBIO + "' ,"
          + cod + "DAT__TIPOCAMBIO" + cod + " = '" + tipoCambio.DAT__TIPOCAMBIO + "' ,"
          + cod + "BOOL_ESTATUS_TIPOCAMBIO" + cod + " = '" + tipoCambio.BOOL_ESTATUS_TIPOCAMBIO + "' "
          + " WHERE" + cod + "INT_ID_TIPOCAMBIO_P" + cod + "=" + tipoCambio.INT_ID_TIPOCAMBIO_P;


            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@INT_ID_TIPOCAMBIO_P", tipoCambio.INT_ID_TIPOCAMBIO_P);
                    cmd.Parameters.AddWithValue("@DBL_TIPOCAMBIO_OFICIAL", tipoCambio.DBL_TIPOCAMBIO_OFICIAL);
                    cmd.Parameters.AddWithValue("@INT_ID_MONEDA_F", tipoCambio.INT_ID_MONEDA_F);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_TIPOCAMBIO", tipoCambio.FEC_MODIF_TIPOCAMBIO);
                    cmd.Parameters.AddWithValue("@DAT__TIPOCAMBIO", tipoCambio.DAT__TIPOCAMBIO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_TIPOCAMBIO", tipoCambio.BOOL_ESTATUS_TIPOCAMBIO);

                    con.Open();
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

        public int Delete(TipoCambio tipoCambio)
        {
            string delete = "UPDATE " + cod + "CAT_TIPOCAMBIO" + cod + "SET" + cod + "BOOL_ESTATUS_TIPOCAMBIO" + cod + "='" + tipoCambio.BOOL_ESTATUS_TIPOCAMBIO + "' WHERE" + cod + "INT_ID_TIPOCAMBIO_P" + cod + "='" + tipoCambio.INT_ID_TIPOCAMBIO_P + "'";
            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", tipoCambio.BOOL_ESTATUS_TIPOCAMBIO);

                    con.Open();
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
