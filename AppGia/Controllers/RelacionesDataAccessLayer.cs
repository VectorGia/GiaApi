using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class RelacionesDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';


        public IEnumerable<Relacion> GetRelacionesAll()
        {
            Relacion relacion = new Relacion();
            string cadena = "SELECT * FROM" + cod + "TAB_RELACIONES" + cod + "WHERE " + cod + "INT_IDRELACION_P" + cod + "=" + relacion.INT_IDRELACION_P ;
            try
            {
                List<Relacion> lstRelacion = new List<Relacion>();
                using (Npgsql.NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        relacion.INT_IDRELACION_P = Convert.ToInt32(rdr["INT_IDPROYECTO_P"]);
                        relacion.INT_IDUSUARIO_F = Convert.ToInt32(rdr["INT_IDUSUARIO_F"]);
                        relacion.INT_IDGRUPO_F = Convert.ToInt32(rdr["INT_IDGRUPO_F"]);
                        relacion.INT_IDROL_F = Convert.ToInt32(rdr["INT_IDROL_F"]);
                        relacion.FEC_MODIF_RELACIONES = Convert.ToDateTime(rdr["FEC_MODIF_RELACIONES"]);                     
                        relacion.BOOL_ESTATUS_RELACION = Convert.ToBoolean(rdr["BOOL_ESTATUS_RELACION"]);
                       
                        lstRelacion.Add(relacion);
                    }
                    con.Close();
                }

                return lstRelacion;
            }
            catch
            {
                throw;
            }
        }


    }
}
