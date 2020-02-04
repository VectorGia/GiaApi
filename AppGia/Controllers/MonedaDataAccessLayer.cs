
//using AppGia.Models;
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
            //string cadena = "SELECT * FROM" + cod + "CAT_MONEDA" + cod + "WHERE " + cod + "BOOL_ESTATUS_LOGICO_MONEDA" + cod + "=" + true;
            string cadena = "select id,activo,clave,descripcion,pais,simbolo FROM public.moneda WHERE activo = true";
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
                        moneda.id = Convert.ToInt64(rdr["id"]);
                        moneda.activo = Convert.ToBoolean(rdr["activo"]);
                        moneda.clave = rdr["clave"].ToString().Trim();
                        moneda.descripcion = rdr["descripcion"].ToString().Trim();
                        moneda.pais = rdr["pais"].ToString();
                        moneda.simbolo = rdr["simbolo"].ToString();
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
                string consulta = "select id,activo,clave,descripcion,pais,simbolo FROM public.moneda WHERE id = " + id;
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    moneda.id = Convert.ToInt64(rdr["id"]);
                    moneda.activo = Convert.ToBoolean(rdr["activo"]);
                    moneda.clave = rdr["clave"].ToString();
                    moneda.descripcion = rdr["descripcion"].ToString().Trim();
                    moneda.pais = rdr["pais"].ToString().Trim();
                    moneda.simbolo = rdr["simbolo"].ToString();
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
            string add = "INSERT INTO public.moneda ( "
                        + "id,"
                        + "activo,"
                        + "clave,"
                        + "descripcion,"
                        + "pais,"
                        + "simbolo)"
                        + " VALUES (@nextval(seq_moneda),"
                        + "@activo,"
                        + "@clave,"
                        + "@descripcion,"
                        + "@pais,"
                        + "@simbolo"
                        + ")";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@activo", moneda.activo);
                    cmd.Parameters.AddWithValue("@clave", moneda.clave.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", moneda.descripcion.Trim());
                    cmd.Parameters.AddWithValue("@pais", moneda.pais);
                    cmd.Parameters.AddWithValue("@simbolo", moneda.simbolo);

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

            string update = "UPDATE moneda SET "
                            + "clave = " + moneda.clave + ","
                            + "descripcion = " + moneda.descripcion + ","
                            + "pais = " + moneda.pais + ","
                            + "simbolo = " + moneda.simbolo
                            + " WHERE id = " + id;


            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@clave", moneda.clave.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", moneda.descripcion);
                    cmd.Parameters.AddWithValue("@pais", moneda.pais);
                    cmd.Parameters.AddWithValue("@simbolo", moneda.simbolo);

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

        public int Delete(string id)
        {
            string status = "false";
            string delete = "UPDATE moneda SET activo = " + status
                + "WHERE id = " + id;
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
