using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class GrupoDataAccessLayer
    {
        //private string connectionString = "User ID=postgres;Password=omnisys;Host=192.168.1.78;Port=5432;Database=GIA;Pooling=true;";
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public GrupoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Grupo> GetAllGrupos()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_GRUPO" + cod + "";
            try
            {
                List<Grupo> lstgrupo = new List<Grupo>();

                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Grupo grupo = new Grupo();
                        grupo.STR_NOMBRE_GRUPO = rdr["STR_NOMBRE_GRUPO"].ToString();
                        grupo.INT_IDGRUPO_P = Convert.ToInt32(rdr["INT_IDGRUPO_P"]);

                        lstgrupo.Add(grupo);
                    }
                    con.Close();
                }

                return lstgrupo;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public int addGrupo(Grupo grupo)

        {
           
            string add = "INSERT INTO" + cod + "TAB_GRUPO" + cod + "(" + cod + "STR_NOMBRE_GRUPO" + cod + "," + cod + "BOOL_ESTATUS_LOGICO_GRUPO"+cod+","+cod+ "FEC_MODIF_GRUPO"+cod+") VALUES " +
                "(@STR_NOMBRE_GRUPO,@BOOL_ESTATUS_LOGICO_GRUPO,@FEC_MODIF_GRUPO)";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_GRUPO", grupo.STR_NOMBRE_GRUPO);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_GRUPO", grupo.BOOL_ESTATUS_LOGICO_GRUPO);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_GRUPO", DateTime.Now);
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
        }

        public int UpdateGrupo(Grupo grupo)
        {
            string add = "UPDATE " + cod + "TAB_GRUPO" + cod + " SET "
                + cod + "STR_NOMBRE_GRUPO" + cod + "= " + "@STR_NOMBRE_GRUPO"
                + " WHERE " + cod + "INT_IDGRUPO_P" + cod + " = " + "@INT_IDGRUPO_P";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_GRUPO", Value = grupo.STR_NOMBRE_GRUPO });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_P", Value = grupo.INT_IDGRUPO_P });
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns>cantF</returns>
        public int DeleteGrupo(Grupo grupo)
        {
            string add = "UPDATE " + cod + "TAB_GRUPO" + cod +
                " SET " + cod + "BOOL_ESTATUS_LOGICO_GRUPO" + cod + "= " + "@BOOL_ESTATUS_LOGICO_GRUPO" +
                " WHERE " + cod + "INT_IDGRUPO_P" + cod + " = " + "@INT_IDGRUPO_P";
            try
            {
                //using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDGRUPO_P", Value = grupo.INT_IDGRUPO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_GRUPO", Value = grupo.BOOL_ESTATUS_LOGICO_GRUPO });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
    }
}
