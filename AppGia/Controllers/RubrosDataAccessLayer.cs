using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Controllers
{
    public class RubrosDataAccesLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();



        public RubrosDataAccesLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<Rubros> GetAllRubros()
        {
            string consulta = "select modelo_negocio.id, rubro.clave, rubro.nombre as nombre, rubro.hijos, rubro.rangos_cuentas_incluidas,"
             + "rubro.rango_cuentas_excluidas, rubro.activo, rubro.tipo_cuenta from rubro " +
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
                    rubro.id = Convert.ToInt32(rdr["id"]);
                    rubro.nombre = rdr["nombre"].ToString().Trim();
                    rubro.hijos = rdr["hijos"].ToString().Trim();
                    rubro.clave = rdr["clave"].ToString().Trim();
                    rubro.rango_cuentas_excluidas = rdr["rango_cuentas_excluidas"].ToString().Trim();
                    rubro.rangos_cuentas_incluidas = rdr["rangos_cuentas_incluidas"].ToString().Trim();
                    rubro.tipo_cuenta = rdr["tipo_cuenta"].ToString().Trim();
                    rubro.tipo_cuenta = rdr["tipo_agrupador"].ToString().Trim();
                    rubro.activo = Convert.ToBoolean(rdr["activo"]);

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
                    rubro.id = Convert.ToInt32(rdr["id"]);
                    rubro.nombre = rdr["nombre"].ToString().Trim();
                    rubro.clave = rdr["clave"].ToString().Trim();
                    rubro.aritmetica = rdr["aritmetica"].ToString().Trim();
                    rubro.rango_cuentas_excluidas = rdr["rango_cuentas_excluidas"].ToString().Trim();
                    rubro.rangos_cuentas_incluidas = rdr["rangos_cuentas_incluidas"].ToString().Trim();
                    rubro.tipo_cuenta = rdr["tipo_cuenta"].ToString().Trim();
                    rubro.tipo_agrupador = rdr["tipo_agrupador"].ToString().Trim();
                    rubro.hijos = rdr["hijos"].ToString().Trim();   
                    rubro.naturaleza = rdr["naturaleza"].ToString().Trim();
                    rubro.id_modelo_neg = Convert.ToInt32(rdr["id_modelo_neg"]);
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

        public int InsertRubro(Rubros rubro)

            
        {
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
                 + "id_modelo_neg," + "hijos" + ")"
                 + "values (nextval('seq_rubro'),@nombre" + ","
                 + "@rango_cuentas_excluidas" + ","
                 + "@rangos_cuentas_incluidas" + ","
                 + "@tipo_cuenta" + ","
                 + "@tipo_agrupador" + ","
                 + "@activo" + ","
                 + "@aritmetica" + ","
                 + "@clave" + ","
                 + "@tipo_id" + ","
                 + "@id_modelo_neg, @hijos )";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@id", Value = rubro.id });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@nombre", Value = rubro.nombre.Trim() });
       
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@rango_cuentas_excluidas", Value = rubro.rango_cuentas_excluidas!=null?rubro.rango_cuentas_excluidas.Trim():"" });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@rangos_cuentas_incluidas", Value = rubro.rangos_cuentas_incluidas !=null?rubro.rangos_cuentas_incluidas.Trim():"" });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo_cuenta", Value = rubro.tipo_cuenta.Trim()});
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo_agrupador", Value = rubro.tipo_agrupador.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = rubro.activo });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@aritmetica", Value = rubro.aritmetica !=null?rubro.aritmetica.Trim():"" });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@clave", Value = rubro.clave.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@tipo_id", Value = rubro.tipo_id });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@id_modelo_neg", Value = rubro.id_modelo_neg });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@hijos", Value = rubro.hijos.Trim() });


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
            string add = "UPDATE rubro " +
                "SET " +
                "nombre = @nombre, " +
                "aritmetica = @aritmetica, " +
                "clave = @clave, " +
                "rango_cuentas_excluidas = @rango_cuentas_excluidas," +
                "rangos_cuentas_incluidas = @rangos_cuentas_incluidas " +
                "tipo_cuenta = @tipo_cuenta " +
                "tipo_agrupador = @tipo_agrupador " +
                "where id = " + id;

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@nombre", Value = rubro.nombre.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@aritmetica", Value = rubro.aritmetica.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@clave", Value = rubro.clave.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@rango_cuentas_excluidas", Value = rubro.rango_cuentas_excluidas.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@rangos_cuentas_incluidas", Value = rubro.rangos_cuentas_incluidas.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo_cuenta", Value = rubro.tipo_cuenta });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@tipo_agrupador", Value = rubro.tipo_cuenta });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@id", Value = rubro.id });

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
