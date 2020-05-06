using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class TipoProformaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public TipoProformaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Tipo_Proforma> GetAllTipoProformas()
        {
            string cadena = "select * from tipo_proforma where activo = true order by mes_inicio asc";

            try
            {
                List<Tipo_Proforma> lsttipoproforma = new List<Tipo_Proforma>();
                NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Tipo_Proforma tipoproforma = new Tipo_Proforma();
                    tipoproforma.id = Convert.ToInt64(rdr["id"]);
                    tipoproforma.nombre = rdr["nombre"].ToString();
                    tipoproforma.clave = rdr["clave"].ToString();
                    tipoproforma.descripcion = rdr["descripcion"].ToString();
                    tipoproforma.activo = Convert.ToBoolean(rdr["activo"]);
                    tipoproforma.mes_inicio = Convert.ToInt32(rdr["mes_inicio"]);
                    lsttipoproforma.Add(tipoproforma);

                }
                con.Close();
                return lsttipoproforma;
            }

            catch
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
