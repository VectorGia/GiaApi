using System.Collections.Generic;
using AppGia.Models;
using Npgsql;


namespace AppGia.Controllers
{
    public class PantallaDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';
        public IEnumerable<Pantalla> GetAllPantallas()
        {
            string cadena = "SELECT * FROM" + cod + "TAB_PANTALLA" + cod + "";
            try
            {
                List<Pantalla> lstpantalla = new List<Pantalla>();

                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Pantalla pantalla = new Pantalla();
                        pantalla.STR_NOMBRE_PANTALLA = rdr["STR_NOMBRE_PANTALLA"].ToString();

                        lstpantalla.Add(pantalla);
                    }
                    con.Close();
                }

                return lstpantalla;
            }
            catch
            {
                throw;
            }
        }

        public int addPantalla(Pantalla pantalla)
        {
            string add = "INSERT INTO" + cod + "TAB_PANTALLA" + cod + "(" + cod + "STR_NOMBRE_PANTALLA" + cod + ") VALUES " +
                "(@STR_NOMBRE_PANTALLA)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@STR_NOMBRE_PANTALLA", pantalla.STR_NOMBRE_PANTALLA);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return 1;
            }
            catch
            {
                throw;
            }
        } 
        
        public int UpdatePantalla(Pantalla pantalla)
            {

                string add = "UPDATE " + cod + "TAB_PANTALLA" + cod +
                " SET " + cod + "INT_IDROL_F" + cod + "= " + "'" + "@INT_IDROL_F" + "'" + ","
                + cod + "FEC_MODIF" + cod + "= " + "'" + "@FEC_MODIF" + "'" + ","
                + cod + "STR_NOMBRE_PANTALLA" + cod + "= " + "'" + "@STR_NOMBRE_PANTALLA" + "'"            
                + " WHERE " + cod + "INT_IDPANTALLA_P" + cod + " = " + "@INT_IDPANTALLA_P";

            try
                {
                    using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                    {
                        NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = pantalla.INT_IDPANTALLA_P});
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDROL_F", Value = pantalla.INT_IDROL_F });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.TimestampTz, ParameterName = "@FEC_MODIF", Value = pantalla.FEC_MODIF });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@STR_NOMBRE_PANTALLA", Value = pantalla.STR_NOMBRE_PANTALLA });                  
                    
                    con.Open();
                        int cantFilas = cmd.ExecuteNonQuery();
                        con.Close();
                        return cantFilas;
                    }
                    //return 1;
                }
                catch
                {
                    throw;
                }
            }

       

        //public int DeletePantalla(int id)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand("spDeleteCentroCostos", con);


        //            cmd.Parameters.AddWithValue("STR_NOMBRE_PANTALLA", id);

        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return 1;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

 public int DeleteGrupo(Pantalla pantalla)
        {
            string add = "UPDATE " + cod + "TAB_PANTALLA" + cod + " SET " + cod + "BOOL_ESTATUS_LOGICO_PANT" + cod + "= " + "'" + "@BOOL_ESTATUS_LOGICO_PANT" + "'" + " WHERE " + cod + "INT_IDPANTALLA_P" + cod + " = " + "@INT_IDPANTALLA_P";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPANTALLA_P", Value = pantalla.INT_IDPANTALLA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_PANT", Value = pantalla.BOOL_ESTATUS_LOGICO_PANT });
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }
                //return 1;
            }
            catch
            {
                throw;
            }
        }

    }
}
