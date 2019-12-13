using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class UsuarioDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
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

                        usuario.STR_NOMBRE_USUARIO = rdr["STR_NOMBRE_USUARIO"].ToString();
                        usuario.STR_EMAIL_USUARIO = rdr["STR_EMAIL_USUARIO"].ToString();
                        //usuario.BOOL_ESTATUS_USUARIO = Convert.ToBoolean(rdr["BOOL_ESTATUS_USUARIO"]);
                        usuario.STR_USERNAME_USUARIO = rdr["SRT_USERNAME_USUARIO"].ToString();
                        usuario.STR_DISPLAYNAME_USUARIO = rdr["SRT_DISPLAYNAME_USUARIO"].ToString();
                        usuario.BOOL_PUESTO = Convert.ToBoolean(rdr["SRT_PUESTO"]);

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
            string add = "INSERT INTO" + cod + "TAB_USUARIO" + cod + "(" + cod + "STR_NOMBRE_USUARIO" + cod + ") VALUES " +
                "(@STR_NOMBRE_USUARIO)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_USUARIO", usuario.STR_NOMBRE_USUARIO);
                    cmd.Parameters.AddWithValue("@STR_EMAIL_USUARIO", usuario.STR_EMAIL_USUARIO);
                    //cmd.Parameters.AddWithValue("@BOOL_ESTATUS_USUARIO", usuario.BOOL_ESTATUS_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_USERNAME_USUARIO", usuario.STR_USERNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_DISPLAYNAME_USUARIO", usuario.STR_DISPLAYNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_PUESTO", usuario.STR_PUESTO);

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
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@SRT_DISPLAYNAME_USUARIO", Value = usuario.STR_DISPLAYNAME_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@SRT_PUESTO", Value = usuario.STR_PUESTO });


                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch
            {
                throw;
            }
        }

        //public int DeleteUsuario(int id)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


        //            cmd.Parameters.AddWithValue("STR_IDCENTROCOSTO", id);

        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return 1;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public int DeleteGrupo(Usuario usuario)
        {
            string add = "UPDATE " + cod + "TAB_USUARIO" + cod + " SET " + cod + "BOOL_ESTATUS_LOGICO_USUARIO" + cod + "= " + "'" + "@BOOL_ESTATUS_LOGICO_USUARIO" + "'" + " WHERE " + cod + "INT_IDUSUARIO_P" + cod + " = " + "@INT_IDUSUARIO_P";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDUSUARIO_P", Value = usuario.INT_IDUSUARIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_PERM", Value = usuario.BOOL_ESTATUS_LOGICO_USUARIO });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch
            {
                throw;
            }

        }
}
}

