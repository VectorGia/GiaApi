using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class PeriodoDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        public PeriodoDataAccessLayer()
        {
            //Constructor
            con = conex.ConnexionDB();
        }

        public IEnumerable<Periodo> GetAllPeriodos()
        {
            //Obtiene todos los periodods habilitados "TRUE"            
            string consulta = "select p.id, p.activo, p.anio_periodo, p.estatus, p.fec_modif, tp.clave, t.descripcion from periodo p " +
                                    "inner join tipo_captura tp on p.tipo_captura_id = tp.id " +
                                    "inner join tipo_proforma t on p.tipo_proforma_id = t.id " +
                                "where p.activo = true";
            try
            {
                List<Periodo> lstperiodo = new List<Periodo>();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Periodo periodo = new Periodo();

                        periodo.id = Convert.ToInt64(rdr["id"]);
                        periodo.activo = Convert.ToBoolean(rdr["activo"]);
                        periodo.anio_periodo = Convert.ToInt32(rdr["anio_periodo"]);
                        periodo.estatus = rdr["estatus"].ToString().Trim();
                        periodo.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                        //periodo.idusuario = Convert.ToInt64(rdr["idusuario"]);
                        periodo.clave = rdr["clave"].ToString();
                        periodo.descripcion = rdr["descripcion"].ToString();
                        lstperiodo.Add(periodo);
                    }
                    con.Close();
                }
                return lstperiodo;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public Periodo GetPeriodoData(string id)
        {
            try
            {
                Periodo periodo = new Periodo();
                {
                    string consulta = "  select * from periodo"
                                     + " where  id  = " + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        periodo.id = Convert.ToInt64(rdr["id"]);
                        periodo.activo = Convert.ToBoolean(rdr["activo"]);
                        periodo.anio_periodo = Convert.ToInt32(rdr["anio_periodo"]);
                        periodo.estatus = rdr["estatus"].ToString().Trim();
                        periodo.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                        //periodo.idusuario = Convert.ToInt64(rdr["idusuario"]);
                        periodo.tipo_captura_id = Convert.ToInt64(rdr["tipo_captura_id"]);
                        periodo.tipo_proforma_id = Convert.ToInt64(rdr["tipo_proforma_id"]);
                    }
                }
                return periodo;
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

        public int AddPeriodo(Periodo periodo)
        {
            string add = "INSERT INTO periodo(" +
                "id," +
                "activo," +
                "anio_periodo, " +
                "estatus, " +
                "fec_modif, " +
                "tipo_captura_id, " +
                "tipo_proforma_id) " +
                "values (nextval('seq_periodo'), " +
                "@activo, " +
                "@anio_periodo, " +
                "@estatus, " +
                "@fec_modif, "+
                "@tipo_captura_id, " +
                "@tipo_proforma_id);";
            {
                try
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    //cmd.Parameters.AddWithValue("@id", periodo.id);
                    cmd.Parameters.AddWithValue("@activo", periodo.activo);
                    cmd.Parameters.AddWithValue("@anio_periodo", periodo.anio_periodo);
                    cmd.Parameters.AddWithValue("@estatus", periodo.estatus);
                    cmd.Parameters.AddWithValue("@fec_modif", periodo.fec_modif);
                    //cmd.Parameters.AddWithValue("@idusuario", periodo.idusuario);
                    cmd.Parameters.AddWithValue("@tipo_captura_id", periodo.tipo_captura_id);
                    cmd.Parameters.AddWithValue("@tipo_proforma_id", periodo.tipo_proforma_id);
                    int cantFilAfec = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilAfec;
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

        public int updatePeriodo(string id, Periodo periodo)
        {
            string update = "UPDATE periodo SET activo=@activo, " +
                "anio_periodo=@anio_periodo," +
                "fec_modif=@fec_modif, " +
                "estatus=@estatus, " +
                "tipo_captura_id=@tipo_captura_id," +
                "tipo_proforma_id=@tipo_proforma_id" +
                "WHERE id = @id;";
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@anio_periodo", Value = periodo.anio_periodo });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@fec_modif", Value = periodo.fec_modif });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@estatus", Value = periodo.estatus });
                    //cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@idusuario", Value = periodo.idusuario });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo_captura_id", Value = periodo.tipo_captura_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo_proforma_id", Value = periodo.tipo_captura_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@id", Value = id });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
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

        public int deletePeriodo(string id)
        {
            string update = "UPDATE periodo SET activo=false" +
                            " where id = " + id;
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);                    
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
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
}
