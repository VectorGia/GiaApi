using AppGia.Models.Etl;
using Npgsql;
using NpgsqlTypes;

namespace AppGia.Dao.Etl
{
   public class ProcesoDataAccessLayer
    {
        NpgsqlConnection conP;
        public ProcesoDataAccessLayer()
        {
            conP = new Conexion.Conexion().ConnexionDB();
        }
        

        public int AddProceso(Proceso proceso)
        {
            string add = "INSERT INTO proceso ("
                + " id,"
                + " estatus,"
                + " fecha_fin,"
                + " fecha_inicio,"
                + " mensaje,"
                + " tipo,"
                + " id_empresa "
                + " ) "
                + " VALUES ("
                + " @nextval('seq_proceso'),"
                + " @estatus,"
                + " @fecha_fin,"
                + " @fecha_inicio,"
                + " @mensaje,"
                + " @tipo, "
                + " @id_empresa )";

            try
            {
                {
                    conP.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, conP);
                    cmd.Parameters.AddWithValue("@estatus", NpgsqlDbType.Text, proceso.estatus);
                    cmd.Parameters.AddWithValue("@fecha_fin", NpgsqlDbType.Date, proceso.fecha_fin);
                    cmd.Parameters.AddWithValue("@fecha_inicio", NpgsqlDbType.Date, proceso.fecha_inicio);
                    cmd.Parameters.AddWithValue("@mensaje", NpgsqlDbType.Text, proceso.mensaje);
                    cmd.Parameters.AddWithValue("@tipo", NpgsqlDbType.Text, proceso.tipo);
                    cmd.Parameters.AddWithValue("@id_empresa", NpgsqlDbType.Bigint, proceso.id_empresa);

                    int cantFilAfec = cmd.ExecuteNonQuery();
                    conP.Close();
                    return cantFilAfec;
                }
            }
            finally
            {
                conP.Close();
            }
        }
    }
}
