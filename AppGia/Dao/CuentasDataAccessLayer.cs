using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Dao
{
    public class CuentasDataAccessLayer 
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public CuentasDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        char cod = '"';

        public IEnumerable<Cuentas> GetAllCuentas()
        {
            //Obtiene todas las Cuentas 
            string consulta = "SELECT id, activo, cta, descripcion, id_companiaf, sub_cta, sub_sub_cta FROM cuenta ";
            try
            {
                List<Cuentas> lstCuentas = new List<Cuentas>();
                {

                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);


                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Cuentas cuentas = new Cuentas();

                        cuentas.id = Convert.ToInt32(rdr["id"]);
                        cuentas.activo = Convert.ToBoolean(rdr["activo"].ToString().Trim());
                        cuentas.cta = rdr["cta"].ToString().Trim();
                        cuentas.descripcion = rdr["descripcion"].ToString().Trim();
                        cuentas.id_companiaf = Convert.ToInt32(rdr["id_companiaf"].ToString().Trim());
                        cuentas.sub_cta = rdr["sub_cta"].ToString();
                        cuentas.sub_sub_cta = rdr["sub_sub_cta"].ToString();

                        lstCuentas.Add(cuentas);
                    }
                    con.Close();
                }
                return lstCuentas;
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

        //Obtiene las cuentas de cada compañia 
        public List<Cuentas> GetCuentasData(string intIdCompania)
        {
            string consulta = "SELECT id, activo, cta, descripcion, id_companiaf, sub_cta, sub_sub_cta FROM cuenta WHERE id_companiaf = " + intIdCompania;
            try
            {
                List<Cuentas> listCuentas = new List<Cuentas>();
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Cuentas cuentas = new Cuentas();
                        cuentas.id = Convert.ToInt32(rdr["id"]);
                        cuentas.activo = Convert.ToBoolean(rdr["activo"].ToString().Trim());
                        cuentas.cta = rdr["cta"].ToString().Trim();
                        cuentas.descripcion = rdr["descripcion"].ToString().Trim();
                        cuentas.id_companiaf = Convert.ToInt32(rdr["id_companiaf"].ToString().Trim());
                        cuentas.sub_cta = rdr["sub_cta"].ToString().Trim();
                        cuentas.sub_sub_cta = rdr["sub_sub_cta"].ToString().Trim();
                        listCuentas.Add(cuentas);
                    }

                    con.Close();
                }
                return listCuentas;
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
        public int AddCuenta(Cuentas cuentas)
        {
            string add = "INSERT INTO cuenta ("
                + " id,"
                + " activo,"
                + " cta,"
                + " descripcion,"
                + " id_companiaf,"
                + " sub_cta,"
                + " sub_sub_cta"
                + " )" 
                +" VALUES (" 
                +" @nextval('seq_cuenta')," 
                +" @activo," 
                +" @cta," 
                +" @descripcion," 
                +" @id_companiaf," 
                +" @sub_cta," 
                +" @sub_sub_cta)";

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@activo", NpgsqlTypes.NpgsqlDbType.Boolean, cuentas.activo);
                    cmd.Parameters.AddWithValue("@cta", cuentas.cta.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", cuentas.descripcion.Trim());
                    cmd.Parameters.AddWithValue("@id_companiaf", cuentas.id_companiaf);
                    cmd.Parameters.AddWithValue("@sub_cta", cuentas.sub_cta.Trim());
                    cmd.Parameters.AddWithValue("@sub_sub_cta", cuentas.sub_sub_cta.Trim());

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
