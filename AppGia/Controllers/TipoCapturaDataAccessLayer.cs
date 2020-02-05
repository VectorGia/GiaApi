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
            string cadena = "SELECT id, activo, clave, descripcion, fec_modif, idusuario FROM public.tipo_captura WHERE activo = true;";
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
                        tipocaptura.id = Convert.ToInt64(rdr["id"]);
                        tipocaptura.activo = Convert.ToBoolean(rdr["activo"]);
                        tipocaptura.clave = rdr["clave"].ToString().Trim();
                        tipocaptura.descripcion = rdr["descripcion"].ToString(); ;
                        tipocaptura.fec_modif = rdr["fec_modif"].ToString();
                        tipocaptura.idusuario = Convert.ToInt64(rdr["iduduario"]);
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
                    string consulta = "SELECT id, activo, clave, descripcion, fec_modif, idusuario FROM tipo_captura where id =" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        tipocaptura.id = Convert.ToInt64(id);
                        tipocaptura.activo = Convert.ToBoolean(rdr["activo"]);
                        tipocaptura.clave = rdr["clave"].ToString().Trim();
                        tipocaptura.descripcion = rdr["descripcion"].ToString(); ;
                        tipocaptura.fec_modif = rdr["fec_modif"].ToString();
                        tipocaptura.idusuario = Convert.ToInt64(rdr["iduduario"]);
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
            string add = "INSERT INTO public.tipo_captura(id, activo, clave, descripcion, fec_modif, idusuario)"
                + "VALUES "
                + "("
                + "@id,"
                + "@activo,"
                + "@clave,"
                + "@descripcion,"
                + "@fec_modif,"
                + "@idusuario"
                + ")";
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@id", tipocaptura.id);
                    cmd.Parameters.AddWithValue("@activo", tipocaptura.activo);
                    cmd.Parameters.AddWithValue("@clave", tipocaptura.clave);
                    cmd.Parameters.AddWithValue("@descripcion", tipocaptura.descripcion);
                    cmd.Parameters.AddWithValue("@fec_modif", tipocaptura.fec_modif);
                    cmd.Parameters.AddWithValue("@fec_modif", tipocaptura.fec_modif);
                    cmd.Parameters.AddWithValue("@idusuario", tipocaptura.idusuario);

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
            string update = "UPDATE tipo_captura SET " +
                " activo=@activo," +
                " clave=@clave, " +
                " descripcion=@descripcion, " +
                " fec_modif=@fec_modif, " +
                " idusuario=@idusuario " +
                " WHERE id=@id;";

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update.Trim(), con);
                    cmd.Parameters.AddWithValue("activo", tipocaptura.activo);
                    cmd.Parameters.AddWithValue("@descripcion", tipocaptura.descripcion);
                    cmd.Parameters.AddWithValue("@fec_modif", DateTime.Now);
                    cmd.Parameters.AddWithValue("@idusuario", tipocaptura.idusuario);
                    cmd.Parameters.AddWithValue("@id", id);

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
            string delete = "UPDATE tipo_captura SET activo = " + status + " WHERE id = @id";
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
