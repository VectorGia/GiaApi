using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class UsuarioDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
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
                        usuario.BOOL_ESTATUS_LOGICO_USUARIO = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_USUARIO"]);
                        usuario.SRT_USERNAME_USUARIO = rdr["SRT_USERNAME_USUARIO"].ToString();
                        usuario.STR_DISPLAYNAME_USUARIO = rdr["STR_DISPLAYNAME_USUARIO"].ToString();
                        usuario.SRT_PUESTO = Convert.ToBoolean(rdr["SRT_PUESTO"]);

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
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_USUARIO", usuario.BOOL_ESTATUS_LOGICO_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_USERNAME_USUARIO", usuario.SRT_USERNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_DISPLAYNAME_USUARIO", usuario.STR_DISPLAYNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_PUESTO", usuario.SRT_PUESTO);

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
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("spUpdateCentroCostos", con);

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_USUARIO", usuario.STR_NOMBRE_USUARIO);
                    cmd.Parameters.AddWithValue("@STR_EMAIL_USUARIO", usuario.STR_EMAIL_USUARIO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_USUARIO", usuario.BOOL_ESTATUS_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_USERNAME_USUARIO", usuario.SRT_USERNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_DISPLAYNAME_USUARIO", usuario.SRT_DISPLAYNAME_USUARIO);
                    cmd.Parameters.AddWithValue("@SRT_PUESTO", usuario.SRT_PUESTO);

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
