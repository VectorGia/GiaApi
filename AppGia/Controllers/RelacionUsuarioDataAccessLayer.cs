using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class RelacionUsuarioDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = '"';

        public RelacionUsuarioDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Relacion_Usuario> GetAllRelacionUsuario()
        {
            Relacion_Usuario relacionusuario = new Relacion_Usuario();
            string cadena = "SELECT *FROM " + cod + "TAB_RELACION_USUARIO" + cod;
            try
            {
                List<Relacion_Usuario> lstRelacionUsuario = new List<Relacion_Usuario>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        relacionusuario.SERIAL_IDRELACIONUSU_P = Convert.ToInt32(rdr["SERIAL_IDRELACIONUSU_P"]);
                        relacionusuario.INT_IDUSUARIO_P = Convert.ToInt32(rdr["INT_IDUSUARIO_F"]);
                        relacionusuario.INT_IDGRUPO_P = Convert.ToInt32(rdr["INT_IDGRUPO_F"]);
                        relacionusuario.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_F"]);
                        relacionusuario.INT_IDPANTALLA_P = Convert.ToInt32(rdr["INT_IDPANTALLA_P"]);
                        relacionusuario.INT_IDPERMISO_P = Convert.ToInt32(rdr["INT_IDPERMISO_P"]);
                        relacionusuario.FEC_MODIF_RELUSU = Convert.ToDateTime(rdr["FEC_MODIF_RELUSU"]);
                        relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_RELUSU"]);

                        lstRelacionUsuario.Add(relacionusuario);
                    }
                    con.Close();
                }

                return lstRelacionUsuario;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int update(Relacion_Usuario relacionusuario)
        {
            string add = "UPDATE " + cod + "TAB_RELACION_USUARIO" + cod +
            " SET " + cod + "INT_IDUSUARIO_P" + cod + "= " + "@INT_IDUSUARIO_P" + ","
            + cod + "INT_IDGRUPO_P" + cod + "= " + "@INT_IDGRUPO_P" + ","
            + cod + "INT_IDROL_P" + cod + "= " + "@INT_IDROL_P" + ","
            + cod + "INT_IDPANTALLA_P" + cod + "= " + "@INT_IDPANTALLA_P" + ","
            + cod + "INT_IDPERMISO_P" + cod + "= " + "@INT_IDPERMISO_P" + ","
            + cod + "BOOL_ESTATUS_LOGICO_RELUSU" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELUSU" + ","
            + cod + "FEC_MODIF_RELUSU" + cod + "= " + "@FEC_MODIF_RELUSU"
            + " WHERE " + cod + "SERIAL_IDRELACIONUSU_P" + cod + " = " + "@SERIAL_IDRELACIONUSU_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@SERIAL_IDRELACIONUSU_P", Value = relacionusuario.SERIAL_IDRELACIONUSU_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDUSUARIO_P", Value = relacionusuario.INT_IDUSUARIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_P", Value = relacionusuario.INT_IDGRUPO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_P", Value = relacionusuario.INT_IDROL_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = relacionusuario.INT_IDPANTALLA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_P", Value = relacionusuario.INT_IDPERMISO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELUSU", Value = relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELUSU", Value = DateTime.Now });
                    


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

        public int delete(Relacion_Usuario relacionusuario)
        {

            string add = "UPDATE " + cod + "TAB_RELACION_USUARIO" + cod +
            " SET " + cod + "BOOL_ESTATUS_LOGICO_RELUSU" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELUSU" + ","
            + cod + "FEC_MODIF_RELUSU" + cod + "= " + "@FEC_MODIF_RELUSU"
            + " WHERE " + cod + "SERIAL_IDRELACIONUSU_P" + cod + " = " + "@SERIAL_IDRELACIONUSU_P";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@SERIAL_IDRELACIONUSU_P", Value = relacionusuario.SERIAL_IDRELACIONUSU_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELUSU", Value = relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELUSU", Value = DateTime.Now });



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

        public int insert(Relacion_Usuario relacionusuario)

        {

            string add = "INSERT INTO" + cod + "TAB_RELACION_USUARIO" + cod + "(" + cod + "INT_IDUSUARIO_P" + cod + ","+ cod + "INT_IDGRUPO_P" + cod + "," + cod + "INT_IDROL_P" + cod + "," + cod + "INT_IDPANTALLA_P" + cod + "," + cod + "INT_IDPERMISO_P" + cod + "," + cod + "BOOL_ESTATUS_LOGICO_RELUSU" + cod + "," + cod + "FEC_MODIF_RELUSU" + cod + ") VALUES " +
                "(@INT_IDUSUARIO_P,@INT_IDGRUPO_P,@INT_IDROL_P,@INT_IDPANTALLA_P,@INT_IDPERMISO_P,@BOOL_ESTATUS_LOGICO_RELUSU,@FEC_MODIF_RELUSU)";
            try
            {

                {
                    con.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO_P", relacionusuario.INT_IDUSUARIO_P);
                    cmd.Parameters.AddWithValue("@INT_IDGRUPO_P", relacionusuario.INT_IDGRUPO_P);
                    cmd.Parameters.AddWithValue("@INT_IDROL_P", relacionusuario.INT_IDROL_P);
                    cmd.Parameters.AddWithValue("@INT_IDPANTALLA_P", relacionusuario.INT_IDPANTALLA_P);
                    cmd.Parameters.AddWithValue("@INT_IDPERMISO_P", relacionusuario.INT_IDPERMISO_P);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_RELUSU", relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_RELUSU", DateTime.Now);
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
