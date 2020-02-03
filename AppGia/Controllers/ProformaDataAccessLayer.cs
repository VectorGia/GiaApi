using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ProformaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public ProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Proforma> GetProforma(int idProforma)
        {
            string cadena = "";
            cadena += " select id, anio, modelo_negocio_id, tipo_captura_id, tipo_proforma_id, centro_costo_id, activo, usuario, fecha_captura ";
            cadena += " from proforma ";
            cadena += " where id = " + idProforma.ToString();
            cadena += " and activo = 'true' ";

            try
            {
                List<Proforma> lstProforma = new List<Proforma>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);

                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Proforma proforma = new Proforma();
                    proforma.id = Convert.ToInt32(rdr["id"]);
                    proforma.modelo_negocio_id = Convert.ToInt32(rdr["modelo_negocio_id"]);
                    proforma.tipo_captura_id = Convert.ToInt32(rdr["tipo_captura_id"]);
                    proforma.tipo_proforma_id = Convert.ToInt32(rdr["tipo_proforma_id"]);
                    proforma.centro_costo_id = Convert.ToInt32(rdr["centro_costo_id"]);
                    proforma.activo = Convert.ToBoolean(rdr["activo"]);
                    proforma.usuario = Convert.ToInt32(rdr["usuario"]);
                    proforma.fecha_captura = Convert.ToDateTime(rdr["fecha_captura"]);
                    lstProforma.Add(proforma);
                }

                return lstProforma;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

        }

        public int AddProforma(Proforma proforma)
        {
            string cadena = "";
            cadena += " insert into proforma ( ";
            cadena += " 	id, anio, modelo_negocio_id, tipo_captura_id, tipo_proforma_id, centro_costo_id, activo, usuario, fecha_captura ";
            cadena += " ) values ( ";
            cadena += " 	nextval('seq_proforma'), @anio, @modelo_negocio_id, @tipo_captura_id, @tipo_proforma_id, @centro_costo_id, @activo, @usuario, @fecha_captura ";
            cadena += " ) ";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                cmd.Parameters.AddWithValue("@anio", proforma.anio);
                cmd.Parameters.AddWithValue("@modelo_negocio_id", proforma.modelo_negocio_id);
                cmd.Parameters.AddWithValue("@tipo_captura_id", proforma.tipo_captura_id);
                cmd.Parameters.AddWithValue("@tipo_proforma_id", proforma.tipo_proforma_id);
                cmd.Parameters.AddWithValue("@centro_costo_id", proforma.centro_costo_id);
                cmd.Parameters.AddWithValue("@activo", proforma.activo);
                cmd.Parameters.AddWithValue("@usuario", proforma.usuario);
                cmd.Parameters.AddWithValue("@fecha_captura", proforma.fecha_captura);

                con.Open();
                int regInsert = cmd.ExecuteNonQuery();

                return regInsert;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int UpdateProforma(int idProforma, bool bandActivo, int idUsuario)
        {
            string cadena = "";
            cadena += " update proforma set activo = '" + bandActivo.ToString() + "', ";
            cadena += " 	usuario = " + idUsuario.ToString() + ", fecha_captura = current_timestamp ";
            cadena += " 	where id = " + idProforma.ToString();

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cadena, conex.ConnexionDB());

                con.Open();
                int regActual = cmd.ExecuteNonQuery();
                return regActual;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
