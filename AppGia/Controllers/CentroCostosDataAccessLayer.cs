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

        char cod = '"';

        public IEnumerable<CentroCostos> GetAllCentros()
        {
            //Obtiene todos los centros de costos habilitados "TRUE"
            string consulta = " select * from " + "centro_costo" + " where " + "activo" + "=" + true; 
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
                        centroCostos.estatus = Convert.ToBoolean(rdr["estatus"]);
                        centroCostos.nombre = rdr["nombre"].ToString().Trim();
                        centroCostos.tipo = rdr["tipo"].ToString().Trim();
                        centroCostos.categoria = rdr["categoria"].ToString().Trim();
                        centroCostos.gerente = rdr["gerente"].ToString().Trim();
                        centroCostos.fech_modificacion = Convert.ToDateTime(rdr["fech_modificacion"]);




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
        public CentroCostos GetCentroData(string id )
        {
            string consulta = "select * from" + "centro_costo" + "where" + "id" + "=" + id;
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
                        centroCostos.estatus = Convert.ToBoolean(rdr["estatus"]);
                        centroCostos.nombre = rdr["nombre"].ToString().Trim();
                        centroCostos.tipo = rdr["tipo"].ToString().Trim();
                        centroCostos.categoria = rdr["categoria"].ToString().Trim();
                        centroCostos.gerente = rdr["gerente"].ToString().Trim();
                        centroCostos.fech_modificacion = Convert.ToDateTime(rdr["fech_modificacion"]);

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
            string add = "insert into " + "centro_costo" + "(" + "tipo" + "," + "desc_id" + "," + "nombre" + "," + "categoria" + "," + "estatus" + "," + "gerente" + "," + "fecha_modificacion" + "," + "activo" + ")" +
                "values (@tipo,@desc_id,@nombre,@categaria,@estatus,@gerente,@fecha_modificacion,@activo)";
            {
                try
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.AddWithValue("tipo", centroCostos.tipo.Trim());
                    cmd.Parameters.AddWithValue("@desc_id", centroCostos.desc_id.Trim());
                    cmd.Parameters.AddWithValue("nombre", centroCostos.nombre.Trim());
                    cmd.Parameters.AddWithValue("categoria", centroCostos.categoria.Trim());
                    cmd.Parameters.AddWithValue("estatus", centroCostos.estatus);
                    cmd.Parameters.AddWithValue("gerente", centroCostos.gerente.Trim());
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

            string update = "update "  + "centro" + "set"
            +  "desc_id" + " = '" + centroCostos.desc_id + "' ,"
            +  "categoria" + " = '" + centroCostos.categoria + "' ,"
            +  "gerente"  + " = '" + centroCostos.gerente + "' ,"
            + "activo" + " = '" + centroCostos.activo + "' ,"
            +  "estatus"  + " = '" + centroCostos.estatus + "' ,"
            +  "fecha_modifcacion" + " = " + centroCostos.fech_modificacion + "' ,"
            +  "tipo"  + " = '" + centroCostos.tipo + "'"
            + " where"  + "id" + cod + "=" + id;


            try
            {
                {
                    
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@desc_id", centroCostos.desc_id.Trim());
                    cmd.Parameters.AddWithValue("@nombre", centroCostos.nombre.Trim());
                    cmd.Parameters.AddWithValue("@categoria", centroCostos.categoria.Trim());
                    cmd.Parameters.AddWithValue("@gerente", centroCostos.gerente.Trim());
                    cmd.Parameters.AddWithValue("@activo", centroCostos.activo);
                    cmd.Parameters.AddWithValue("@estatus", centroCostos.estatus);
                    cmd.Parameters.AddWithValue("@fecha_modificacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@tipo", centroCostos.tipo.Trim());
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

        public int Delete(string id)
        {
            bool status = false;
            string delete = "update " + cod + "centro_costo"  + "set" + "estatus"  + "='" +status+ "' where"  + "id"  + "='" + id + "'";
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
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
