using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using AppGia.Models;

namespace AppGia.Controllers
{
    public class RelacionesDataAccessLayer
   {
//        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
//        char cod = '"';
//        public RelacionesDataAccessLayer()
//        {
//            //Constructor
//        }

//        public int addRol_permisos(ROL_PERMISOS rol_permisos)
//        {
//            string add = "INSERT INTO " + cod + "TAB_ROL_PERMISOS" + cod
//                        + "(" + cod + "INT_IDPERMISO_F" + cod + ","
//                        + cod + "BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + cod + ","
//                        + cod + "FEC_MODIF_RELA_ROL_PERMISO" + cod + ","
//                        + cod + "INT_IDROL_F" + cod
//                        + " VALUES ( @INT_IDPERMISO_F" + ","
//                        + "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + ","
//                        + "@FEC_MODIF_RELA_ROL_PERMISO" + ","
//                        + "@INT_IDROL_F"
//                        + ")";
//            try
//            {
//                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
//                {
//                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_F", Value = rol_permisos.INT_IDPERMISO_F });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO", Value = rol_permisos.BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELA_ROL_PERMISO", Value = rol_permisos.FEC_MODIF_RELA_ROL_PERMISO });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_F", Value = rol_permisos.INT_IDROL_F });
//                    con.Open();
//                    int cantFilAfec = cmd.ExecuteNonQuery();
//                    con.Close();
//                    return cantFilAfec;
//                }
//            }
//            catch (Exception ex)
//            {
//                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString)) ;
//                //con.close();
//                string error = ex.Message;
//                throw;
//            }
//        }

//        public int UpdateGrupo(ROL_PERMISOS rol_permisos)
//        {
//            string add = "UPDATE " + cod + "TAB_ROL_PERMISOS" + cod + " SET "
//                + cod + "INT_IDPERMISO_F" + cod + "= " + "@INT_IDPERMISO_F" + ","
//                + cod + "BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + ","
//                + cod + "FEC_MODIF_RELA_ROL_PERMISO" + cod + "= " + "@FEC_MODIF_RELA_ROL_PERMISO" + ","
//                + cod + "INT_IDROL_F" + cod + "= " + "@INT_IDROL_F"
//                + " WHERE " + cod + "INT_IDRELACION_P" + cod + " = " + "@INT_IDRELACION_P";
//            try
//            {
//                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
//                {
//                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_F", Value = rol_permisos.INT_IDPERMISO_F });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO", Value = rol_permisos.BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELA_ROL_PERMISO", Value = rol_permisos.FEC_MODIF_RELA_ROL_PERMISO });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@INT_IDROL_F", Value = rol_permisos.INT_IDROL_F });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_P", Value = rol_permisos.INT_IDRELACION_P });
//                    con.Open();
//                    int cantFilas = cmd.ExecuteNonQuery();
//                    con.Close();
//                    return cantFilas;
//                }
//            }
//            catch (Exception ex)
//            {
//                string error = ex.Message;
//                throw;
//            }
//        }

//        public int Update_ESTATUS_LOGICO(ROL_PERMISOS rol_permisos)
//        {
//            string add = "UPDATE " + cod + "TAB_ROL_PERMISOS" + cod + " SET "
//                + cod + "BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO" + ","
//                + cod + "FEC_MODIF_RELA_ROL_PERMISO" + cod + "= " + "@FEC_MODIF_RELA_ROL_PERMISO"
//                + " WHERE " + cod + "INT_IDRELACION_P" + cod + " = " + "@INT_IDRELACION_P";
//            try
//            {
//                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
//                {
//                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPERMISO_F", Value = rol_permisos.INT_IDPERMISO_F });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO", Value = rol_permisos.BOOL_ESTATUS_LOGICO_RELA_ROL_PERMISO });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FEC_MODIF_RELA_ROL_PERMISO", Value = rol_permisos.FEC_MODIF_RELA_ROL_PERMISO });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@INT_IDROL_F", Value = rol_permisos.INT_IDROL_F });
//                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_P", Value = rol_permisos.INT_IDRELACION_P });
//                    con.Open();
//                    int cantFilas = cmd.ExecuteNonQuery();
//                    con.Close();
//                    return cantFilas;
//                }
//            }
//            catch (Exception ex)
//            {
//                string error = ex.Message;
//                throw;
//            }
//        }
}
}

