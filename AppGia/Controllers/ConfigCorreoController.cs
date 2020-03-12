using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigCorreoController : ControllerBase
    {
        ConfigCorreoDataAccessLayer objConfigCorreo = new ConfigCorreoDataAccessLayer();
        // GET: api/ConfigCorreo
        [HttpGet]
        public IEnumerable<ConfigCorreo> GetAllConfigCorreo()
        {
            return objConfigCorreo.GetAllConfigCorreo();
        }

        // GET: api/ConfigCorreo/5
        [HttpGet("{id}", Name = "GetConfigCorreo")]
        public string GetConfigCorreo(int id)
        {
            return "value";
        }

        // POST: api/ConfigCorreo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ConfigCorreo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public void EnviarCorreo(string cuerpoMensaje, string tituloMensaje)
        {

            string listaDestinatarios = ListaCorreosDestinatarios().TrimEnd(',');
            List<ConfigCorreo> listaConfiguracionCorreo = new List<ConfigCorreo>();
            listaConfiguracionCorreo = objConfigCorreo.GetAllConfigCorreo();

            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
            mmsg.To.Add(listaDestinatarios.ToString());
            mmsg.Subject = tituloMensaje;//"Correo ejemplo GIA";
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
            mmsg.Body = cuerpoMensaje;//"Prueba de correo GIA";
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = false;
            mmsg.From = new System.Net.Mail.MailAddress(listaConfiguracionCorreo[0].remitente);

            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
            cliente.Credentials = new System.Net.NetworkCredential(listaConfiguracionCorreo[0].remitente, listaConfiguracionCorreo[0].password);
            cliente.Port = listaConfiguracionCorreo[0].puerto;
            cliente.EnableSsl = true;
            cliente.Host = listaConfiguracionCorreo[0].host;

            string output = null;
            try
            {
                cliente.Send(mmsg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }

        }

        public string ListaCorreosDestinatarios()
        {

            string correos = string.Empty;
            List<Usuario> lista = new List<Usuario>();
            lista = objConfigCorreo.GetDestinatariosCorreo();
            foreach (Usuario usuario in lista)
            {
                correos += usuario.email.ToString() + ",";
            }
            return correos;

        }
    }
}
