using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class ConfigCorreoDataAccessLayer

    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public ConfigCorreoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public List<ConfigCorreo> GetAllConfigCorreo()
        {


            string cadena = "SELECT " + cod + "TEXT_FROM" + cod + "," + cod + "TEXT_PASSWORD" + cod + ","
                              + cod + "INT_PORT" + cod + "," + cod + "INT_ID_CORREO" + cod + "," + cod + "TEXT_HOST" + cod + " FROM " + cod + "TAB_CONFIG_CORREO" + cod;

            try
            {
                List<ConfigCorreo> listaConfigCorreo = new List<ConfigCorreo>();
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena.Trim(), con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        ConfigCorreo configCorreo = new ConfigCorreo();
                        configCorreo.INT_ID_CORREO = Convert.ToInt32(rdr["INT_ID_CORREO"]);
                        configCorreo.TEXT_FROM = rdr["TEXT_FROM"].ToString().Trim();
                        configCorreo.TEXT_PASSWORD = rdr["TEXT_PASSWORD"].ToString().Trim();
                        configCorreo.INT_PORT = Convert.ToInt32(rdr["INT_PORT"]);
                        configCorreo.TEXT_HOST = rdr["TEXT_HOST"].ToString().Trim();


                        listaConfigCorreo.Add(configCorreo);
                    }
                    con.Close();
                }
                return listaConfigCorreo;
            }
            catch (Exception ex)
            {
                con.Close();
                
                throw ex; 
            }
        }
        public List<Usuario> GetDestinatariosCorreo() {


            string cadena = "SELECT "
                            + cod + "STR_EMAIL_USUARIO" + cod
                            + " FROM "
                            + cod + "TAB_USUARIO" + cod
                            + " WHERE " + cod + "STR_EMAIL_USUARIO" + cod + "!='sin Correo'";

            try
            {
                List<Usuario> listaUsuarioCorreo = new List<Usuario>();
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena.Trim(), con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        Usuario configUsuarioCorreo = new Usuario();
                        configUsuarioCorreo.email = rdr["email"].ToString().Trim();

                        


                        listaUsuarioCorreo.Add(configUsuarioCorreo);
                    }
                    con.Close();
                }
                return listaUsuarioCorreo;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
    }
}