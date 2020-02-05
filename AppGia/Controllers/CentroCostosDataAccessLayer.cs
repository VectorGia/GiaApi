using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.IO;
using AppGia.Conexion;

namespace AppGia.Controllers
{
    public class CentroCostosDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public CentroCostosDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<CentroCostos> GetAllCentros()
        {
            //Obtiene todos los centros de costos habilitados "TRUE"
            //string consulta = " select * from " + " centro_costo " + " where " + "activo" + " = " + true;
            string consulta = "SELECT cc.id, cc.activo, cc.nombre, " +
               "cc.categoria, cc.desc_id, cc.estatus, " +
               "cc.fecha_modificacion, cc.gerente,cc.tipo, cc.empresa_id, " +
               "emp.nombre as nombre_empresa,cc.proyecto_id, " +
               "pry.nombre as nombre_proyecto " +
               "FROM centro_costo cc " +
               "INNER JOIN empresa emp on emp.id = cc.empresa_id " +
               "INNER JOIN proyecto pry on pry.id = cc.proyecto_id" + 
               " WHERE " + "activo" + " = " + true; ;
            try
            {
                List<CentroCostos> lstcentros = new List<CentroCostos>();
                {

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);


                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        CentroCostos centroCostos = new CentroCostos();

                        centroCostos.id = Convert.ToInt32(rdr["id"]);
                        centroCostos.desc_id = rdr["desc_id"].ToString().Trim();
                        centroCostos.activo = Convert.ToBoolean(rdr["activo"]);
                        centroCostos.estatus = (rdr["estatus"]).ToString().Trim();
                        centroCostos.nombre = rdr["nombre"].ToString().Trim();
                        centroCostos.tipo = rdr["tipo"].ToString().Trim();
                        centroCostos.categoria = rdr["categoria"].ToString().Trim();
                        centroCostos.gerente = rdr["gerente"].ToString().Trim();
                        centroCostos.fecha_modificacion = Convert.ToDateTime(rdr["fecha_modificacion"]);
                        centroCostos.nombre_empresa = rdr["nombre_empresa"].ToString().Trim();
                        centroCostos.nombre_proyecto = rdr["nombre_proyecto"].ToString().Trim();
                        lstcentros.Add(centroCostos);
                    }
                    con.Close();
                }
                return lstcentros;
            }
            catch (Exception ex)
            {
                con.Close();
                throw;
            }
        }
        //Obtiene los centro de costos por identificador unico 
        public CentroCostos GetCentroData(string id)
        {
            //string consulta = "select * from" + "centro_costo" + "where" + "id" + "=" + id;
            string consulta = "SELECT cc.id, cc.activo, cc.nombre, " +
                "cc.categoria, cc.desc_id, cc.estatus, " +
                "cc.fecha_modificacion, cc.gerente,cc.tipo, cc.empresa_id, " +
                "emp.nombre as nombre_empresa,cc.proyecto_id, " +
                "pry.nombre as nombre_proyecto " +
                "FROM centro_costo cc " +
                "INNER JOIN empresa emp on emp.id = cc.empresa_id " +
                "INNER JOIN proyecto pry on pry.id = cc.proyecto_id";
            try
            {
                CentroCostos centroCostos = new CentroCostos();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        centroCostos.id = Convert.ToInt32(rdr["id"]);
                        centroCostos.desc_id = rdr["desc_id"].ToString().Trim();
                        centroCostos.activo = Convert.ToBoolean(rdr["activo"]);
                        centroCostos.estatus = (rdr["estatus"]).ToString().Trim();
                        centroCostos.nombre = rdr["nombre"].ToString().Trim();
                        centroCostos.tipo = rdr["tipo"].ToString().Trim();
                        centroCostos.categoria = rdr["categoria"].ToString().Trim();
                        centroCostos.gerente = rdr["gerente"].ToString().Trim();
                        centroCostos.fecha_modificacion = Convert.ToDateTime(rdr["fecha_modificacion"]);
                        centroCostos.nombre_empresa = rdr["nombre_empresa"].ToString().Trim();
                        centroCostos.nombre_proyecto = rdr["nombre_proyecto"].ToString().Trim();
                    }
                    con.Close();
                }
                return centroCostos;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public int AddCentro(CentroCostos centroCostos)
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
                    cmd.Parameters.AddWithValue("@nombre", centroCostos.nombre.Trim());
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
            }
        }
        public int update(string id, CentroCostos centroCostos)
        {

            string update = " update  centro_costo set " +
                " tipo = @tipo  ," +
                " desc_id = @desc_id ," +
                " nombre  =  @nombre ," +
                " categoria =  @categoria ," +
                " estatus =   @estatus ," +
                " gerente =  @gerente ," +
                " empresa_id =  @empresa_id ," +
                " proyecto_id =  @proyecto_id ," +
                " fecha_modificacion =  @fecha_modificacion ," +
                " activo =  @activo " +
                " where " + "id" + " = " + id;


            {
                try
                {

                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo", Value = centroCostos.tipo });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@desc_id", Value = centroCostos.desc_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@nombre", Value = centroCostos.nombre });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@categoria", Value = centroCostos.categoria });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@estatus", Value = centroCostos.estatus });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@gerente", Value = centroCostos.gerente });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@proyecto_id", Value = centroCostos.proyecto_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@empresa_id", Value = centroCostos.empresa_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = centroCostos.activo });
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
            }
        }
        public int Delete(string id, CentroCostos centroCostos)
        {

            string delete = " update  centro_costo set  activo = @activo  where  @id  = " + id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = centroCostos.activo });
                    //   cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@id", Value = id });
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
