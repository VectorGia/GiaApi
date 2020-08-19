using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using AppGia.Models;
using AppGia.Util;
using Npgsql;
using NpgsqlTypes;

namespace AppGia.Dao
{
    public class EmpresaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        private QueryExecuter _queryExecuter=new QueryExecuter();

        public EmpresaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }
        public List<Empresa> GetAllEmpresas()
        {
            //string cadena = "select *from empresa where  activo = " + true;
            string cadena = "select id,activo,nombre,abrev,bd_name," +
                //"contrasenia_etl," +
                "desc_id,etl,fec_modif,host,puerto_compania,usuario_etl,moneda_id from empresa where activo = true order by id desc";
            DataTable dataTable = _queryExecuter.ExecuteQuery(cadena);
            List<Empresa> lstempresa = new List<Empresa>();
            foreach (DataRow rdr in dataTable.Rows)
            {
                Empresa empresa = new Empresa();
                empresa.id = Convert.ToInt32(rdr["id"]);
                empresa.desc_id = rdr["desc_id"].ToString().Trim();
                empresa.activo = Convert.ToBoolean(rdr["activo"]);
                empresa.nombre = rdr["nombre"].ToString().Trim();
                empresa.abrev = rdr["abrev"].ToString().Trim();
                empresa.bd_name = rdr["bd_name"].ToString().Trim();
                //empresa.contrasenia_etl = rdr["contrasenia_etl"].ToString().Trim();
                empresa.etl = Convert.ToBoolean(rdr["etl"]);
                empresa.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                empresa.host = rdr["host"].ToString().Trim();
                empresa.puerto_compania = Convert.ToInt32(rdr["puerto_compania"]);
                empresa.usuario_etl = rdr["usuario_etl"].ToString().Trim();
                empresa.moneda_id = Convert.ToInt32(rdr["moneda_id"]);
                lstempresa.Add(empresa);
            }
            return lstempresa;
        }

        public Empresa GetEmpresaData(Int64 id)
        {
            Empresa empresa = new Empresa();

            string consulta =
                " select id , activo , nombre , abrev , bd_name , desc_id,etl,fec_modif,host,puerto_compania,usuario_etl,moneda_id" +
                " from empresa  where  id  = @id";
            DataRow rdr = _queryExecuter.ExecuteQueryUniqueresult(consulta, new NpgsqlParameter("@id", id));

            empresa.id = Convert.ToInt32(rdr["id"]);
            empresa.desc_id = rdr["desc_id"].ToString().Trim();
            empresa.activo = Convert.ToBoolean(rdr["activo"]);
            empresa.nombre = rdr["nombre"].ToString().Trim();
            empresa.abrev = rdr["abrev"].ToString().Trim();
            empresa.bd_name = rdr["bd_name"].ToString().Trim();
            //empresa.contrasenia_etl = rdr["contrasenia_etl"].ToString().Trim();
            empresa.etl = Convert.ToBoolean(rdr["etl"]);
            empresa.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
            empresa.host = rdr["host"].ToString().Trim();
            empresa.puerto_compania = Convert.ToInt32(rdr["puerto_compania"]);
            empresa.usuario_etl = rdr["usuario_etl"].ToString().Trim();
            empresa.moneda_id = Convert.ToInt32(rdr["moneda_id"]);

            return empresa;
        }
        public long Add(Empresa empresa)
        {
            int existentes=Convert.ToInt32(_queryExecuter.ExecuteQueryUniqueresult(
                "select count(1) as existentes from empresa where desc_id=@desc_id and activo=true",
                new NpgsqlParameter("@desc_id", empresa.desc_id.Trim()))["existentes"]);
            if (existentes > 0)
            {
                throw new DataException("Ya existe una empresa registrada con el id="+empresa.desc_id);
            }
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
                    var transaction = con.BeginTransaction();
                    
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
                    cmd.CommandText = "SELECT currval('seq_empresa') AS lastEmpresa;";
                    long     idEmpresa = (long)cmd.ExecuteScalar();
                    transaction.Commit();
                    con.Close();
                    return idEmpresa;
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
            finally
            {
                con.Close();
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
            finally
            {
                con.Close();
            }
        }

        public int UpdateContrasenia(Empresa empresa)
        {
            using (Rijndael myRijndael = Rijndael.Create())
            {
                try
                {
                    byte[] encrypted = Utilerias.EncryptStringToBytes(empresa.contrasenia_etl, myRijndael.Key, myRijndael.IV);
                    con.Open();

                    string update = "update empresa set  contra_bytes = @contra_bytes, llave = @llave, apuntador=@apuntador where id = @id";
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Bytea, ParameterName = "@contra_bytes", Value = encrypted });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Bytea, ParameterName = "@llave", Value = myRijndael.Key });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Bytea, ParameterName = "@apuntador", Value = myRijndael.IV });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlDbType.Bigint, ParameterName = "@id", Value = empresa.id });

                    int cantF = cmd.ExecuteNonQuery();  
                    con.Close();
                    return cantF;
                }
                catch(Exception ex)
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
