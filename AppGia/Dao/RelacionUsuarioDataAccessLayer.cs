using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models;
using AppGia.Util;
using Npgsql;
using NpgsqlTypes;

namespace AppGia.Dao
{
    public class RelacionUsuarioDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        char cod = ' ';

        public RelacionUsuarioDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Relacion_Usuario> GetAllRelacionUsuario()
        {
            string cadena =
                "SELECT usr.user_name as usuario, rol.str_nombre_rol as rol, tg.str_nombre_grupo as grupo, rel.* " +
                " FROM TAB_RELACION_USUARIO rel " +
                " join tab_usuario usr on usr.id = rel.int_idusuario_p" +
                " join tab_grupo tg on rel.int_idgrupo_p = tg.int_idgrupo_p" +
                " join cat_rol rol on rol.int_idrol_p = rel.int_idrol_p " +
                " where BOOL_ESTATUS_LOGICO_RELUSU = true;";
            try
            {
                List<Relacion_Usuario> lstRelacionUsuario = new List<Relacion_Usuario>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Relacion_Usuario relacionusuario = new Relacion_Usuario();
                        relacionusuario.SERIAL_IDRELACIONUSU_P = Convert.ToInt32(rdr["SERIAL_IDRELACIONUSU_P"]);
                        relacionusuario.INT_IDUSUARIO_P = Convert.ToInt32(rdr["INT_IDUSUARIO_P"]);
                        relacionusuario.INT_IDGRUPO_P = Convert.ToInt32(rdr["INT_IDGRUPO_P"]);
                        relacionusuario.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_P"]);
                        relacionusuario.pantalla = Convert.ToString(rdr["pantalla"]);
                        relacionusuario.permiso = Convert.ToString(rdr["permiso"]);
                        relacionusuario.FEC_MODIF_RELUSU = Convert.ToDateTime(rdr["FEC_MODIF_RELUSU"]);
                        relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU =
                            Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_RELUSU"]);
                        relacionusuario.usuario = Convert.ToString(rdr["usuario"]);
                        relacionusuario.grupo = Convert.ToString(rdr["grupo"]);
                        relacionusuario.rol = Convert.ToString(rdr["rol"]);

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
            finally
            {
                con.Close();
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

                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@SERIAL_IDRELACIONUSU_P",
                        Value = relacionusuario.SERIAL_IDRELACIONUSU_P
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@INT_IDUSUARIO_P",
                        Value = relacionusuario.INT_IDUSUARIO_P
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_P",
                        Value = relacionusuario.INT_IDGRUPO_P
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_P",
                        Value = relacionusuario.INT_IDROL_P
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@pantalla",
                        Value = relacionusuario.pantalla
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@permiso",
                        Value = relacionusuario.permiso
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELUSU",
                        Value = relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELUSU",
                        Value = DateTime.Now
                    });


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
            finally
            {
                con.Close();
            }
        }

        public int delete(Int32 id)
        {
            string add = "UPDATE " + cod + "TAB_RELACION_USUARIO" + cod +
                         " SET " + cod + "BOOL_ESTATUS_LOGICO_RELUSU" + cod + "= " + "false" + ","
                         + cod + "FEC_MODIF_RELUSU" + cod + "= " + "@FEC_MODIF_RELUSU"
                         + " WHERE " + cod + "SERIAL_IDRELACIONUSU_P" + cod + " = " + "@SERIAL_IDRELACIONUSU_P";

            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Integer, ParameterName = "@SERIAL_IDRELACIONUSU_P",
                        Value = id
                    });
                    cmd.Parameters.Add(new NpgsqlParameter()
                    {
                        NpgsqlDbType = NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELUSU",
                        Value = DateTime.Now
                    });


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
            finally
            {
                con.Close();
            }
        }

        public int insert(Relacion_Usuario relacionusuario)

        {
            string add = "INSERT INTO" + cod + " TAB_RELACION_USUARIO" + cod + "("
                         + cod + "INT_IDUSUARIO_P" + cod + ","
                         + cod + "INT_IDGRUPO_P" + cod + ","
                         + cod + "INT_IDROL_P" + cod + ","
                         + cod + "pantalla" + cod + ","
                         + cod + "permiso" + cod + ","
                         + cod + "BOOL_ESTATUS_LOGICO_RELUSU" + cod + ","
                         + cod + "FEC_MODIF_RELUSU" + cod + ") " +
                         "VALUES " +
                         "(@INT_IDUSUARIO_P,@INT_IDGRUPO_P," +
                         "@INT_IDROL_P,@pantalla," +
                         "@permiso,@BOOL_ESTATUS_LOGICO_RELUSU,@FEC_MODIF_RELUSU)";
            try
            {
                {
                    con.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@INT_IDUSUARIO_P", relacionusuario.INT_IDUSUARIO_P);
                    cmd.Parameters.AddWithValue("@INT_IDGRUPO_P", relacionusuario.INT_IDGRUPO_P);
                    cmd.Parameters.AddWithValue("@INT_IDROL_P", relacionusuario.INT_IDROL_P);
                    cmd.Parameters.AddWithValue("@pantalla", relacionusuario.pantalla);
                    cmd.Parameters.AddWithValue("@permiso", relacionusuario.permiso);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_RELUSU",
                        relacionusuario.BOOL_ESTATUS_LOGICO_RELUSU);
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
            finally
            {
                con.Close();
            }
        }

        public List<Dictionary<string, string>> getRelacionesByUserName(string username)
        {
            string query =
                "SELECT " +
                " pantalla, " +
                " permiso, " +
                " rol.str_nombre_rol as rol," +
                " tg.str_nombre_grupo as grupo " +
                " FROM TAB_RELACION_USUARIO rel join tab_usuario usr on usr.id = rel.int_idusuario_p " +
                " join tab_grupo tg on rel.int_idgrupo_p = tg.int_idgrupo_p" +
                " join cat_rol rol on rol.int_idrol_p = rel.int_idrol_p" +
                " where BOOL_ESTATUS_LOGICO_RELUSU = true" +
                " and usr.user_name = @user_name";
            DataTable dataTable = new QueryExecuter().ExecuteQuery(query, new NpgsqlParameter("@user_name", username));
            List<Dictionary<string, string>> relaciones = new List<Dictionary<string, string>>();
            foreach (DataRow rdr in dataTable.Rows)
            {
               Dictionary<string,string> dictionary=new Dictionary<string, string>();
               dictionary.Add("pantalla",rdr["pantalla"].ToString());
               dictionary.Add("permiso",rdr["permiso"].ToString());
               dictionary.Add("rol",rdr["rol"].ToString());
               dictionary.Add("grupo",rdr["grupo"].ToString());
               relaciones.Add(dictionary);
            }

            return relaciones;
        }
    }
}