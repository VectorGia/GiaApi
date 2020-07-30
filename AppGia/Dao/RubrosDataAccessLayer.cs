using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Controllers;
using AppGia.Models;
using AppGia.Util;
using NLog;
using Npgsql;
using NpgsqlTypes;
using static System.Convert;
using static NpgsqlTypes.NpgsqlDbType;

namespace AppGia.Dao
{
    public class RubrosDataAccesLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        QueryExecuter _queryExecuter=new QueryExecuter();

        private static Logger logger = LogManager.GetCurrentClassLogger();



        public RubrosDataAccesLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Rubros> GetAllRubros()
        {
            string consulta = "select modelo_negocio.id, rubro.clave, rubro.nombre as nombre, rubro.hijos, rubro.rangos_cuentas_incluidas,"
             + "rubro.rango_cuentas_excluidas, rubro.activo, rubro.tipo_cuenta,rubro.naturaleza,rubro.es_total_ingresos from rubro " +
                "inner join modelo_negocio on rubro.id_modelo_neg = modelo_negocio.id";
            try
            {
                List<Rubros> lstrubro = new List<Rubros>();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Rubros rubro = new Rubros();
                    rubro.id = ToInt32(rdr["id"]);
                    rubro.nombre = rdr["nombre"].ToString().Trim();
                    rubro.hijos = rdr["hijos"].ToString().Trim();
                    rubro.clave = rdr["clave"].ToString().Trim();
                    rubro.rango_cuentas_excluidas = rdr["rango_cuentas_excluidas"].ToString().Trim();
                    rubro.rangos_cuentas_incluidas = rdr["rangos_cuentas_incluidas"].ToString().Trim();
                    rubro.tipo_cuenta = rdr["tipo_cuenta"].ToString().Trim();
                    rubro.tipo_cuenta = rdr["tipo_agrupador"].ToString().Trim();
                    rubro.activo = ToBoolean(rdr["activo"]);
                    rubro.naturaleza = Convert.ToString(rdr["naturaleza"]);
                    rubro.es_total_ingresos = ToBoolean(rdr["es_total_ingresos"]);
;
                    lstrubro.Add(rubro);
                }
                con.Close();
                return lstrubro;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        public List<Rubros> GetRubroById(int id)
        {
            return GetRubro(id, " id ");
        }
        public List<Rubros> GetRubroByModeloId(int id)
        {
            return GetRubro(id," id_modelo_neg " );
        }
       

        public List<Rubros> GetRubro(int id,string nombreColumna)
        {
            string consulta = "select * from " + "rubro" + " where " + nombreColumna + " = " + id + " and activo = " + true;

            try
            {
               

                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                List<Rubros> lstRubros = new List<Rubros>();
                while (rdr.Read())
                {
                    Rubros rubro = new Rubros();
                    rubro.id = ToInt32(rdr["id"]);
                    rubro.nombre = rdr["nombre"].ToString().Trim();
                    rubro.clave = rdr["clave"].ToString().Trim();
                    rubro.aritmetica = rdr["aritmetica"].ToString().Trim();
                    rubro.rango_cuentas_excluidas = rdr["rango_cuentas_excluidas"].ToString().Trim();
                    rubro.rangos_cuentas_incluidas = rdr["rangos_cuentas_incluidas"].ToString().Trim();
                    rubro.tipo_cuenta = rdr["tipo_cuenta"].ToString().Trim();
                    rubro.tipo_agrupador = rdr["tipo_agrupador"].ToString().Trim();
                    rubro.hijos = rdr["hijos"].ToString().Trim();   
                    rubro.id_modelo_neg = ToInt32(rdr["id_modelo_neg"]);
                    rubro.naturaleza = Convert.ToString(rdr["naturaleza"]);
                    rubro.es_total_ingresos = ToBoolean(rdr["es_total_ingresos"]);
                    rubro.tipo_id= ToInt64(rdr["tipo_id"]);
                    lstRubros.Add(rubro);

                }
                con.Close();
                return lstRubros;
            }
            catch
            {
                con.Close();
                throw;
            }
        }

        private Boolean existeRubroTotalIngresos(Int64 idModelo,Int64 idRubro)
        {
            string query = "select count(1) as numRubrosTotIng from rubro " +
                           " where activo=true " +
                           " and id_modelo_neg=@id_modelo_neg " +
                           " and es_total_ingresos=true ";
            if (idRubro > 0)
            {
                query += " and id<>"+idRubro;
            }
            
            Int32 numRubrosTotIng=ToInt32(_queryExecuter.ExecuteQueryUniqueresult(query,
                new NpgsqlParameter("@id_modelo_neg", idModelo))["numRubrosTotIng"]);
            return numRubrosTotIng > 0;
        }
        public int InsertRubro(Rubros rubro)
        {
            if (rubro.es_total_ingresos&&existeRubroTotalIngresos(rubro.id_modelo_neg,0))
            {
                throw new DataException("Ya existe un rubro marcado como total de ingresos, favor de revisar");
            }
            
            string add = "insert into " + "rubro " + "("
                + "id" + ","
                 + "nombre" + ","
                 + "rango_cuentas_excluidas" + ","
                 + "rangos_cuentas_incluidas" + ","
                 + "tipo_cuenta" + ","
                 + "tipo_agrupador" + ","
                 + "activo" + ","
                 + "aritmetica" + ","
                 + "clave" + ","
                 + "tipo_id" + ","
                 + "id_modelo_neg," + "hijos" + ",naturaleza,es_total_ingresos)"
                 + "values (nextval('seq_rubro'),@nombre" + ","
                 + "@rango_cuentas_excluidas" + ","
                 + "@rangos_cuentas_incluidas" + ","
                 + "@tipo_cuenta" + ","
                 + "@tipo_agrupador" + ","
                 + "@activo" + ","
                 + "@aritmetica" + ","
                 + "@clave" + ","
                 + "@tipo_id" + ","
                 + "@id_modelo_neg, " 
                + " @hijos, " 
                + " @naturaleza,@es_total_ingresos )";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Integer, ParameterName = "@id", Value = rubro.id });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@nombre", Value = rubro.nombre.Trim() });
       
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@rango_cuentas_excluidas", Value = rubro.rango_cuentas_excluidas!=null?rubro.rango_cuentas_excluidas.Trim():"" });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@rangos_cuentas_incluidas", Value = rubro.rangos_cuentas_incluidas !=null?rubro.rangos_cuentas_incluidas.Trim():"" });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@tipo_cuenta", Value = rubro.tipo_cuenta.Trim()});
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@tipo_agrupador", Value = rubro.tipo_agrupador.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = NpgsqlDbType.Boolean, ParameterName = "@activo", Value = rubro.activo });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@aritmetica", Value = rubro.aritmetica !=null?rubro.aritmetica.Trim():"" });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@clave", Value = rubro.clave.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Integer, ParameterName = "@tipo_id", Value = rubro.tipo_id });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Integer, ParameterName = "@id_modelo_neg", Value = rubro.id_modelo_neg });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@hijos", Value = rubro.hijos.Trim() });

                if (rubro.tipo_id == Constantes.TipoRubroCuentas)
                {
                    cmd.Parameters.Add(new NpgsqlParameter {NpgsqlDbType = Text, ParameterName = "@naturaleza", Value = rubro.naturaleza.Trim()});
                }
                else
                {
                    cmd.Parameters.Add(new NpgsqlParameter {NpgsqlDbType = Text, ParameterName = "@naturaleza", Value = ""});
                }

                cmd.Parameters.Add(new NpgsqlParameter
                    { ParameterName = "@es_total_ingresos", Value = rubro.es_total_ingresos});

                con.Open();
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

        public int UpdateRubro(int id, Rubros rubro)
        {
            Int64 idModelo=ToInt64(_queryExecuter.ExecuteQueryUniqueresult("select id_modelo_neg from rubro where id=@id",
                new NpgsqlParameter("@id", id))["id_modelo_neg"]);

            if (rubro.es_total_ingresos&&existeRubroTotalIngresos(idModelo,id))
            {
                throw new DataException("Ya existe un rubro marcado como total de ingresos, favor de revisar");
            }
            string add = "UPDATE rubro " +
                "SET " +
                "nombre = @nombre, " +
                "hijos = @hijos," +
                "aritmetica = @aritmetica, " +
                "clave = @clave, " +
                "rango_cuentas_excluidas = @rango_cuentas_excluidas," +
                "rangos_cuentas_incluidas = @rangos_cuentas_incluidas, " +
                "tipo_cuenta = @tipo_cuenta, " +
                "naturaleza = @naturaleza, " +
                "es_total_ingresos = @es_total_ingresos " +
                //"tipo_agrupador = @tipo_agrupador " +
                "where id = " + id;

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);


                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@nombre", Value = rubro.nombre.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@aritmetica", Value = rubro.aritmetica.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@clave", Value = rubro.clave.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@rango_cuentas_excluidas", Value = rubro.rango_cuentas_excluidas.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@rangos_cuentas_incluidas", Value = rubro.rangos_cuentas_incluidas.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@tipo_cuenta", Value = rubro.tipo_cuenta });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@naturaleza", Value = rubro.naturaleza });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Integer, ParameterName = "@id", Value = rubro.id });
                cmd.Parameters.Add(new NpgsqlParameter { NpgsqlDbType = Text, ParameterName = "@hijos", Value = rubro.hijos });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@es_total_ingresos", Value = rubro.es_total_ingresos
                });
                
                con.Open();
                int cantFilAfec = cmd.ExecuteNonQuery();
                con.Close();

                return cantFilAfec;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en ejecucion de UpdateRubro");
                throw;
            }
            finally
            {
                con.Close();   
            }

        }

        public int DeleteRubro(int id)
        {
            string add = "UPDATE rubro SET activo = false where id = " + id;

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                con.Open();
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
}
