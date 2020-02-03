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
            string consulta = "select modelo_negocio.id, modelo_negocio.nombre as nombrem, rubro.nombre as nombre, rubro.rangos_cuentas_incluidas,"
             + "rubro.rango_cuentas_excluidas, rubro.activo from rubro " +
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
                    rubro.nombreM = rdr["nombrem"].ToString().Trim();
                    rubro.rango_cuentas_excluidas = rdr["rango_cuentas_excluidas"].ToString().Trim();
                    rubro.rangos_cuentas_incluidas = rdr["rangos_cuentas_incluidas"].ToString().Trim();
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

        public Rubros GetRubro(string id)
        {
            string consulta = "select * from " + "rubro" + " where " + "id" + " = " + id;

            try
            {
                Rubros rubro = new Rubros();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    rubro.id = Convert.ToInt32(rdr["id"]);
                    rubro.nombre = rdr["nombre"].ToString().Trim();
                    rubro.rango_cuentas_excluidas = rdr["rango_cuentas_excluias"].ToString().Trim();
                    rubro.rangos_cuentas_incluidas = rdr["rango_cuentas_incluidas"].ToString().Trim();
                    rubro.naturaleza = rdr["naturaleza"].ToString().Trim();

                }
                con.Close();
                return rubro;
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
                 + "activo" + ","
                 + "aritmetica" + ","
                 + "clave" + ","
                 + "tipo_id" + ","
                 + "id_modelo_neg" + ")"
                 + "values (nextval('seq_rubro'),@nombre" + ","
                 + "@rango_cuentas_excluidas" + ","
                 + "@rangos_cuentas_incluidas" + ","
                 + "@activo" + ","
                 + "@aritmetica" + ","
                 + "@clave" + ","
                 + "@tipo_id" + ","
                 + "@id_modelo_neg )";

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@nombre", Value = rubro.nombre.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@rango_cuentas_excluidas", Value = rubro.rango_cuentas_excluidas.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@rangos_cuentas_incluidas", Value = rubro.rangos_cuentas_incluidas.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@activo", Value = rubro.activo });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@aritmetica", Value = rubro.aritmetica.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, ParameterName = "@clave", Value = rubro.clave.Trim() });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@tipo_id", Value = rubro.tipo_id });
                cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@id_modelo_neg", Value = rubro.id_modelo_neg });

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
