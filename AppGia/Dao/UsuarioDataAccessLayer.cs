using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppGia.Controllers;
using AppGia.Models;
using Npgsql;
using NpgsqlTypes;

namespace AppGia.Dao
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
            string cadena = "select * from tab_usuario where  estatus = true order by fech_modificacion  desc"; 
            try
            {
                List<Usuario> lstusuario = new List<Usuario>();
                
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Usuario usuario = new Usuario();
                        usuario.id = Convert.ToInt32(rdr["id"]);
                        usuario.nombre = rdr["nombre"].ToString().Trim();
                        usuario.user_name = rdr["user_name"].ToString().Trim();
                        usuario.puesto = rdr["puesto"].ToString().Trim();
                        usuario.email= rdr["email"].ToString().Trim();
                        usuario.password = rdr["password"].ToString().Trim();

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
            finally
            {
                con.Close();
            }
        }
        public Usuario GetUsuario(string id)
        {
            string cadena = "select * from tab_usuario where id = " + id;
            try
            {
                Usuario usuario = new Usuario();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        usuario.id = Convert.ToInt32(rdr["id"]);
                        usuario.nombre = rdr["nombre"].ToString().Trim();
                        usuario.user_name= rdr["user_name"].ToString().Trim();
                        usuario.puesto = rdr["puesto"].ToString().Trim();
                        usuario.email = rdr["email"].ToString().Trim();
                        /*usuario.password = rdr["password"].ToString().Trim();*/


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
            finally
            {
                con.Close();
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

                if (lstUsu[i].id != 0)
                {
                    usuario.id = lstUsu[i].id;
                }

                if (lstUsu[i].user_name_interno != null)
                {
                    usuario.user_name_interno = lstUsu[i].user_name_interno;
                }
                else
                {
                    usuario.user_name_interno = "sin username";
                }

                if (lstUsu[i].password != null)
                {
                    usuario.password= lstUsu[i].password;
                }
                else
                {
                    usuario.password = "sin contraseña";
                }

                if (lstUsu[i].email != null)
                {
                    usuario.email = lstUsu[i].email;
                }
                else
                {
                    usuario.email = "sin Correo";
                }


                usuario.estatus = true;


                if (lstUsu[i].puesto != null)
                {
                    usuario.puesto = lstUsu[i].puesto;
                }
                else
                {
                    usuario.puesto = "sin puesto";
                }


                if (lstUsu[i].fech_modificacion != null)
                {
                    usuario.fech_modificacion = lstUsu[i].fech_modificacion;
                }
                else
                {
                    usuario.fech_modificacion = DateTime.Now;
                }

                if (lstUsu[i].nombre != null)
                {
                    usuario.nombre = lstUsu[i].nombre;
                }
                else
                {
                    usuario.nombre = "sin nombre usuario";
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
            string add = " insert into tab_usuario " +
                       "("  + "user_name" + ","
                        /*+ "password" + ","*/
                        + "email" + ","
                        + "estatus" +  ","
                        + "puesto" + ","
                        + "nombre" +  ","
                        + "fech_modificacion" + ")"
                        + " values ( @user_name" + ","
                        /*+ "@password" + ","*/
                        + "@email" + ","
                        + "@estatus" + ","
                        + "@puesto" + ","
                        + "@nombre" + ","
                        + "@fech_modificacion"
                        + ")";

            try
            {
               

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@nombre", Value = usuario.nombre.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@user_name", Value = usuario.user_name.Trim() });
                   // cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@user_name_interno", Value = usuario.user_name_interno.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@email", Value = usuario.email.Trim() });
                   // cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@password", Value = usuario.password.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Boolean, ParameterName = "@estatus", Value = usuario.estatus });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@puesto", Value = usuario.puesto.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Date, ParameterName = "@fech_modificacion", Value = DateTime.Now });
                  
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
            finally
            {
                con.Close();
            }
        }

        public int updateUsuario(string id, Usuario usuario)
        {
            string update = "update tab_usuario "  +
                " set "
                /*+ "user_name_interno=@user_name_interno,"*/
                /*+ "password=@password,"*/
                + "email=@email,"
                + "puesto=@puesto,"
                + "nombre=@nombre,"
                + "fech_modificacion=@fech_modificacion"
                + " where id = " + id;

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@nombre", Value = usuario.nombre.Trim() });
                //cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@user_name_interno", Value = usuario.user_name_interno.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@user_name", Value = usuario.user_name.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@email", Value = usuario.email.Trim() });
                // cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@password", Value = usuario.password.Trim() });
                //cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Boolean, ParameterName = "@estatus", Value = usuario.estatus });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Text, ParameterName = "@puesto", Value = usuario.puesto.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Date, ParameterName = "@fech_modificacion", Value = DateTime.Now });
                con.Open();
                int cantFilas = cmd.ExecuteNonQuery();
                con.Close();
                return cantFilas;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
        }

        public int DeleteUser(string id)
        {
            
            string add = "update tab_usuario "  +
                " set estatus=false " + 
                " where id = " + id;
            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                int cantFilas = cmd.ExecuteNonQuery();
                con.Close();
                return cantFilas;
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

            string consulta = "select 1" 
                + " from " 
                + "tab_usuario where "  
                + "user_name = '" 
                + usuario.user_name_interno.Trim() + "'";

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
            finally
            {
                con.Close();
            }

        }
        public DataTable Dat_getObtieneUsuarios(Usuario usuario)
        {

            string select = "select user_name from tab_usuario" ;

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
                ent.user_name = Convert.ToString(r["user_name".Trim()]);
                lstUsuario.Add(ent);
            }
            return lstUsuario;
        }
    }

}
