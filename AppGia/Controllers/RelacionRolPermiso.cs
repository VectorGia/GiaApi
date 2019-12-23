using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class RelacionRolPermiso
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';
        public RelacionRolPermiso()
        {
            con = conex.ConnexionDB();
        }

        public int addRol_permisos(RelacionRol relacionRol)
        {
            string add = "INSERT INTO " + cod + "TAB_ROL_PERMISOS" + cod
                        + "(" + cod + "INT_IDPERMISO_F" + cod + ","
                        + cod + "BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + cod + ","
                        + cod + "FEC_MODIF_RELA_ROL_PERMISO" + cod + ","
                        + cod + "INT_IDROL_F" + cod
                        + " VALUES ( @INT_IDPERMISO_F" + ","
                        + "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + ","
                        + "@FEC_MODIF_RELA_ROL_PERMISO" + ","
                        + "@INT_IDROL_F"
                        + ")";
            try
            {
              
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_F", Value = relacionRol.INT_IDPERMISO_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO", Value = relacionRol.BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELA_ROL_PERMISO", Value = relacionRol.FEC_MODIF_RELA_ROL_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_F", Value = relacionRol.INT_IDROL_F });
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

        public int UpdateGrupo(RelacionRol relacionRol)
        {
            string add = "UPDATE " + cod + "TAB_ROL_PERMISOS" + cod + " SET "
                + cod + "INT_IDPERMISO_F" + cod + "= " + "@INT_IDPERMISO_F" + ","
                + cod + "BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + ","
                + cod + "FEC_MODIF_RELA_ROL_PERMISO" + cod + "= " + "@FEC_MODIF_RELA_ROL_PERMISO" + ","
                + cod + "INT_IDROL_F" + cod + "= " + "@INT_IDROL_F"
                + " WHERE " + cod + "INT_IDRELACION_P" + cod + " = " + "@INT_IDRELACION_P";
            try
            {
 
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_F", Value = relacionRol.INT_IDPERMISO_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO", Value = relacionRol.BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELA_ROL_PERMISO", Value = relacionRol.FEC_MODIF_RELA_ROL_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@INT_IDROL_F", Value = relacionRol.INT_IDROL_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_P", Value = relacionRol.INT_IDRELACION_P });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                con.Close();
                throw;
            }
        }

        public int Update_ESTATUS_LOGICO(RelacionRol relacionRol)
        {
            string add = "UPDATE " + cod + "TAB_ROL_PERMISOS" + cod + " SET "
                + cod + "BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + ","
                + cod + "FEC_MODIF_RELA_ROL_PERMISO" + cod + "= " + "@FEC_MODIF_RELA_ROL_PERMISO"
                + " WHERE " + cod + "INT_IDRELACION_P" + cod + " = " + "@INT_IDRELACION_P";
            try
            {
              
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_F", Value = relacionRol.INT_IDPERMISO_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO", Value = relacionRol.BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELA_ROL_PERMISO", Value = relacionRol.FEC_MODIF_RELA_ROL_PERMISO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@INT_IDROL_F", Value = relacionRol.INT_IDROL_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_P", Value = relacionRol.INT_IDRELACION_P });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;
            }
        }

    }
}
