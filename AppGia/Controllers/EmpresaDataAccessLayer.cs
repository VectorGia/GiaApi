using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class EmpresaDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        char cod = '"';

        public EmpresaDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Empresa> GetAllEmpresas()
        {
            string cadena = "select * from " + "empresa " + "where " + "activo " + "=" + true;
            try
            {
                List<Empresa> lstempresa = new List<Empresa>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                     
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        
                        Empresa empresa = new Empresa();
                        empresa.id = Convert.ToInt32(rdr["id"]);
                        empresa.desc_id = rdr["desc_id"].ToString().Trim();
                        empresa.nombre = rdr["nombre"].ToString().Trim();
                        empresa.abrev = rdr["abrev"].ToString().Trim();
                        empresa.etl = Convert.ToBoolean(rdr["etl"]);
                        empresa.host = rdr["host"].ToString().Trim();
                        empresa.puerto_compania = Convert.ToInt32(rdr["puerto_compania"]);
                        empresa.usuario_etl = rdr["usuario_etl"].ToString().Trim();
                        empresa.contrasenia_etl = rdr["contrasenia_etl"].ToString().Trim();
                        empresa.bd_name = rdr["bd_name"].ToString().Trim();
                        empresa.moneda_id = Convert.ToInt32(rdr["moneda_id"]);
                        empresa.fec_modif = Convert.ToDateTime (rdr["fec_modif"]);
                        empresa.activo = Convert.ToBoolean(rdr["activo"]);
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
                    string consulta = "select * from " + "empresa " + "where " + "id" + "=" + id;
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        empresa.id = Convert.ToInt32(rdr["id"]);
                        empresa.desc_id = rdr["desc_id"].ToString().Trim();
                        empresa.nombre = rdr["nombre"].ToString().Trim();
                        empresa.abrev = rdr["abrev"].ToString().Trim();
                        empresa.etl = Convert.ToBoolean(rdr["etl"]);
                        empresa.host = rdr["host"].ToString().Trim();
                        empresa.puerto_compania = Convert.ToInt32(rdr["puerto_compania"]);
                        empresa.usuario_etl = rdr["usuario_etl"].ToString().Trim();
                        empresa.contrasenia_etl = rdr["contrasenia_etl"].ToString().Trim();
                        empresa.bd_name = rdr["bd_name"].ToString().Trim();
                        empresa.moneda_id = Convert.ToInt32(rdr["moneda_id"]);
                        empresa.fec_modif = Convert.ToDateTime(rdr["fec_modif"]);
                        empresa.activo = Convert.ToBoolean(rdr["activo"]);

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

        public int AddCompania(Empresa empresa)
        {
            string add = "insert into "  +
                "empresa"  +
                "(id,"+"nombre"+
                ","+"abrev"+
                ","+ "etl" +
                ","+"host"+
                ","+"moneda_id"+
                ","+"desc_id"+
                ","+"usuario_etl"+
                ","+ "contrasenia_etl" +
                ","+ "puerto_compania" +
                ","+ "bd_name" +
                ","+ "fec_modif" +
                ","+"activo"+
                ") values " +
                "(nextval('seq_empresa')," +
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
          
                    cmd.Parameters.AddWithValue("@nombre", empresa.nombre);
                    cmd.Parameters.AddWithValue("@abrev", empresa.abrev );
                    cmd.Parameters.AddWithValue("@etl", empresa.etl);
                    cmd.Parameters.AddWithValue("@host", empresa.host );
                    cmd.Parameters.AddWithValue("@moneda_id", empresa.moneda_id );
                    cmd.Parameters.AddWithValue("@desc_id", empresa.desc_id.Trim());
                    cmd.Parameters.AddWithValue("@usuario_etl", empresa.usuario_etl );
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
          
            string update = "update "  + "empresa" 
                 + "set" 
                 + "desc_id"  + "=" + "@desc_id" +","
                 + "nombre" + "= " + "@nombre" + ","
                 + "abrev"  + "= " + "@abrev"  + ","
                 + "activo_etl"   + "= " + "@activo_etl"   + ","
                 + "host"  + "= " + "@host"   + ","
                 + "moneda_id" + "= " + "@moneda_id" + ","
                 + "usuario_etl"    + "= " + "@usuario_etl"     + ","
                 + "contrasenia_etl" + "= " + "@contrasenia_etl" + ","
                 + "puerto_compania" + "= " + "@puerto_compania" + ","
                 + "activo" +  "= " + "@activo" + ","
                 + "fec_modif" + "= " + "@fec_modif" + ","
                 + "bd_name"    + "= " + "@bd_name"
                + " where "+cod+"id"+ "=" + id;

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(update.Trim(), con);
                    cmd.Parameters.AddWithValue("id_empresa", empresa.desc_id);
                    cmd.Parameters.AddWithValue("@STR_NOMBRE_COMPANIA", empresa.nombre);
                    cmd.Parameters.AddWithValue("@STR_ABREV_COMPANIA", empresa.abrev);
                    cmd.Parameters.AddWithValue("@BOOL_ETL_COMPANIA", empresa.activo);
                    cmd.Parameters.AddWithValue("@STR_HOST_COMPANIA", empresa.host);
                    cmd.Parameters.AddWithValue("@STR_USUARIO_ETL", empresa.usuario_etl);
                    cmd.Parameters.AddWithValue("@STR_CONTRASENIA_ETL", empresa.contrasenia_etl);
                    cmd.Parameters.AddWithValue("@STR_PUERTO_COMPANIA", empresa.puerto_compania);
                    cmd.Parameters.AddWithValue("@STR_BD_COMPANIA", empresa.bd_name);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_COMPANIA", empresa.estatus);
                    cmd.Parameters.AddWithValue("@FEC_MODIF_COMPANIA", DateTime.Now);

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
            string delete = "UPDATE " + cod + "CAT_COMPANIA" + cod + "SET" 
                + cod + "BOOL_ESTATUS_LOGICO_COMPANIA" + cod + "='" + status + "' " +
                "WHERE" + cod + "INT_IDCOMPANIA_P" + cod + "='" + id + "'";
           
            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(delete.Trim(), con);


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
