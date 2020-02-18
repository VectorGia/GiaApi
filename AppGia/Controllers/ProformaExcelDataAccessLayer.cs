using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class ProformaExcelDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand comP = new NpgsqlCommand();
        public ProformaExcelDataAccessLayer()
        {
            //Constructor
            con = conex.ConnexionDB();
        }
        public DataTable Detalle(String consulta)
        {
            //string consulta = "SELECT * FROM proforma_detalle WHERE id_proforma = " + id_proforma;
            //+ " WHERE " + cod + "INT_ID_EMPRESA" + cod + " = " + id_empresa;

            try
            {
                //eve.WriteEntry("Se inicio el proceso de consulta", EventLogEntryType.Information);
                con.Open();
                comP = new Npgsql.NpgsqlCommand(consulta, con);
                Npgsql.NpgsqlDataAdapter daP = new Npgsql.NpgsqlDataAdapter(comP);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                //eve.WriteEntry(ex.Message, EventLogEntryType.Error);
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
