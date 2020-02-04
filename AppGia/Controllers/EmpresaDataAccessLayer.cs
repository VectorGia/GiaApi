﻿using System;
using System.Collections.Generic;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class EmpresaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public EmpresaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public IEnumerable<Empresa> GetAllEmpresas()
        {
            //string cadena = "select *from empresa where  activo = " + true;
            string cadena = "select id,activo,nombre,abrev,bd_name,contrasenia_etl," +
                "desc_id,etl,fec_modif,host,puerto_compania,usuario_etl,moneda_id from empresa where activo = " + true;
            try
            {
                List<Empresa> lstempresa = new List<Empresa>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    Empresa empresa = new Empresa();

                    while (rdr.Read())
                    {
                        empresa.id = Convert.ToInt32(rdr["id"]);
                        empresa.desc_id = rdr["desc_id"].ToString().Trim();
                        empresa.activo = Convert.ToBoolean(rdr["activo"]);
                        empresa.nombre = rdr["nombre"].ToString().Trim();
                        empresa.abrev = rdr["abrev"].ToString().Trim();
                        empresa.bd_name = rdr["bd_name"].ToString().Trim();
                        empresa.contrasenia_etl = rdr["contrasenia_etl"].ToString().Trim();
                        empresa.etl = Convert.ToBoolean(rdr["etl"]);
                        empresa.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                        empresa.host = rdr["host"].ToString().Trim();
                        empresa.puerto_compania = Convert.ToInt32(rdr["puerto_compania"]);
                        empresa.usuario_etl = rdr["usuario_etl"].ToString().Trim();
                        empresa.moneda_id = Convert.ToInt32(rdr["moneda_id"]);
                        lstempresa.Add(empresa);
                    }
                    con.Close();
                }
                return lstempresa;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public Empresa GetEmpresaData(string id)
        {
            try
            {
                Empresa empresa = new Empresa();
                {
                    string consulta = "select id,activo,nombre,abrev,bd_name,contrasenia_etl,desc_id,etl,fec_modif,host,puerto_compania,usuario_etl,moneda_id" +
                        "from empresa  where  @id  =" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        empresa.id = Convert.ToInt32(rdr["id"]);
                        empresa.desc_id = rdr["desc_id"].ToString().Trim();
                        empresa.activo = Convert.ToBoolean(rdr["activo"]);
                        empresa.nombre = rdr["nombre"].ToString().Trim();
                        empresa.abrev = rdr["abrev"].ToString().Trim();
                        empresa.bd_name = rdr["bd_name"].ToString().Trim();
                        empresa.contrasenia_etl = rdr["contrasenia_etl"].ToString().Trim();
                        empresa.etl = Convert.ToBoolean(rdr["etl"]);
                        empresa.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                        empresa.host = rdr["host"].ToString().Trim();
                        empresa.puerto_compania = Convert.ToInt32(rdr["puerto_compania"]);
                        empresa.usuario_etl = rdr["usuario_etl"].ToString().Trim();
                        empresa.moneda_id = Convert.ToInt32(rdr["moneda_id"]);
                    }
                    con.Close();
                }
                return empresa;
            }
            catch
            {
                con.Close();
                throw;
            }
        }
        public int Add(Empresa empresa)
        {

            string add = "insert into " +
                "empresa" +
                "(" + "id" +
                "," + "nombre" +
                "," + "abrev" +
                "," + "etl" +
                "," + "host" +
                "," + "moneda_id" +
                "," + "desc_id" +
                "," + "usuario_etl" +
                "," + "contrasenia_etl" +
                "," + "puerto_compania" +
                "," + "bd_name" +
                "," + "fec_modif" +
                "," + "activo" +
                ") values " +
                "(" + "nextval('seq_empresa')," +
                "@nombre," +
                "@abrev," +
                "@etl," +
                "@host," +
                "@moneda_id ," +
                "@desc_id," +
                "@usuario_etl," +
                "@contrasenia_etl," +
                "@puerto_compania," +
                "@bd_name," +
                "@fec_modif," +
                "@activo)";
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@desc_id", empresa.desc_id.Trim());
                    cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                    cmd.Parameters.AddWithValue("@abrev", empresa.abrev);
                    cmd.Parameters.AddWithValue("@etl", empresa.etl);
                    cmd.Parameters.AddWithValue("@host", empresa.host);
                    cmd.Parameters.AddWithValue("@moneda_id", empresa.moneda_id);
                    cmd.Parameters.AddWithValue("@usuario_etl", empresa.usuario_etl);
                    cmd.Parameters.AddWithValue("@contrasenia_etl", empresa.contrasenia_etl);
                    cmd.Parameters.AddWithValue("@puerto_compania", empresa.puerto_compania);
                    cmd.Parameters.AddWithValue("@bd_name", empresa.bd_name);
                    cmd.Parameters.AddWithValue("@fec_modif", DateTime.Now);
                    cmd.Parameters.AddWithValue("@activo", empresa.activo);
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
        public int Update(string id, Empresa empresa)
        {
            string update = "update empresa set "
                 + "desc_id = @desc_id ,"
                 + "nombre = @nombre ,"
                 + "abrev = @abrev ,"
                 + "etl = @etl ,"
                 + "host = @host ,"
                 + "moneda_id = @moneda_id ,"
                 + "usuario_etl = @usuario_etl ,"
                 + "contrasenia_etl = @contrasenia_etl ,"
                 + "puerto_compania = @puerto_compania ,"
                 + "activo = @activo ,"
                 + "fec_modif = @fec_modif ,"
                 + "bd_name = @bd_name"
                + " where id = " + id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@desc_id", empresa.desc_id);
                    cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                    cmd.Parameters.AddWithValue("@abrev", empresa.abrev);
                    cmd.Parameters.AddWithValue("@etl", empresa.etl);
                    cmd.Parameters.AddWithValue("@host", empresa.host);
                    cmd.Parameters.AddWithValue("@usuario_etl", empresa.usuario_etl);
                    cmd.Parameters.AddWithValue("@contrasenia_etl", empresa.contrasenia_etl);
                    cmd.Parameters.AddWithValue("@puerto_compania", empresa.puerto_compania);
                    cmd.Parameters.AddWithValue("@bd_name", empresa.bd_name);
                    cmd.Parameters.AddWithValue("@activo", empresa.activo);
                    cmd.Parameters.AddWithValue("@fec_modif", DateTime.Now);
                    cmd.Parameters.AddWithValue("@moneda_id", empresa.moneda_id);

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
            string status = "false";
            string delete = "update  empresa set activo  = " + status + " where  id   = " + id;
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
