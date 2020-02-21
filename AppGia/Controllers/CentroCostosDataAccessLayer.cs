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
               "pry.nombre as nombre_proyecto, cc.modelo_negocio_id, cc.porcentaje, cc.proyeccion  " +
               "FROM centro_costo cc " +
               "INNER JOIN empresa emp on emp.id = cc.empresa_id " +
               "INNER JOIN proyecto pry on pry.id = cc.proyecto_id" + 
               " where cc.activo = true order by cc.id desc" ; 

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
                        centroCostos.estatus = (rdr["estatus"]).ToString().Trim();
                        centroCostos.nombre = rdr["nombre"].ToString().Trim();
                        centroCostos.tipo = rdr["tipo"].ToString().Trim();
                        centroCostos.categoria = rdr["categoria"].ToString().Trim();
                        centroCostos.gerente = rdr["gerente"].ToString().Trim();
                        centroCostos.fecha_modificacion = Convert.ToDateTime(rdr["fecha_modificacion"]);
                        centroCostos.nombre_empresa = rdr["nombre_empresa"].ToString().Trim();
                        centroCostos.nombre_proyecto = rdr["nombre_proyecto"].ToString().Trim();
                        centroCostos.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"].ToString());
                        centroCostos.porcentaje = Convert.ToDouble(rdr["porcentaje"]);
                        centroCostos.proyeccion = rdr["proyeccion"].ToString().Trim();
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
            finally
            {
                con.Close();
            }
        }
        //Obtiene los centro de costos por identificador unico 
        public List<CentroCostos> GetCentroData(int idproyecto)
        {
            //string consulta = "select * from" + "centro_costo" + "where" + "id" + "=" + id;
            string consulta = "SELECT cc.id, cc.activo, cc.nombre, " +
                "cc.categoria, cc.desc_id, cc.estatus, " +
                "cc.fecha_modificacion, cc.gerente,cc.tipo, cc.empresa_id, " +
                "emp.nombre as nombre_empresa,cc.proyecto_id, " +
                "pry.nombre as nombre_proyecto,cc.modelo_negocio_id, cc.porcentaje, cc.proyeccion " +
                "FROM centro_costo cc " +
                "INNER JOIN empresa emp on emp.id = cc.empresa_id " +
                "INNER JOIN proyecto pry on pry.id = cc.proyecto_id " + "and cc.activo = true and cc.proyecto_id = " + idproyecto;

            List<CentroCostos> listcentrocostos = new List<CentroCostos>();
            try
            {
                
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
                        centroCostos.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"]);
                        centroCostos.porcentaje = Convert.ToDouble(rdr["porcentaje"]);
                        centroCostos.proyeccion = rdr["proyeccion"].ToString().Trim();
                        listcentrocostos.Add(centroCostos);
                    }
                    con.Close();
                }
                return listcentrocostos;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public CentroCostos GetCentro(int id)
        {
            string consulta = "select * from centro_costo where id = " + id;

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
                        centroCostos.modelo_negocio_id = Convert.ToInt64(rdr["modelo_negocio_id"]);
                        centroCostos.porcentaje = Convert.ToDouble(rdr["porcentaje"]);
                        centroCostos.proyeccion = rdr["proyeccion"].ToString().Trim();
                     
                    }
                    con.Close();
                }
                return centroCostos;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        public int AddCentro(CentroCostos centroCostos)
        {
            string add = "insert into centro_costo(id, tipo, desc_id, nombre, categoria, estatus, gerente, empresa_id, proyecto_id, fecha_modificacion, activo, modelo_negocio_id, porcentaje, proyeccion) " +
                " values (nextval('seq_centro_costo'), @tipo, @desc_id, @nombre, @categoria, @estatus, @gerente, @empresa_id, @proyecto_id, @fecha_modificacion, @activo, @modelo_negocio_id, @porcentaje, @proyeccion)";
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
                    cmd.Parameters.AddWithValue("@modelo_negocio_id", centroCostos.modelo_negocio_id);
                    cmd.Parameters.AddWithValue("@porcentaje", centroCostos.porcentaje);
                    cmd.Parameters.AddWithValue("@proyeccion", centroCostos.proyeccion);
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
                " fecha_modificacion =  @fecha_modificacion ," +
                " porcentaje =  @porcentaje ," +
                " proyeccion =  @proyeccion " +
                " where id = " + id;


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
                    //cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@proyecto_id", Value = centroCostos.proyecto_id });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@empresa_id", Value = centroCostos.empresa_id });
                    //cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = centroCostos.activo });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@fecha_modificacion", Value = DateTime.Now });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Double, ParameterName = "@porcentaje", Value = centroCostos.porcentaje });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@proyeccion", Value = centroCostos.proyeccion });

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
        public int Delete(int id)
        {

            string delete = " update  centro_costo set  activo = false  where  @id  = " + id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
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
            finally
            {
                con.Close();
            }
        }

    }
}
