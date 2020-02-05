using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class ProcesoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public ProcesoDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Proceso> GetAllProcesos()
        {
             
            string consulta = "SELECT id,empresa,estatus,fecha_fin,fecha_inicio,mensaje,tipo,id_empresa FROM proceso ";
            try
            {
                List<Proceso> lstProcesos = new List<Proceso>();
                {

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);


                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Proceso proceso = new Proceso();

                        proceso.id = Convert.ToInt64(rdr["id"]);
                        proceso.empresa = Convert.ToString(rdr["empresa"]);
                        proceso.estatus = Convert.ToString(rdr["estatus"]);
                        proceso.fecha_fin = Convert.ToDateTime(rdr["fecha_fin"]);
                        proceso.fecha_inicio = Convert.ToDateTime(rdr["fecha_inicio"]);
                        proceso.mensaje = Convert.ToString(rdr["mensaje"]);
                        proceso.tipo = Convert.ToString(rdr["tipo"]);
                        proceso.id_empresa = Convert.ToInt64(rdr["id_empresa"]);
                        lstProcesos.Add(proceso);
                    }
                    con.Close();
                }
                return lstProcesos;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
        }
        //Obtiene las cuentas de cada compañia 
        public Proceso GetProcesoData(long id)
        {
            string consulta = "SELECT id,empresa,estatus,fecha_fin,fecha_inicio,mensaje,tipo,id_empresa FROM proceso where id= "+id;
            try
            {
                Proceso proceso = new Proceso();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        proceso.id = Convert.ToInt64(rdr["id"]);
                        proceso.empresa = Convert.ToString(rdr["empresa"]);
                        proceso.estatus = Convert.ToString(rdr["estatus"]);
                        proceso.fecha_fin = Convert.ToDateTime(rdr["fecha_fin"]);
                        proceso.fecha_inicio = Convert.ToDateTime(rdr["fecha_inicio"]);
                        proceso.mensaje = Convert.ToString(rdr["mensaje"]);
                        proceso.tipo = Convert.ToString(rdr["tipo"]);
                        proceso.id_empresa = Convert.ToInt64(rdr["id_empresa"]);

                    }

                    con.Close();
                }
                return proceso;
            }
            catch
            {
                con.Close();
                throw;

            }
        }
        public int AddProceso(Proceso proceso)
        {
            string add = "INSERT INTO proceso ("
                + " id,"
                + " empresa,"
                + " estatus,"
                + " fecha_fin,"
                + " fecha_inicio,"
                + " mensaje,"
                + " tipo,"
                + " id_empresa "
                + " ) "
                + " VALUES ("
                + " @nextval('seq_proceso'),"
                + " @empresa,"
                + " @estatus,"
                + " @fecha_fin,"
                + " @fecha_inicio,"
                + " @mensaje,"
                + " @tipo, "
                + " @id_empresa )";

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@empresa", NpgsqlTypes.NpgsqlDbType.Text, proceso.empresa);
                    cmd.Parameters.AddWithValue("@estatus", NpgsqlTypes.NpgsqlDbType.Text, proceso.estatus);
                    cmd.Parameters.AddWithValue("@fecha_fin",NpgsqlTypes.NpgsqlDbType.Date, proceso.fecha_fin);
                    cmd.Parameters.AddWithValue("@fecha_inicio",  NpgsqlTypes.NpgsqlDbType.Date, proceso.fecha_inicio);
                    cmd.Parameters.AddWithValue("@mensaje", NpgsqlTypes.NpgsqlDbType.Text, proceso.mensaje);
                    cmd.Parameters.AddWithValue("@tipo",  NpgsqlTypes.NpgsqlDbType.Text, proceso.tipo);
                    cmd.Parameters.AddWithValue("@id_empresa", NpgsqlTypes.NpgsqlDbType.Bigint, proceso.id_empresa);

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
    }
}
