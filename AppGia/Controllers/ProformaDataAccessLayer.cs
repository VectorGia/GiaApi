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
            cadena += " select id, anio, periodo_id, modelo_negocio_id, tipo_captura_id, fecha_captura ";
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
            cadena += " 	anio, periodo_id, fecha_captura, modelo_negocio_id, tipo_captura_id, activo ";
            cadena += " ) values ( ";
            cadena += " 	@anio, @periodo_id, @fecha_captura, @modelo_negocio_id, @tipo_captura_id, @activo ";
            cadena += " ) ";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                cmd.Parameters.AddWithValue("@anio", proforma.anio);
                cmd.Parameters.AddWithValue("@periodo_id", proforma.periodo_id);
                cmd.Parameters.AddWithValue("@fecha_captura", proforma.fecha_captura);
                cmd.Parameters.AddWithValue("@modelo_negocio_id", proforma.modelo_negocio_id);
                cmd.Parameters.AddWithValue("@tipo_captura_id", proforma.tipo_captura_id);
                cmd.Parameters.AddWithValue("@activo", proforma.activo);

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

        public int UpdateProforma(int idProforma, bool bandActivo)
        {
            string cadena = "";
            cadena += " update proforma set activo = '" + bandActivo.ToString() + "' ";
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
