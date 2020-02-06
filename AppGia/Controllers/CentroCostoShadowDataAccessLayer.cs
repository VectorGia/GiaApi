using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class CentroCostoShadowDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public CentroCostoShadowDataAccessLayer()
        {
            //Constructor
            con = conex.ConnexionDB();
        }

        public int AddCentroShadow(CentroCostos centroCostos)
        {
            string add = "insert into " + "centro_costo" + "(" + "id" + "," + "tipo" + "," + "desc_id" + "," + "nombre" + "," + "categoria" + "," + "estatus" + "," + "gerente" + "," + "empresa_id" + "," + "proyecto_id" + "," + "fecha_modificacion" + "," + "activo" + ")" +
                "values (nextval('seq_centro_costo'),@tipo,@desc_id,@nombre,@categoria,@estatus,@gerente,@empresa_id,@proyecto_id,@fecha_modificacion,@activo)";
            {
                try
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("@tipo", centroCostos.tipo.Trim());
                    cmd.Parameters.AddWithValue("@desc_id", centroCostos.desc_id.Trim());
                    cmd.Parameters.AddWithValue("@nombre", centroCostos.nombre.Trim() + "-shadow");
                    cmd.Parameters.AddWithValue("@categoria", centroCostos.categoria.Trim());
                    cmd.Parameters.AddWithValue("@estatus", centroCostos.estatus);
                    cmd.Parameters.AddWithValue("gerente", centroCostos.gerente.Trim());
                    cmd.Parameters.AddWithValue("empresa_id", centroCostos.empresa_id);
                    cmd.Parameters.AddWithValue("proyecto_id", centroCostos.proyecto_id);
                    cmd.Parameters.AddWithValue("@fecha_modificacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@activo", centroCostos.activo);
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
        public int updateCentroShadow(string id, CentroCostos centroCostos)
        {

            string update = " update  centro_costo set " +
                " tipo = @tipo  ," +
                " desc_id = @desc_id ," +
                " nombre  =  @nombre ," +
                " categoria =  @categoria ," +
                " estatus =   @estatus ," +
                " gerente =  @gerente ," +
                " empresa_id =  @empresa_id ," +
                " fecha_modificacion =  @fecha_modificacion ," +
                " where " + "id" + " = " + id;
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo", Value = centroCostos.tipo });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@desc_id", Value = centroCostos.desc_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@nombre", Value = centroCostos.nombre + "-shadow" });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@categoria", Value = centroCostos.categoria });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@estatus", Value = centroCostos.estatus });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@gerente", Value = centroCostos.gerente });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@empresa_id", Value = centroCostos.empresa_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@fecha_modificacion", Value = DateTime.Now });

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
        public int DeleteCentroShadow(string id, CentroCostos centroCostos)
        {
            string delete = " update  centro_costo set  activo = @activo  where  @id  = " + id;
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = centroCostos.activo });                  
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
