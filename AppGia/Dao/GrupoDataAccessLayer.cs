using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class GrupoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public GrupoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Grupo> GetAllGrupos()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_GRUPO" + cod + "WHERE " + cod + "BOOL_ESTATUS_LOGICO_GRUPO" + cod + "=" + true;
            try
            {
                List<Grupo> lstgrupo = new List<Grupo>();

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Grupo grupo = new Grupo();
                        grupo.STR_NOMBRE_GRUPO = rdr["STR_NOMBRE_GRUPO"].ToString().Trim();
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
            finally
            {
                con.Close();
            }
        }

        public Grupo GetGrupo(string id)
        {
            string consulta = " SELECT * FROM " + cod + "TAB_GRUPO" + cod + "WHERE" + cod + "INT_IDGRUPO_P" + cod + "=" + id;
            try
            {
                Grupo grupo = new Grupo();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    grupo.INT_IDGRUPO_P = Convert.ToInt32(rdr["INT_IDGRUPO_P"]);
                    grupo.STR_NOMBRE_GRUPO = rdr["STR_NOMBRE_GRUPO"].ToString().Trim();
                }

                con.Close();
                return grupo;

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

        public int addGrupo(Grupo grupo)

        {
           
            string add = "INSERT INTO" + cod + "TAB_GRUPO" + cod + "(" + cod + "STR_NOMBRE_GRUPO" + cod + "," + cod + "BOOL_ESTATUS_LOGICO_GRUPO"+cod+","+cod+ "FEC_MODIF_GRUPO"+cod+") VALUES " +
                "(@STR_NOMBRE_GRUPO,@BOOL_ESTATUS_LOGICO_GRUPO,@FEC_MODIF_GRUPO)";
            try
            {
            
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
            finally
            {
                con.Close();
            }
        }

        public int UpdateGrupo(string id, Grupo grupo)
        {
            string add = "UPDATE " + cod + "TAB_GRUPO" + cod + " SET "
                + cod + "STR_NOMBRE_GRUPO" + cod + "= " + "@STR_NOMBRE_GRUPO"
                + " WHERE " + cod + "INT_IDGRUPO_P" + cod + " = " + id;
            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_GRUPO", Value = grupo.STR_NOMBRE_GRUPO });
                 
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
            finally
            {
                con.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns>cantF</returns>
        public int DeleteGrupo(string id)
        {
            bool status = false;
            string add = "UPDATE " + cod + "TAB_GRUPO" + cod +
                " SET " + cod + "BOOL_ESTATUS_LOGICO_GRUPO" + cod + "= " + status +
                " WHERE " + cod + "INT_IDGRUPO_P" + cod + " = " + id;
            try
            {
              
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
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
            finally
            {
                con.Close();
            }
        }
    }
}
