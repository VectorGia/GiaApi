using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class RolDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();


        public RolDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public Rol GetRolById(string id)
        {
            string cadena = "SELECT * FROM CAT_ROL where BOOL_ESTATUS_LOGICO_ROL=true and INT_IDROL_P=" + id;
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Rol rol = new Rol();
                        rol.STR_NOMBRE_ROL = rdr["STR_NOMBRE_ROL"].ToString().Trim();
                        rol.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_P"]);
                        return rol;
                    }
                }

                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<Rol> GetAllRoles()
        {
            string cadena = "SELECT * FROM CAT_ROL where BOOL_ESTATUS_LOGICO_ROL=true";
            try
            {
                List<Rol> lstrol = new List<Rol>();


                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Rol rol = new Rol();
                        rol.STR_NOMBRE_ROL = rdr["STR_NOMBRE_ROL"].ToString().Trim();
                        rol.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_P"]);

                        lstrol.Add(rol);
                    }

                    con.Close();
                }

                return lstrol;
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

        public int addRol(Rol rol)
        {
            string add = "INSERT INTO" +
                         " CAT_ROL" +
                         "(STR_NOMBRE_ROL" +
                         ",BOOL_ESTATUS_LOGICO_ROL" +
                         ",FEC_MODIF_ROL" +
                         ") VALUES " +
                         "(@STR_NOMBRE_ROL,@BOOL_ESTATUS_LOGICO_ROL,@FEC_MODIF_ROL)";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_ROL", rol.STR_NOMBRE_ROL.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF_ROL", DateTime.Now);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_ROL", rol.BOOL_ESTATUS_LOGICO_ROL);

                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
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

        public int update(Rol rol)
        {
            string update = "UPDATE CAT_ROL SET " +
                            "STR_NOMBRE_ROL = @STR_NOMBRE_ROL ," +
                            "FEC_MODIF_ROL  = @FEC_MODIF_ROL " +
                            " WHERE INT_IDROL_P=@INT_IDROL_P";


            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);


                    cmd.Parameters.AddWithValue("@STR_NOMBRE_ROL", rol.STR_NOMBRE_ROL.Trim());
                    cmd.Parameters.AddWithValue("@FEC_MODIF_ROL", DateTime.Now);
                    cmd.Parameters.AddWithValue("@INT_IDROL_P", rol.INT_IDROL_P);


                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
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

        public int Delete(Rol rol)
        {
            string delete = "UPDATE CAT_ROL SET BOOL_ESTATUS_LOGICO_ROL=false WHERE INT_IDROL_P=" + rol.INT_IDROL_P;
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);

                    con.Open();
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
                }
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
    }
}