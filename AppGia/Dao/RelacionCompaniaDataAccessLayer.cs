using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    
    public class RelacionCompaniaDataAccessLayer 
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        char cod = '"';

        public RelacionCompaniaDataAccessLayer()
        {

            con = conex.ConnexionDB();
        }

        public IEnumerable<RelacionCompania> GetAllRelacionesCompania()
        {
            RelacionCompania relacionCompania = new RelacionCompania();
            string cadena = "SELECT * FROM " + cod + "TAB_RELACION_COMPANIA" + cod;
            try
            {
                List<RelacionCompania> lstRelacion = new List<RelacionCompania>();
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        relacionCompania.INT_IDRELACION_COMPANIA = Convert.ToInt32(rdr["INT_IDRELACION_COMPANIA"]);
                        relacionCompania.INT_IDCOMPANIA_P = Convert.ToInt32(rdr["INT_IDCOMPANIA_P"]);
                        relacionCompania.INT_IDMODELO_NEGOCIO_P = Convert.ToInt32(rdr["INT_IDMODELO_NEGOCIO_P"]);
                        relacionCompania.INT_IDPROYECTO_P = Convert.ToInt32(rdr["INT_IDPROYECTO_P"]);
                        relacionCompania.INT_IDCENTROSCOSTO_P = Convert.ToInt32(rdr["INT_IDCENTROSCOSTO_P"]);
                        relacionCompania.FECH_MODIF_RELCOMP = Convert.ToDateTime(rdr["FECH_MODIF_RELCOMP"]);
                        relacionCompania.BOOL_ESTATUS_LOGICO_RELACION_COMPANIA = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_RELACION_COMPANIA"]);

                        lstRelacion.Add(relacionCompania);
                    }
                    con.Close();
                }

                return lstRelacion;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
            finally
            {
                con.Close();
            }
        }

        public RelacionCompania GetRelacionCompaniaData(int id)
        {
            string consulta = "SELECT * FROM" + cod + "TAB_RELACION_COMPANIA" + cod + "WHERE" + cod + "INT_IDRELACION_COMPANIA" + cod + "=" + id; 
            try
            {
                RelacionCompania relacionCompania = new RelacionCompania();


                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta, con);


                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {

                        relacionCompania.INT_IDRELACION_COMPANIA = Convert.ToInt32(rdr["INT_IDRELACION_COMPANIA"]);
                        relacionCompania.INT_IDCOMPANIA_P = Convert.ToInt32(rdr["INT_IDCOMPANIA_P"]);
                        relacionCompania.INT_IDMODELO_NEGOCIO_P = Convert.ToInt32(rdr["INT_IDMODELO_NEGOCIO_P"]);
                        relacionCompania.INT_IDPROYECTO_P = Convert.ToInt32(rdr["INT_IDPROYECTO_P"]);
                        relacionCompania.INT_IDROL_P = Convert.ToInt32(rdr["INT_IDROL_P"]);
                        relacionCompania.INT_IDCENTROSCOSTO_P = Convert.ToInt32(rdr["INT_IDCENTROSCOSTO_P"]);
                        relacionCompania.BOOL_ESTATUS_LOGICO_RELACION_COMPANIA = Convert.ToBoolean(rdr["BOOL_ESTATUS_LOGICO_RELACION_COMPANIA"]);
                        relacionCompania.FECH_MODIF_RELCOMP = Convert.ToDateTime(rdr["FECH_MODIF_RELCOMP"]);

                    }

                    con.Close();
                }
                return relacionCompania;
            }
            catch  (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
            finally
            {
                con.Close();
            }
        }

        public int update(int id,RelacionCompania relacionCompania)
        {
            string add = "UPDATE " + cod + "TAB_RELACION_COMPANIA" + cod +
            " SET " + cod + "INT_IDCOMPANIA_P" + cod + "= " + "@INT_IDCOMPANIA_P" + ","
            + cod + "INT_IDMODELO_NEGOCIO_P" + cod + "= " + "@INT_IDMODELO_NEGOCIO_P" + ","
            + cod + "INT_IDPROYECTO_P" + cod + "= " + "@INT_IDPROYECTO_P" + ","
            + cod + "INT_IDCENTROSCOSTO_P" + cod + "= " + "@INT_IDCENTROSCOSTO_P" + ","
            + cod + "BOOL_ESTATUS_LOGICO_RELACION_COMPANIA" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELACION_COMPANIA" + ","
            + cod + "FECH_MODIF_RELCOMP" + cod + "= " + "@FECH_MODIF_RELCOMP"
            + " WHERE " + cod + "INT_IDRELACION_COMPANIA" + cod + " = " + id; //"@INT_IDRELACION_COMPANIA";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDCOMPANIA_P", Value = relacionCompania.INT_IDCOMPANIA_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDMODELO_NEGOCIO_P", Value = relacionCompania.INT_IDMODELO_NEGOCIO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDPROYECTO_P", Value = relacionCompania.INT_IDPROYECTO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDCENTROSCOSTO_P", Value = relacionCompania.INT_IDCENTROSCOSTO_P });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELACION_COMPANIA", Value = relacionCompania.BOOL_ESTATUS_LOGICO_RELACION_COMPANIA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@FECH_MODIF_RELCOMP", Value = relacionCompania.FECH_MODIF_RELCOMP });
                    //cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Date, ParameterName = "@INT_IDRELACION_COMPANIA", Value = relacionCompania.INT_IDRELACION_COMPANIA });


                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
            finally
            {
                con.Close();
            }

        }

        public int delete(RelacionCompania relacionCompania)
        {

            string add = "UPDATE " + cod + "TAB_RELACION_COMPANIA" + cod +
            " SET " + cod + "BOOL_ESTATUS_LOGICO_RELACION_COMPANIA" + cod + "= " + "@BOOL_ESTATUS_LOGICO_RELACION_COMPANIA"
            + " WHERE " + cod + "INT_IDRELACION_COMPANIA" + cod + " = " + "@INT_IDRELACION_COMPANIA";

            try
            {

                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);

                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Integer, ParameterName = "@INT_IDRELACION_COMPANIA", Value = relacionCompania.INT_IDRELACION_COMPANIA });
                    cmd.Parameters.Add(new NpgsqlParameter() { NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Boolean, ParameterName = "@BOOL_ESTATUS_LOGICO_RELACION_COMPANIA", Value = relacionCompania.BOOL_ESTATUS_LOGICO_RELACION_COMPANIA });

                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                    return cantFilas;
                }

            }

            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
            finally
            {
                con.Close();
            }

        }
        public int insert(RelacionCompania relacionCompania)

        {

            string add = "INSERT INTO" + cod + "TAB_RELACION_COMPANIA" + 
                                         cod + "(" + cod + "INT_IDCOMPANIA_P" + 
                                         cod + "," + cod + "INT_IDMODELO_NEGOCIO_P" + 
                                         cod + "," + cod + "INT_IDPROYECTO_P" +
                                         cod + "," + cod + "INT_IDROL_P" +
                                         cod + "," + cod + "INT_IDCENTROSCOSTO_P" + 
                                         cod + "," + cod + "BOOL_ESTATUS_LOGICO_RELACION_COMPANIA" + 
                                         cod + "," + cod + "FECH_MODIF_RELCOMP" + 
                                         cod + ") VALUES " +
                "(@INT_IDCOMPANIA_P,@INT_IDMODELO_NEGOCIO_P,@INT_IDPROYECTO_P,@INT_IDROL_P,@INT_IDCENTROSCOSTO_P,@BOOL_ESTATUS_LOGICO_RELACION_COMPANIA,@FECH_MODIF_RELCOMP)";
            try
            {

                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);
                    cmd.Parameters.AddWithValue("@INT_IDCOMPANIA_P", relacionCompania.INT_IDCOMPANIA_P);
                    cmd.Parameters.AddWithValue("@INT_IDMODELO_NEGOCIO_P", relacionCompania.INT_IDMODELO_NEGOCIO_P);
                    cmd.Parameters.AddWithValue("@INT_IDPROYECTO_P", relacionCompania.INT_IDPROYECTO_P);
                    cmd.Parameters.AddWithValue("@INT_IDROL_P", relacionCompania.INT_IDROL_P);
                    cmd.Parameters.AddWithValue("@INT_IDCENTROSCOSTO_P", relacionCompania.INT_IDCENTROSCOSTO_P);
                    cmd.Parameters.AddWithValue("@BOOL_ESTATUS_LOGICO_RELACION_COMPANIA", relacionCompania.BOOL_ESTATUS_LOGICO_RELACION_COMPANIA);
                    cmd.Parameters.AddWithValue("@FECH_MODIF_RELCOMP", DateTime.Now);
                    con.Open();
                    int cantFilas = cmd.ExecuteNonQuery();
                    con.Close();
                
                    return cantFilas;
            }
            catch (Exception ex)
            {
                con.Close();
                string error = ex.Message;
                throw;

            }
            finally
            {
                con.Close();
            }
        }

    }
}