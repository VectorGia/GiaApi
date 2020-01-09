﻿using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace AppGia.Controllers
{
    public class UsuarioDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        Usuario usuario = new Usuario();
        

        char cod = '"';    
  
        public UsuarioDataAccessLayer()
        {
        con = conex.ConnexionDB();
        }
        /// <summary>
        /// Obtiene los usuarios que se ecuentran dentro del servicio de activeDirectory 
        /// y los regresa en una lista para su consumo
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Usuario> GetAllUsuarios()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_USUARIO" + cod + "";
            try
            {
                List<Usuario> lstusuario = new List<Usuario>();
                
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.INT_IDUSUARIO_P = Convert.ToInt32(rdr["INT_IDUSUARIO_P"]);
                        usuario.STR_NOMBRE_USUARIO = rdr["STR_NOMBRE_USUARIO"].ToString().Trim();
                        usuario.STR_USERNAME_USUARIO = rdr["STR_USERNAME_USUARIO"].ToString().Trim();
                        usuario.STR_PUESTO = rdr["STR_PUESTO"].ToString().Trim();
                        usuario.STR_EMAIL_USUARIO = rdr["STR_EMAIL_USUARIO"].ToString().Trim();
                        usuario.STR_PASSWORD_USUARIO = rdr["STR_PASSWORD_USUARIO"].ToString().Trim();

                        lstusuario.Add(usuario);
                    }
                con.Close();
            
            return lstusuario;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public Usuario GetUsuario(string id)
        {
            string cadena = "SELECT * FROM" + cod + "TAB_USUARIO" + cod + "WHERE" + cod + "INT_IDUSUARIO_P" + cod + "=" + id;
            try
            {
                Usuario usuario = new Usuario();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        
                        usuario.INT_IDUSUARIO_P = Convert.ToInt32(rdr["INT_IDUSUARIO_P"]);
                        usuario.STR_NOMBRE_USUARIO = rdr["STR_NOMBRE_USUARIO"].ToString().Trim();
                        usuario.STR_USERNAME_USUARIO = rdr["STR_USERNAME_USUARIO"].ToString().Trim();
                        usuario.STR_PUESTO = rdr["STR_PUESTO"].ToString().Trim();
                        usuario.STR_EMAIL_USUARIO = rdr["STR_EMAIL_USUARIO"].ToString().Trim();
                        usuario.STR_PASSWORD_USUARIO = rdr["STR_PASSWORD_USUARIO"].ToString().Trim();


                    }
                    con.Close();
                }
                return usuario;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public int InsertaUsuarios()
        {
            Usuario usuario = new Usuario();
            List<Usuario> lstUsu = new List<Usuario>();
            UsersADController prueba = new UsersADController();
            lstUsu = prueba.Get();

            int numeroUsuarios = lstUsu.Count();


            for (int i = 0; i < numeroUsuarios; i++)
            {

                if (lstUsu[i].INT_IDUSUARIO_P != 0)
                {
                    usuario.INT_IDUSUARIO_P = lstUsu[i].INT_IDUSUARIO_P;
                }

                if (lstUsu[i].STR_USERNAME_USUARIO != null)
                {
                    usuario.STR_USERNAME_USUARIO = lstUsu[i].STR_USERNAME_USUARIO;
                }
                else
                {
                    usuario.STR_USERNAME_USUARIO = "sin username";
                }

                if (lstUsu[i].STR_PASSWORD_USUARIO != null)
                {
                    usuario.STR_PASSWORD_USUARIO = lstUsu[i].STR_PASSWORD_USUARIO;
                }
                else
                {
                    usuario.STR_PASSWORD_USUARIO = "sin contraseña";
                }

                if (lstUsu[i].STR_EMAIL_USUARIO != null)
                {
                    usuario.STR_EMAIL_USUARIO = lstUsu[i].STR_EMAIL_USUARIO;
                }
                else
                {
                    usuario.STR_EMAIL_USUARIO = "sin Correo";
                }


                usuario.BOOL_ESTATUS_LOGICO_USUARIO = true;


                if (lstUsu[i].STR_PUESTO != null)
                {
                    usuario.STR_PUESTO = lstUsu[i].STR_PUESTO;
                }
                else
                {
                    usuario.STR_PUESTO = "sin puesto";
                }


                if (lstUsu[i].FEC_MODIF_USUARIO != null)
                {
                    usuario.FEC_MODIF_USUARIO = lstUsu[i].FEC_MODIF_USUARIO;
                }
                else
                {
                    usuario.FEC_MODIF_USUARIO = DateTime.Now;
                }

                if (lstUsu[i].STR_NOMBRE_USUARIO != null)
                {
                    usuario.STR_NOMBRE_USUARIO = lstUsu[i].STR_NOMBRE_USUARIO;
                }
                else
                {
                    usuario.STR_NOMBRE_USUARIO = "sin nombre usuario";
                }

                bool existe = validacionUsuario(usuario);

                if (existe == false)
                {
                    addUsuario(usuario);
                }
                else
                {
                    continue;
                }

            }
            return 1;
        }
        public int addUsuario(Usuario usuario)
        {
            string add = "INSERT INTO " + cod + "TAB_USUARIO" + cod
                        + "(" + cod + "STR_USERNAME_USUARIO" + cod + ","
                        + cod + "STR_PASSWORD_USUARIO" + cod + ","
                        + cod + "STR_EMAIL_USUARIO" + cod + ","
                        + cod + "BOOL_ESTATUS_LOGICO_USUARIO" + cod + ","
                        + cod + "STR_PUESTO" + cod + ","
                        + cod + "STR_NOMBRE_USUARIO" + cod + ","
                        + cod + "FEC_MODIF_USUARIO" + cod + ")"
                        + " VALUES ( @STR_USERNAME_USUARIO" + ","
                        + "@STR_PASSWORD_USUARIO" + ","
                        + "@STR_EMAIL_USUARIO" + ","
                        + "@BOOL_ESTATUS_LOGICO_USUARIO" + ","
                        + "@STR_PUESTO" + ","
                        + "@STR_NOMBRE_USUARIO" + ","
                        + "@FEC_MODIF_USUARIO"
                        + ")";

            try
            {
               

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_USUARIO", Value = usuario.STR_NOMBRE_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_USERNAME_USUARIO", Value = usuario.STR_USERNAME_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_EMAIL_USUARIO", Value = usuario.STR_EMAIL_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_PASSWORD_USUARIO", Value = usuario.STR_PASSWORD_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_USUARIO", Value = usuario.BOOL_ESTATUS_LOGICO_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_PUESTO", Value = usuario.STR_PUESTO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_USUARIO", Value = DateTime.Now });
                  
                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;

                }

            }
            catch (Exception ex)
            {
              
                con.Close();
                string error = ex.Message;
                throw;
            }
        }

        
        /// <summary>
        /// Metodo que regresa una variable booleana para determinar si un un Usuario existe
        /// y asi determinar si se agrega o no se agrega a la base de datos
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// 
        public bool validacionUsuario(Usuario usuario)
        {

            string consulta = "SELECT " + 1 + " from " + cod + "TAB_USUARIO" + cod + " WHERE " + cod + "STR_USERNAME_USUARIO" + cod + " LIKE " + "'%" + usuario.STR_USERNAME_USUARIO.Trim() + "%'";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    bool esRepetida = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                    return esRepetida;

                }
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }

        }
        public DataTable Dat_getObtieneUsuarios(Usuario usuario)
        {

            string select = "SELECT " + cod + "STR_USERNAME_USUARIO" + cod + " from" + cod + "TAB_USUARIO" + cod;

            try
            {
              
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(select, con);
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(select, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {                
                con.Close();
                throw ex;
            }
        }
        public List<Usuario> List_obtieneUsuarios(Usuario usuario)
        {
            List<Usuario> lstUsuario = new List<Usuario>();
            DataTable dt = new DataTable();
            dt = Dat_getObtieneUsuarios(usuario);

            foreach (DataRow r in dt.Rows)
            {
                Usuario ent = new Usuario();
                ent.userName = Convert.ToString(r["STR_USERNAME_USUARIO".Trim()]);
                lstUsuario.Add(ent);
            }
            return lstUsuario;
        }
    }

}
