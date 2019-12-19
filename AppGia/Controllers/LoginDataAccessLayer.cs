using AppGia.Models;
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
    public class LoginDataAccessLayer
    {

        public Response Login(Login lg)
        {
            string dominio = "infogia";
            string path = "LDAP://ServerOmnisys/CN=users, DC=Infogia, DC=local";
            string domainAndUsername = dominio + @"\" + lg.UserName;
            DirectoryEntry entry = new DirectoryEntry(path,
            domainAndUsername, lg.Password);
            try
            {
                DirectorySearcher dir = new DirectorySearcher(entry);
                dir.ToString();
                SearchResult result = dir.FindOne();
                return new Response { MESSAGE = true };

            }
            catch (DirectoryServicesCOMException cex)
            {
                return new Response { MESSAGE = false };
            }

            catch (Exception ex)
            {
                return new Response { MESSAGE = false };
            }
        }

        public bool validacionLoginUsuario(Relacion relacion, Login lg)
        {
            string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
            char cod = '"';




            string consulta = " select " + 1 + " from " + "(" + " select " + cod + "TAB_RELACIONES" + cod + "." + cod + "INT_IDGRUPO_F" + cod + "," + cod + "TAB_RELACIONES" + cod + "."
            + cod + "INT_IDUSUARIO_F" + cod + "," + cod + "TAB_USUARIO" + cod + "." + cod + "STR_NOMBRE_USUARIO" + cod + " from " + cod + "TAB_RELACIONES" + cod + " inner " + "  " + " join "
            + cod + "TAB_USUARIO" + cod + " on " + cod + "TAB_RELACIONES" + cod + "." + cod + "INT_IDUSUARIO_F" + cod + "=" + cod + "TAB_USUARIO"
            + cod + "." + cod + "INT_IDUSUARIO_P" + cod + " where " + cod + "TAB_USUARIO" + cod + "." + cod + "STR_USERNAME_USUARIO" + cod + " = " + "'" + lg.UserName + "'" + " and " + cod + "TAB_RELACIONES" + cod + "." + cod + "INT_IDGRUPO_F" + cod + " != " + 1 + ")" + "usuario";




            // string consulta = " SELECT " + 1 + " from " + cod + "TAB_RELACIONES" + cod + " WHERE " + cod + "INT_IDUSUARIO_F" + cod + " = " + relacion.INT_IDUSUARIO_F + " and " + cod + "INT_IDGRUPO_F" + cod + " = " + relacion.INT_IDGRUPO_F;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    bool existe = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                    return existe;

                }
            }
            catch (Exception ex)
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                    con.Close();
                throw ex;
            }
        }

    }
}
