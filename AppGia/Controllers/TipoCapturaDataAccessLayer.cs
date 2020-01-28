using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class TipoCapturaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public TipoCapturaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<TipoCaptura> GetAllTipoCaptura()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_TIPO_CAPTURA" + cod + "WHERE " + cod + "BOOL_TIPOCAPTURA_ACTIVO" + cod + "=" + true;
            try
            {
                List<TipoCaptura> lsttipocaptura = new List<TipoCaptura>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        TipoCaptura tipocaptura = new TipoCaptura();
                        tipocaptura.INT_IDTIPOCAPTURA_P = Convert.ToInt32(rdr["INT_IDTIPOCAPTURA_P"]);
                        tipocaptura.STR_DESCRIP_TIPOCAPTURA = rdr["STR_DESCRIP_TIPOCAPTURA"].ToString().Trim();
                        tipocaptura.INT_IDUSUARIO_F = Convert.ToInt32(rdr["INT_IDUSUARIO_F"]);
                        tipocaptura.FEC_MODIF_TIPOCAPTURA = Convert.ToDateTime(rdr["FEC_MODIF_TIPOCAPTURA"]);
                        tipocaptura.BOOL_TIPOCAPTURA_ACTIVO = Convert.ToBoolean(rdr["BOOL_TIPOCAPTURA_ACTIVO"]);
                        lsttipocaptura.Add(tipocaptura);
                    }
                    con.Close();
                }
                return lsttipocaptura;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public TipoCaptura GetTipoCapturaData(string id)
        {
            try
            {
                TipoCaptura tipocaptura = new TipoCaptura();
                {
                    string consulta = "SELECT * FROM" + cod + "CAT_TIPO_CAPTURA" + cod + "WHERE" + cod + "INT_IDTIPOCAPTURA_P" + cod + "=" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        tipocaptura.INT_IDTIPOCAPTURA_P = Convert.ToInt32(rdr["INT_IDTIPOCAPTURA_P"]);
                        tipocaptura.STR_DESCRIP_TIPOCAPTURA = rdr["STR_DESCRIP_TIPOCAPTURA"].ToString().Trim();
                        tipocaptura.INT_IDUSUARIO_F = Convert.ToInt32(rdr["INT_IDUSUARIO_F"]);
                        tipocaptura.FEC_MODIF_TIPOCAPTURA = Convert.ToDateTime(rdr["FEC_MODIF_TIPOCAPTURA"]);
                        tipocaptura.BOOL_TIPOCAPTURA_ACTIVO = Convert.ToBoolean(rdr["BOOL_TIPOCAPTURA_ACTIVO"]);

                    }
                    con.Close();
                }
                return tipocaptura;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int AddTipoCaptura(TipoCaptura tipocaptura)
        {
            string add = "INSERT INTO" + cod +
                "CAT_TIPO_CAPTURA" + cod +
                "(" + cod + "STR_DESCRIP_TIPOCAPTURA" + cod +
                "," + cod + "INT_IDUSUARIO_F" + cod +
                "," + cod + "FEC_MODIF_TIPOCAPTURA" + cod +
                "," + cod + "BOOL_TIPOCAPTURA_ACTIVO" + cod +
                ") VALUES " +
                "(@STR_DESCRIP_TIPOCAPTURA," +
                "@INT_IDUSUARIO_F," +
                "@FEC_MODIF_TIPOCAPTURA," +
                "@BOOL_TIPOCAPTURA_ACTIVO)";
            try
            {

                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_DESCRIP_TIPOCAPTURA", tipocaptura.STR_DESCRIP_TIPOCAPTURA.Trim());
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO_F", tipocaptura.INT_IDUSUARIO_F);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_TIPOCAPTURA", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_TIPOCAPTURA_ACTIVO", tipocaptura.BOOL_TIPOCAPTURA_ACTIVO);

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

        public int Update(string id, TipoCaptura tipocaptura)

        {

            string update = "UPDATE " + cod + "CAT_TIPO_CAPTURA"
                + cod + "SET" + cod + "STR_DESCRIP_TIPOCAPTURA" + cod + "=" + "@STR_DESCRIP_TIPOCAPTURA" + ","
                + cod + "INT_IDUSUARIO_F" + cod + "= " + "@INT_IDUSUARIO_F" + ","
                + cod + "FEC_MODIF_TIPOCAPTURA" + cod + "= " + "@FEC_MODIF_TIPOCAPTURA" + ","
                + cod + "BOOL_TIPOCAPTURA_ACTIVO" + cod + "= " + "@BOOL_TIPOCAPTURA_ACTIVO" + ","
                + " WHERE " + cod + "INT_IDTIPOCAPTURA_P" + cod + "=" + id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update.Trim(), con);
                    cmd.Parameters.AddWithValue("STR_DESCRIP_TIPOCAPTURA", tipocaptura.STR_DESCRIP_TIPOCAPTURA);
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO_F", tipocaptura.INT_IDUSUARIO_F);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_TIPOCAPTURA", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_TIPOCAPTURA_ACTIVO", tipocaptura.BOOL_TIPOCAPTURA_ACTIVO);

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
            string delete = "UPDATE " + cod + "CAT_TIPO_CAPTURA" + cod + "SET"
                + cod + "BOOL_TIPOCAPTURA_ACTIVO" + cod + "='" + status + "' " +
                "WHERE" + cod + "INT_IDTIPOCAPTURA_P" + cod + "='" + id + "'";

            try
            {
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


        }
    }
}
