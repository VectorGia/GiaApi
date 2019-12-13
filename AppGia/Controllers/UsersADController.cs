using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppGia.Models;
using System.DirectoryServices;
using Npgsql;

namespace AppGia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersADController : ControllerBase
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        // GET: api/UsersAD
        [HttpGet]
        public List<Usuario> Get()
        {
            List<Usuario> rst = new List<Usuario>();

            string path = "LDAP://ServerOmnisys.local/CN=users, DC=Infogia, DC=local", us = "Administrador", pass = "Omnisys1958";
            DirectoryEntry adSearchRoot = new DirectoryEntry(path, us, pass);
            DirectorySearcher adSearcher = new DirectorySearcher(adSearchRoot);

            adSearcher.Filter = "(&(objectClass=user)(objectCategory=person))";
            SearchResult result;
            SearchResultCollection iResult = adSearcher.FindAll();

            Usuario item;
            if (iResult != null)
            {
                for (int counter = 3; counter < iResult.Count; counter++)
                {
                    result = iResult[counter];
                    if (result.Properties.Contains("samaccountname"))
                    {
                        item = new Usuario();

                        item.userName = (String)result.Properties["samaccountname"][0];

                        if (result.Properties.Contains("displayname"))
                        {
                            item.displayName = (String)result.Properties["displayname"][0];
                        }

                        rst.Add(item); /*Ya se tiene los usuarios del Active Directory*/
                    }
                }
            }
            else
            {
                // PONER SI VIENE NULLO
            }

            adSearcher.Dispose();
            adSearchRoot.Dispose();
            return rst;            
        }

        // GET: api/UsersAD/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UsersAD
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/UsersAD/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #region InsertarAD

        public void InsertaUsuarios()
        {
            List<Usuario> lstUsu = new List<Usuario>();
            lstUsu = Get();
            int numeroUsuarios = lstUsu.Count();
            Usuario usuario = new Usuario();

            for (int i = 0; i < numeroUsuarios; i++)
            {
                if (lstUsu[i].STR_DISPLAYNAME_USUARIO != null)                
                {
                    usuario.STR_DISPLAYNAME_USUARIO = lstUsu[i].STR_DISPLAYNAME_USUARIO;
                }
                else
                {
                    usuario.STR_DISPLAYNAME_USUARIO = "Sin displayName";
                }
                if (lstUsu[i].STR_EMAIL_USUARIO != null)
                {
                    usuario.STR_EMAIL_USUARIO = lstUsu[i].STR_EMAIL_USUARIO;
                }
                else
                {
                    usuario.STR_EMAIL_USUARIO = "sinCorreo";
                }
                usuario.BOOL_ESTATUS_LOGICO_USUARIO = true;//lstUsu[i].BOOL_ESTATUS_LOGICO_USUARIO;

                if (lstUsu[i].INT_IDROL != 0)
                {
                    usuario.INT_IDROL = lstUsu[i].INT_IDROL;
                }
                else
                {
                    usuario.INT_IDROL = 1;
                }

                if (lstUsu[i].FEC_MODIF != null)
                {
                    usuario.FEC_MODIF = lstUsu[i].FEC_MODIF;
                }
                else
                {
                    usuario.FEC_MODIF = DateTime.Now;
                }
                if (lstUsu[i].INT_IDPANTALLA_F != 0)
                {
                    usuario.INT_IDPANTALLA_F = lstUsu[i].INT_IDPANTALLA_F;
                }
                else
                {
                    usuario.INT_IDPANTALLA_F = 1;
                }

                if (lstUsu[i].INT_IDPANTALLA_F != 0)
                {
                    usuario.INT_IDPANTALLA_F = lstUsu[i].INT_IDPANTALLA_F;
                }
                else
                {
                    usuario.INT_IDPANTALLA_F = 1;
                }
                if (lstUsu[i].INT_IDPROYECTO_F != 0)
                {
                    usuario.INT_IDPROYECTO_F = lstUsu[i].INT_IDPROYECTO_F;
                }
                else
                {
                    usuario.INT_IDPROYECTO_F = 1;
                }
                if (lstUsu[i].INT_IDCOMPANIA_F != 0)
                {
                    usuario.INT_IDCOMPANIA_F = lstUsu[i].INT_IDCOMPANIA_F;
                }
                else
                {
                    usuario.INT_IDCOMPANIA_F = 21;
                }
                if (lstUsu[i].STR_IDCENTROCOSTO_F != null)
                {
                    usuario.STR_IDCENTROCOSTO_F = lstUsu[i].STR_IDCENTROCOSTO_F;
                }
                else
                {
                    usuario.STR_IDCENTROCOSTO_F = "1";
                }
                if (lstUsu[i].INT_IDGRUPO_F != 0)
                {
                    usuario.INT_IDGRUPO_F = lstUsu[i].INT_IDGRUPO_F;
                }
                else
                {
                    usuario.INT_IDGRUPO_F = 8;
                }
                if (lstUsu[i].INT_IDUSUARIO_P != 0)
                {
                    usuario.INT_IDUSUARIO_P = lstUsu[i].INT_IDUSUARIO_P;
                }
                if (lstUsu[i].userName != null)
                {
                    usuario.STR_USERNAME_USUARIO = lstUsu[i].userName;
                }
                else
                {
                    usuario.STR_USERNAME_USUARIO = "SINuser";
                }
                if (lstUsu[i].STR_PUESTO != null)
                {
                    usuario.STR_PUESTO = lstUsu[i].STR_PUESTO;
                }
                else
                {
                    usuario.STR_PUESTO = "sin nombre";
                }
                addUsuario(usuario);
            }
        }

        public int addUsuario(Usuario usuario)
        {
            string add = "INSERT INTO " + cod + "TAB_USUARIO" + cod
                        + "(" + cod + "STR_DISPLAYNAME_USUARIO" + cod + ","
                        + cod + "STR_EMAIL_USUARIO" + cod + ","
                        + cod + "BOOL_ESTATUS_LOGICO_USUARIO" + cod + ","
                        + cod + "INT_IDROL" + cod + ","
                        + cod + "INT_IDPANTALLA_F" + cod + ","
                        + cod + "INT_IDPROYECTO_F" + cod + ","
                        + cod + "INT_IDCOMPANIA_F" + cod + ","
                        + cod + "STR_IDCENTROCOSTO_F" + cod + ","
                        + cod + "INT_IDGRUPO_F" + cod + ","
                        + cod + "STR_USERNAME_USUARIO" + cod + ","
                        + cod + "STR_PUESTO" + cod + ","
                        + cod + "FEC_MODIF_USUARIO" + cod + ")"
                        + " VALUES ( @STR_DISPLAYNAME_USUARIO" + ","
                        + "@STR_EMAIL_USUARIO" + ","
                        + "@BOOL_ESTATUS_LOGICO_USUARIO" + ","
                        + "@INT_IDROL" + ","
                        + "@INT_IDPANTALLA_F" + ","
                        + "@INT_IDPROYECTO_F" + ","
                        + "@INT_IDCOMPANIA_F" + ","
                        + "@STR_IDCENTROCOSTO_F" + ","
                        + "@INT_IDGRUPO_F" + ","
                        + "@STR_USERNAME_USUARIO" + ","
                        + "@STR_PUESTO" + ","
                        + "@FEC_MODIF_USUARIO"
                        + ")";

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_DISPLAYNAME_USUARIO", Value = usuario.STR_DISPLAYNAME_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_EMAIL_USUARIO", Value = usuario.STR_EMAIL_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_USUARIO", Value = usuario.BOOL_ESTATUS_LOGICO_USUARIO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL", Value = usuario.INT_IDROL });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_F", Value = usuario.INT_IDPANTALLA_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPROYECTO_F", Value = usuario.INT_IDPROYECTO_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDCOMPANIA_F", Value = usuario.INT_IDCOMPANIA_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_IDCENTROCOSTO_F", Value = usuario.STR_IDCENTROCOSTO_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_F", Value = usuario.INT_IDGRUPO_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_USERNAME_USUARIO", Value = usuario.STR_USERNAME_USUARIO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_PUESTO", Value = usuario.STR_PUESTO.Trim() });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_USUARIO", Value = usuario.FEC_MODIF_USUARIO });

                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
        #endregion
    }
}
