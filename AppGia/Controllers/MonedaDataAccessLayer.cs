using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class MonedaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public MonedaDataAccessLayer()
        {

            con = conex.ConnexionDB();
        }

        public IEnumerable<Moneda> GetAllMoneda()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_MONEDA" + cod + "";
            try
            {
                List<Moneda> lstNoneda = new List<Moneda>();


                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Moneda moneda = new Moneda();
                        moneda.INT_IDMONEDA =Convert.ToInt32( rdr["INT_IDMONEDA"]);
                        moneda.STR_DESCRIPCION = rdr["STR_DESCRIPCION"].ToString().Trim();
                        moneda.STR_CLAVEDESC = rdr["STR_CLAVEDESC"].ToString().Trim();
                        moneda.STR_PAIS = rdr["STR_PAIS"].ToString().Trim();
                        moneda.BOOL_ESTATUS_MONEDA = Convert.ToBoolean( rdr["BOOL_ESTATUS_MONEDA"]);
                        lstNoneda.Add(moneda);
                    }
                    con.Close();
                }

                return
                        lstNoneda;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int insert(Moneda moneda)
        {
            string add = "INSERT INTO " + cod + "CAT_MONEDA" + cod
                        + "(" 
                        + cod + "STR_DESCRIPCION" + cod + ","
                        + cod + "STR_CLAVEDESC" + cod + ","
                        + cod + "STR_PAIS" + cod + ","
                        + cod + "BOOL_ESTATUS_MONEDA" + cod + ")"
                        + " VALUES ( @STR_DESCRIPCION" + ","
                        + "@STR_CLAVEDESC" + ","
                        + "@STR_PAIS" + ","
                        + "@BOOL_ESTATUS_MONEDA"
                        + ")";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@STR_DESCRIPCION", moneda.STR_DESCRIPCION.Trim());
                    cmd.Parameters.AddWithValue("@STR_CLAVEDESC",moneda.STR_CLAVEDESC.Trim());
                    cmd.Parameters.AddWithValue("@STR_PAIS", moneda.STR_PAIS.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_MONEDA",moneda.BOOL_ESTATUS_MONEDA);

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

        public int update(Moneda moneda)
        {

            string update = "UPDATE " + cod + "CAT_MONEDA" + cod + "SET"

          + cod + "STR_DESCRIPCION" + cod + " = '" + moneda.STR_DESCRIPCION + "' ,"
          + cod + "STR_CLAVEDESC" + cod + " = '" + moneda.STR_CLAVEDESC + "' ,"
          + cod + "STR_PAIS" + cod + " = '" + moneda.STR_PAIS + "' ,"
          + cod + "BOOL_ESTATUS_MONEDA" + cod + " = '" + moneda.BOOL_ESTATUS_MONEDA + "' "
          + " WHERE" + cod + "INT_IDMONEDA" + cod + "=" + moneda.INT_IDMONEDA;


            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);


                    cmd.Parameters.AddWithValue("@STR_DESCRIPCION", moneda.STR_DESCRIPCION.Trim());
                    cmd.Parameters.AddWithValue("@STR_CLAVEDESC",moneda.STR_CLAVEDESC);
                    cmd.Parameters.AddWithValue("@STR_PAIS",moneda.STR_PAIS);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_MONEDA", moneda.BOOL_ESTATUS_MONEDA); 

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

        public int Delete(Moneda  moneda)
        {
            string delete = "SELECT * FROM" + cod + "CAT_MONEDA" + cod + "' WHERE" + cod + "INT_IDMONEDA" + cod + "='" + moneda.INT_IDMONEDA + "'";
            try
                {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@INT_IDMONEDA", moneda.INT_IDMONEDA);

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
