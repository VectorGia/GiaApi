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
            string cadena = "SELECT * FROM" + cod + "CAT_MONEDA" + cod + "WHERE " + cod + "BOOL_ESTATUS_LOGICO_MONEDA" + cod + "=" + true;
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
                        moneda.id = Convert.ToInt32( rdr["INT_IDMONEDA_P"]);
                        moneda.descripcion = rdr["STR_DESCRIPCION"].ToString().Trim();
                        moneda.clave = rdr["STR_CLAVEDESC"].ToString().Trim();
                        moneda.pais = rdr["STR_PAIS"].ToString().Trim();
                        moneda.activo = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_MONEDA"]);
                        moneda.estatus = Convert.ToBoolean( rdr["BOOL_ESTATUS_MONEDA"]);
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

        public Moneda GetMoneda(string id)
        {
            try
            {
                Moneda moneda = new Moneda();
                string consulta = "SELECT * FROM" + cod + "CAT_MONEDA" + cod + "WHERE " + cod + "INT_IDMONEDA_P" + cod + "=" + id;
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    moneda.id = Convert.ToInt32(rdr["INT_IDMONEDA_P"]);
                    moneda.descripcion = rdr["STR_DESCRIPCION"].ToString().Trim();
                    moneda.clave = rdr["STR_CLAVEDESC"].ToString().Trim();
                    moneda.pais = rdr["STR_PAIS"].ToString().Trim();
                    moneda.activo = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_MONEDA"]);
                    moneda.estatus = Convert.ToBoolean(rdr["BOOL_ESTATUS_MONEDA"]);
                }
                con.Close();
                return moneda;
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
                        + cod + "BOOL_ESTATUS_LOGICO_MONEDA" + cod + ","
                        + cod + "BOOL_ESTATUS_MONEDA" + cod + ")"
                        + " VALUES ( @STR_DESCRIPCION" + ","
                        + "@STR_CLAVEDESC" + ","
                        + "@STR_PAIS" + ","
                        + "@BOOL_ESTATUS_LOGICO_MONEDA" + ","
                        + "@BOOL_ESTATUS_MONEDA"
                        + ")";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@STR_DESCRIPCION", moneda.descripcion.Trim());
                    cmd.Parameters.AddWithValue("@STR_CLAVEDESC",moneda.clave.Trim());
                    cmd.Parameters.AddWithValue("@STR_PAIS", moneda.pais.Trim());
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_MONEDA", moneda.activo);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_MONEDA",moneda.estatus);

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

        public int update(string id, Moneda moneda)
        {

            string update = "UPDATE " + cod + "CAT_MONEDA" + cod + "SET"

          + cod + "STR_DESCRIPCION" + cod + " = '" + moneda.descripcion + "' ,"
          + cod + "STR_CLAVEDESC" + cod + " = '" + moneda.clave + "' ,"
          + cod + "STR_PAIS" + cod + " = '" + moneda.pais + "' ,"
          + cod + "BOOL_ESTATUS_MONEDA" + cod + " = '" + moneda.activo + "' "
          + " WHERE" + cod + "INT_IDMONEDA_P" + cod + "=" + id;


            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);


                    cmd.Parameters.AddWithValue("@STR_DESCRIPCION", moneda.descripcion.Trim());
                    cmd.Parameters.AddWithValue("@STR_CLAVEDESC",moneda.clave);
                    cmd.Parameters.AddWithValue("@STR_PAIS",moneda.pais);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_MONEDA", moneda.activo); 

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

        public int Delete(string  id)
        {
            string status = "false";
            string delete = "UPDATE " + cod + "CAT_MONEDA" + cod + "SET"
               + cod + "BOOL_ESTATUS_LOGICO_MONEDA" + cod + "='" + status + "' " +
               "WHERE" + cod + "INT_IDMONEDA_P" + cod + "='" + id + "'";
            try
                {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                   
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
