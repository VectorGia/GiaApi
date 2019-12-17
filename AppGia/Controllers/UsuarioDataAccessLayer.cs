using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class UsuarioDataAccessLayer
    {
        string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';

        public IEnumerable<Usuario> GetAllUsuarios()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_USUARIO" + cod + "";
            try
            {
                List<Usuario> lstUsuario = new List<Usuario>();
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Usuario usuario = new Usuario();

                        usuario.STR_USERNAME_USUARIO = rdr["STR_USERNAME_USUARIO"].ToString();
                        usuario.STR_NOMBRE_USUARIO = rdr["STR_USERNAME_USUARIO"].ToString();
                        usuario.STR_EMAIL_USUARIO = rdr["STR_EMAIL_USUARIO"].ToString();
                        usuario.STR_PASSWORD_USUARIO = rdr["STR_PASSWORD_USUARIO"].ToString();
                        usuario.STR_PUESTO = rdr["STR_PUESTO"].ToString();
                        usuario.BOOL_ESTATUS_LOGICO_USUARIO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_USUARIO"]);
                        usuario.FEC_MODIF_USUARIO = Convert.ToDateTime(rdr["FEC_MODIF_USUARIO"]);

                        lstUsuario.Add(usuario);
                    }
                    con.Close();
                }

                return lstUsuario;
            }
            catch
            {
                throw;
            }
        }

        public int addUsuario(Usuario usuario)
        {
            string add = "INSERT INTO" + cod + "TAB_USUARIO" + cod + "("
                + cod + "STR_NOMBRE_USUARIO" + cod + ","
                + cod + "SRT_USERNAME_USUARIO" + cod + ","
                + cod + "SRT_PUESTO" + cod + ","
                + cod + "STR_EMAIL_USUARIO" + cod + ","
                + cod + "STR_PASSWORD_USUARIO" + cod + ") " +
                "VALUES " +
                "(@STR_NOMBRE_USUARIO)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@STR_USERNAME_USUARIO", usuario.STR_USERNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@STR_EMAIL_USUARIO", usuario.STR_EMAIL_USUARIO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_USUARIO", usuario.BOOL_ESTATUS_LOGICO_USUARIO);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_USUARIO", usuario.STR_NOMBRE_USUARIO);
                    cmd.Parameters.AddWithValue("@STR_PUESTO", usuario.STR_PUESTO);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_USUARIO", usuario.FEC_MODIF_USUARIO);
                    cmd.Parameters.AddWithValue("@STR_PASSWORD_USUARIO", usuario.STR_PASSWORD_USUARIO);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int UpdateUsuario(Usuario usuario)
        {

            string add = "UPDATE " + cod + "TAB_USUARIO" + cod +
            " SET " + cod + "STR_NOMBRE_USUARIO" + cod + "= " + "'" + "@STR_NOMBRE_USUARIO" + "'" + ","
            + cod + "STR_EMAIL_USUARIO" + cod + "= " + "'" + "@STR_EMAIL_USUARIO" + "'" + ","
            + cod + "BOOL_ESTATUS_LOGICO_USUARIO" + cod + "= " + "'" + "@BOOL_ESTATUS_LOGICO_USUARIO" + "'" + ","
            + cod + "SRT_USERNAME_USUARIO" + cod + "= " + "'" + "@SRT_USERNAME_USUARIO" + "'" + ","
            + cod + "SRT_DISPLAYNAME_USUARIO" + cod + "= " + "'" + "@SRT_DISPLAYNAME_USUARIO" + "'" + ","
            + cod + "SRT_PUESTO" + cod + "= " + "'" + "@SRT_PUESTO" + "'"
            + " WHERE " + cod + "INT_IDUSUARIO_P" + cod + " = " + "@INT_IDUSUARIO_P";

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDUSUARIO_P", Value = usuario.INT_IDUSUARIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_USUARIO", Value = usuario.STR_NOMBRE_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_EMAIL_USUARIO", Value = usuario.STR_EMAIL_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_USUARIO", Value = usuario.BOOL_ESTATUS_LOGICO_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@SRT_USERNAME_USUARIO", Value = usuario.STR_USERNAME_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@SRT_PUESTO", Value = usuario.STR_PUESTO });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch
            {
                throw;

            }

        }

        public int DeleteUsuario(int id)
        {
            try
            {

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


                    cmd.Parameters.AddWithValue("STR_IDCENTROCOSTO", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }

    }
}
