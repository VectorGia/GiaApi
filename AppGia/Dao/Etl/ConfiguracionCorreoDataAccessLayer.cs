﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using AppGia.Models.Etl;
using Npgsql;

namespace AppGia.Dao.Etl
{
    public class ConfiguracionCorreoDataAccessLayer

    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
       

        public ConfiguracionCorreoDataAccessLayer()
        {
             con = conex.ConnexionDB();
        }

        public void EnviarCorreo(string cuerpoMensaje, string tituloMensaje)
        {

            string listaDestinatarios = ListaCorreosDestinatarios().TrimEnd(',');
            List<ConfigCorreo> listaConfiguracionCorreo = new List<ConfigCorreo>();
            listaConfiguracionCorreo = GetAllConfigCorreo();

            MailMessage mmsg = new MailMessage();
            mmsg.To.Add(listaDestinatarios.ToString());
            mmsg.Subject = tituloMensaje;//"Correo ejemplo GIA";
            mmsg.SubjectEncoding = Encoding.UTF8;
            mmsg.Body = cuerpoMensaje;//"Prueba de correo GIA";
            mmsg.BodyEncoding = Encoding.UTF8;
            mmsg.IsBodyHtml = false;
            mmsg.From = new MailAddress(listaConfiguracionCorreo[0].remitente);

            SmtpClient cliente = new SmtpClient();

            cliente.Host = listaConfiguracionCorreo[0].host;
            cliente.Port = listaConfiguracionCorreo[0].puerto;
            cliente.EnableSsl = true;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.UseDefaultCredentials = false;
            cliente.Credentials = new NetworkCredential(listaConfiguracionCorreo[0].remitente, listaConfiguracionCorreo[0].password);
           
       


            string output = null;
            try
            {
                cliente.Send(mmsg);
            }
            catch (SmtpException ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }

        }


        public string ListaCorreosDestinatarios()
        {

            string correos = string.Empty;
            List<Usuario> lista = new List<Usuario>();
            lista = GetDestinatariosCorreo();
            foreach (Usuario usuario in lista)
            {
                correos += usuario.email.ToString() + ",";
            }
            return correos;

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
                + " WHERE  email != 'sin Correo' ";

            try
            {
                List<Usuario> listaUsuarioCorreo = new List<Usuario>();
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
