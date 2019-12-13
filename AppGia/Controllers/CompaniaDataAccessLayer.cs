using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class CompaniaDataAccessLayer
    {
        private string connectionString = "User ID=postgres;Password=HolaMundo1;Host=192.168.1.73;Port=5432;Database=GIA;Pooling=true;";
        char cod = '"';

        public IEnumerable<Compania> GetAllCompanias()
        {
            string cadena = "SELECT * FROM" + cod + "CAT_COMPANIA" + cod + "";
            try
            {
                List<Compania> lstcompania = new List<Compania>();
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Compania compania = new Compania();

                        compania.STR_IDCOMPANIA = rdr["STR_IDCOMPANIA"].ToString();
                        compania.STR_NOMBRE_COMPANIA = rdr["STR_NOMBRE_COMPANIA"].ToString();
                        compania.STR_ABREV_COMPANIA = rdr["STR_ABREV_COMPANIA"].ToString();
                        compania.BOOL_ETL_COMPANIA = Convert.ToBoolean(rdr["BOOL_ETL_COMPANIA"]);
                        compania.STR_HOST_COMPANIA = rdr["STR_HOST_COMPANIA"].ToString();
                        compania.STR_PUERTO_COMPANIA = rdr["STR_PUERTO_COMPANIA"].ToString();
                        compania.STR_USUARIO_ETL = rdr["STR_USUARIO_ETL"].ToString();
                        compania.STR_CONTRASENIA_ETL = rdr["STR_CONTRASENIA_ETL"].ToString();
                        compania.STR_BD_COMPANIA = rdr["STR_BD_COMPANIA"].ToString();
                        compania.STR_MONEDA_COMPANIA = rdr["STR_MONEDA_COMPANIA"].ToString();

                        lstcompania.Add(compania);
                    }
                    con.Close();
                }
                return lstcompania;
            }
            catch
            {
                throw;
            }
        }

        public int addCompania(Compania compania)
        {
            string add = "INSERT INTO" + cod +
                "CAT_COMPANIA" + cod +
                "("+cod+"STR_NOMBRE_COMPANIA"+cod+
                ","+cod+"STR_ABREV_COMPANIA"+cod+
                ","+cod+"BOOL_ETL_COMPANIA"+cod+
                ","+cod+"STR_HOST_COMPANIA"+cod+
                ","+cod+"STR_MONEDA_COMPANIA"+cod+
                ","+cod+"STR_IDCOMPANIA"+cod+
                ","+cod+"STR_USUARIO_ETL"+cod+
                ","+cod+"STR_CONTRASENIA_ETL"+cod+
                ","+cod+"STR_PUERTO_COMPANIA"+cod+
                ","+cod+"STR_BD_COMPANIA"+cod+
                ","+cod+"BOOL_ESTATUS_LOGICO_COMPANIA"+cod+
                ") VALUES " +
                "(@STR_NOMBRE_COMPANIA,@STR_ABREV_COMPANIA,@BOOL_ETL_COMPANIA,@STR_HOST_COMPANIA,@STR_MONEDA_COMPANIA,@STR_IDCOMPANIA,@STR_USUARIO_ETL,@STR_CONTRASENIA_ETL,@STR_PUERTO_COMPANIA,@STR_BD_COMPANIA,@BOOL_ESTATUS_LOGICO_COMPANIA)";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    //cmd.Parameters.AddWithValue("@INT_IDGRUPO", grupo.INT_IDGRUPO);
                    cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", compania.STR_IDCOMPANIA);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_COMPANIA", compania.STR_NOMBRE_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_ABREV_COMPANIA", compania.STR_ABREV_COMPANIA);
                    cmd.Parameters.AddWithValue("@BOOL_ETL_COMPANIA", compania.BOOL_ETL_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_HOST_COMPANIA", compania.STR_HOST_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_MONEDA_COMPANIA", compania.STR_MONEDA_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_USUARIO_ETL", compania.STR_USUARIO_ETL);
                    cmd.Parameters.AddWithValue("@STR_CONTRASENIA_ETL", compania.STR_CONTRASENIA_ETL);
                    cmd.Parameters.AddWithValue("@STR_PUERTO_COMPANIA", compania.STR_PUERTO_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_BD_COMPANIA", compania.STR_BD_COMPANIA);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", compania.BOOL_ESTATUS_LOGICO_COMPANIA);
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

        public int update(Compania compania)
        {

            string update = "UPDATE " + cod + "CAT_COMPANIA" + cod + "SET"
            + cod + "INT_IDPROYECTO_F" + cod + " = " + "@INT_IDPROYECTO_F" + ","
            + cod + "INT_IDCENTROCOSTO_F" + cod + " = " + "@INT_IDCENTROCOSTO_F" + ","
            + cod + "STR_NOMBRE_COMPANIA" + cod + " = '" + "@STR_NOMBRE_COMPANIA" + "' ,"
            + cod + "STR_ABREV_COMPANIA" + cod + " = '" + "@STR_ABREV_COMPANIA" + "' ,"
            + cod + "STR_HOST_COMPANIA" + cod + " = '" + "@STR_HOST_COMPANIA" + "' ,"
            + cod + "STR_MONEDA_COMPANIA" + cod + " = '" + "@STR_MONEDA_COMPANIA" + "' ,"
            + cod + "STR_IDCOMPANIA" + cod + " = '" + "@STR_IDCOMPANIA" + "' ,"
            + cod + "STR_USUARIO_ETL" + cod + " = '" + "@STR_USUARIO_ETL" + "' ,"
            + cod + "STR_CONTRASENIA_ETL" + cod + " = '" + "@STR_CONTRASENIA_ETL" + "' ,"
            + cod + "STR_PUERTO_COMPANIA" + cod + " = '" + "@STR_PUERTO_COMPANIA" + "' ,"
            + cod + "STR_BD_COMPANIA" + cod + " = '" + "@STR_BD_COMPANIA" + "' ,"
            + cod + "BOOL_ETL_COMPANIA" + cod + " = '" + "@BOOL_ETL_COMPANIA" + "' ,"
            + cod + "FEC_MODIF_COMPANIA" + cod + "= '" + "@FEC_MODIF_COMPANIA" + "' ,"
            + cod + "BOOL_ESTATUS_LOGICO_COMPANIA" + cod + "= '" + "@BOOL_ESTATUS_LOGICO_COMPANIA" + "' "
            + " WHERE" + cod + "INT_IDCOMPANIA_P" + cod + "=" + "@INT_IDCOMPANIA_P";

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@INT_IDPROYECTO_F", NpgsqlTypes.NpgsqlDbType.Integer,compania.INT_IDPROYECTO_F);
                    cmd.Parameters.AddWithValue("@INT_IDCENTROCOSTO_F", NpgsqlTypes.NpgsqlDbType.Integer, compania.INT_IDCENTROCOSTO_F);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text,compania.STR_NOMBRE_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_ABREV_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_ABREV_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_HOST_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_HOST_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_MONEDA_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_MONEDA_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_IDCOMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_IDCOMPANIA);
                    cmd.Parameters.AddWithValue("@STR_USUARIO_ETL", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_USUARIO_ETL);
                    cmd.Parameters.AddWithValue("@STR_CONTRASENIA_ETL", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_CONTRASENIA_ETL);
                    cmd.Parameters.AddWithValue("@STR_PUERTO_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_PUERTO_COMPANIA);
                    cmd.Parameters.AddWithValue("@STR_BD_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.STR_BD_COMPANIA);
                    cmd.Parameters.AddWithValue("@BOOL_ETL_COMPANIA", NpgsqlTypes.NpgsqlDbType.Text, compania.BOOL_ETL_COMPANIA);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_COMPANIA", NpgsqlTypes.NpgsqlDbType.Date, compania.FEC_MODIF_COMPANIA);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", NpgsqlTypes.NpgsqlDbType.Boolean, compania.BOOL_ESTATUS_LOGICO_COMPANIA);

                    con.Open();
                    int cantF=  cmd.ExecuteNonQuery();
                    con.Close();
                    return cantF;
                }
                
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                int numError = 0;
                return numError;
            }
        }

        public int Delete(Compania compania)
        {
            string delete = "UPDATE " + cod + "CAT_COMPANIA" + cod + "SET" + cod + "BOOL_ESTATUS_LOGICO_COMPANIA" + cod + "='" + compania.BOOL_ESTATUS_LOGICO_COMPANIA + "' WHERE" + cod + "INT_IDCOMPANIA_P" + cod + "='" + compania.INT_IDCOMPANIA_P + "'";
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", compania.BOOL_ESTATUS_LOGICO_COMPANIA);

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
    }
}
