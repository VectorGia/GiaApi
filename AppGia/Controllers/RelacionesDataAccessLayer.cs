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
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = '"';

        public RelacionesDataAccessLayer() {

            con = conex.ConnexionDB();
        }

        public IEnumerable<Relacion> GetAllRelaciones()
        {
            Relacion relacion = new Relacion();
            string cadena = "SELECT *FROM " + cod + "TAB_RELACIONES" + cod;
            try
            {
                List<Relacion> lstRelacion = new List<Relacion>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        relacion.INT_IDRELACION_P = Convert.ToInt32(rdr["INT_IDRELACION_P"]);
                        relacion.INT_IDUSUARIO_P = Convert.ToInt32(rdr["INT_IDUSUARIO_F"]);
                        relacion.INT_IDGRUPO_P = Convert.ToInt32(rdr["INT_IDGRUPO_F"]);
                        relacion.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_F"]);
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
                con.Close();
                throw;
            }
        }

        public int update(Relacion relacion)
        {
            string add = "UPDATE " + cod + "TAB_RELACIONES" + cod +
            " SET " + cod + "INT_IDGRUPO_F" + cod + "= " + "@INT_IDGRUPO_F" + ","
            + cod + "INT_IDROL_F" + cod + "= " + "@INT_IDROL_F" + ","
            + cod + "INT_IDUSUARIO_F" + cod + "= " + "@INT_IDUSUARIO_F" + ","
            + cod + "BOOL_ESTATUS_RELACION" + cod + "= " + "@BOOL_ESTATUS_RELACION" + ","
            + cod + "FEC_MODIF_RELACIONES" + cod + "= " + "@FEC_MODIF_RELACIONES"
            + " WHERE " + cod + "INT_IDRELACION_P" + cod + " = " + "@INT_IDRELACION_P";

            try
            {
               
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_P", Value = relacion.INT_IDRELACION_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_F", Value = relacion.INT_IDGRUPO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_F", Value = relacion.INT_IDROL_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDUSUARIO_F", Value = relacion.INT_IDUSUARIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_RELACION", Value = relacion.BOOL_ESTATUS_RELACION });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELACIONES", Value = relacion.FEC_MODIF_RELACIONES });


                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch
            {
                con.Close();
                throw;

            }

        }

        public int delete(Relacion relacion)
        {

            string add = "UPDATE " + cod + "TAB_RELACIONES" + cod +
            " SET " + cod + "BOOL_ESTATUS_RELACION" + cod + "= " + "@BOOL_ESTATUS_RELACION"
            + " WHERE " + cod + "INT_IDRELACION_P" + cod + " = " + "@INT_IDRELACION_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_P", Value = relacion.INT_IDRELACION_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_RELACION", Value = relacion.BOOL_ESTATUS_RELACION });



                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch
            {
                con.Close();
                throw;

            }

        }

        public int insert(Relacion relacion)

        {
            
            string add = "INSERT INTO" + cod + "TAB_RELACIONES" + cod + "(" + cod + "INT_IDGRUPO_F" + cod + "," + cod + "INT_IDROL_F" + cod + "," + cod + "INT_IDUSUARIO_F" + cod + "," + cod + "BOOL_ESTATUS_RELACION" + cod + "," + cod + "FEC_MODIF_RELACIONES" + cod + ") VALUES " +
                "(@INT_IDGRUPO_F,@INT_IDROL_F,@INT_IDUSUARIO_F,@BOOL_ESTATUS_RELACION,@FEC_MODIF_RELACIONES)";
            try
            {

                {

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@INT_IDGRUPO_F", relacion.INT_IDGRUPO_P);
                    cmd.Parameters.AddWithValue("@INT_IDROL_F", relacion.INT_IDROL_P);
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO_F", relacion.INT_IDUSUARIO_P);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_RELACION", relacion.BOOL_ESTATUS_RELACION);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_RELACIONES", DateTime.Now);
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

