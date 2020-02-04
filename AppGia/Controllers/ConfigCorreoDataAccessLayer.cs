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


            string cadena = "SELECT  remitente ,"
                            + " password,"
                            + " puerto ,"
                            + " id,"
                            + " host "
                            + " FROM CONFIG_CORREO ";

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
                        configCorreo.id = Convert.ToInt32(rdr["id"]);
                        configCorreo.remitente = rdr["remitente"].ToString().Trim();
                        configCorreo.password = rdr["password"].ToString().Trim();
                        configCorreo.puerto = Convert.ToInt32(rdr["puerto"]);
                        configCorreo.host = rdr["host"].ToString().Trim();


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
        public List<Usuario> GetDestinatariosCorreo()
        {


            string cadena = "  SELECT "
                            + " email"
                            + " FROM "
                            + " USUARIO"
                            + " WHERE " + cod + "email" + cod + "!='sin Correo'";

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