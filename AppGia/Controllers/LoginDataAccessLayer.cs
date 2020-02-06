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
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        char cod = '"';
        public LoginDataAccessLayer()
        {
         con = conex.ConnexionDB();
        }

        public bool validacionLoginUsuario(Relacion relacion, Login lg)
        {
            relacion.INT_IDGRUPO_P = 9;
            string consulta = " select " + 1 + " from " + "(" + " select " + cod + "TAB_RELACION_USUARIO" + cod + "." + cod + "INT_IDGRUPO_P" + cod + "," + cod + "TAB_RELACION_USUARIO" + cod + "."
             + cod + "INT_IDUSUARIO_P" + cod + "," + cod + "TAB_USUARIO" + cod + "." + cod + "STR_NOMBRE_USUARIO" + cod + " from " + cod + "TAB_RELACION_USUARIO" + cod + " inner " + "  " + " join "
             + cod + "TAB_USUARIO" + cod + " on " + cod + "TAB_RELACION_USUARIO" + cod + "." + cod + "INT_IDUSUARIO_P" + cod + "=" + cod + "TAB_USUARIO"
             + cod + "." + cod + "INT_IDUSUARIO_P" + cod + " where " + cod + "TAB_USUARIO" + cod + "." + cod + "STR_USERNAME_USUARIO" + cod + " = " + "'" + lg.UserName + "'" + " and " 
             + cod + "TAB_RELACION_USUARIO" + cod + "." + cod + "INT_IDGRUPO_P" + cod + " = " + relacion.INT_IDGRUPO_P + " and " +  cod + "BOOL_ESTATUS_LOGICO_USUARIO" + cod + " = "  + true +  ")" + "usuario";

            // string consulta = " SELECT " + 1 + " from " + cod + "TAB_RELACIONES" + cod + " WHERE " + cod + "INT_IDUSUARIO_F" + cod + " = " + relacion.INT_IDUSUARIO_F + " and " + cod + "INT_IDGRUPO_F" + cod + " = " + relacion.INT_IDGRUPO_F;
            try
            {
              
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
      
             con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
