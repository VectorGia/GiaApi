﻿using AppGia.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Util;

namespace AppGia.Dao
{
    public class TipoCambioDataAccessLayer
    {

        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();
        //char cod = '"';

        public TipoCambioDataAccessLayer()
        {

            con = conex.ConnexionDB();
        }

        public IEnumerable<TipoCambio> GetAllTipoCambio()
        {
            string cadena = "SELECT id, activo, estatus, fec_modif, fecha, valor, moneda_id FROM tipo_cambio";
            try
            {
                List<TipoCambio> lstTipoCambio = new List<TipoCambio>();



                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cadena, con);
                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        TipoCambio tipoCambio = new TipoCambio();
                        tipoCambio.id = Convert.ToInt64(rdr["id"]);
                        tipoCambio.activo = Convert.ToBoolean(rdr["activo"]);
                        tipoCambio.estatus = rdr["estatus"].ToString();
                        tipoCambio.fec_modif = rdr["fec_modif"].ToString();
                        tipoCambio.fecha = rdr["fecha"].ToString();
                        tipoCambio.valor = Convert.ToInt32(rdr["valor"]);
                        tipoCambio.moneda_id = Convert.ToInt64(rdr["moneda_id"]);

                        lstTipoCambio.Add(tipoCambio);
                    }
                    con.Close();
                }
                return
                        lstTipoCambio;
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

        public List<TipoCambio> GetTpoCambioPorIdProforma(int idProforma, int idTipoCaptura)
        {
            string consulta = "";
            consulta += " select t.id, t.moneda_id, t.valor, t.fecha ";
            consulta += "	 from tipo_cambio t, moneda m, empresa e, proforma p, centro_costo c ";
            consulta += "	 where t.moneda_id = m.id ";
            consulta += "	 and e.moneda_id = m.id ";
            consulta += "	 and p.centro_costo_id = c.id ";
            consulta += "	 and e.id = c.empresa_id ";
            consulta += "	 and t.fec_modif = ( ";
            switch (idTipoCaptura)
            {
                case Constantes.TipoCapturaContable:    // Busca la fecha de inicio de mes para el tipo de cambio
                    consulta += "	 select min(fec_modif) from tipo_cambio where moneda_id = m.id";
                    consulta += "		 and extract(month from fecha) = extract(month from current_date) ";
                    break;
                case Constantes.TipoCapturaFlujo:       // Busca la ultima fecha del tipo de cambio
                    consulta += "	 select max(fec_modif) from tipo_cambio where moneda_id = m.id ";
                    break;
            }
            consulta += "	 ) ";
            consulta += "	 and p.id = " + idProforma;

            try
            {
                List<TipoCambio> lstTipoCambio = new List<TipoCambio>();

                con.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TipoCambio detTipoCambio = new TipoCambio();
                    detTipoCambio.id = Convert.ToInt64(rdr["id"]);
                    detTipoCambio.moneda_id = Convert.ToInt32(rdr["moneda_id"]);
                    detTipoCambio.valor = Convert.ToInt32(rdr["valor"]);
                    detTipoCambio.fec_modif = Convert.ToDateTime(rdr["fec_modif"]).ToString("yyyy/mm/dd");
                    lstTipoCambio.Add(detTipoCambio);
                }
                return lstTipoCambio;
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public int insert(TipoCambio tipoCambio)
        {
            string add = "INSERT INTO tipo_cambio ("
                        + "id,"
                        + "activo,"
                        + "estatus,"
                        + "fec_modif,"
                        + "fecha,"
                        + "valor,"
                        + "moneda_id )"
                        + " VALUES ( nextval('seq_tipo_cambio'),"
                        + "@activo,"
                        + "@estatus,"
                        + "@fec_modif,"
                        + "@fecha,"
                        + "@valor,"
                        + "@moneda_id)";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(add, con);


                    cmd.Parameters.AddWithValue("@activo", tipoCambio.activo);
                    cmd.Parameters.AddWithValue("@estatus", tipoCambio.estatus);
                    cmd.Parameters.AddWithValue("@fec_modif", DateTime.Now);
                    cmd.Parameters.AddWithValue("@fecha", tipoCambio.fecha);
                    cmd.Parameters.AddWithValue("@valor", tipoCambio.valor);
                    cmd.Parameters.AddWithValue("@moneda_id", tipoCambio.moneda_id);

                    con.Open();
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

        public int update(TipoCambio tipoCambio)
        {

            string update = "UPDATE tipo_cambio SET "
          + "activo = @activo ,"
          + "estatus = @estatus ,"
          + "fec_modif = @fec_modif ,"
          + "fecha = @fecha ,"
          + "valor = @valor"
          + " WHERE id = @id";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(update, con);

                    cmd.Parameters.AddWithValue("@activo", tipoCambio.activo);
                    cmd.Parameters.AddWithValue("@estatus", tipoCambio.estatus);
                    cmd.Parameters.AddWithValue("@fec_modif", tipoCambio.fec_modif);
                    cmd.Parameters.AddWithValue("@fecha", tipoCambio.fecha);
                    cmd.Parameters.AddWithValue("@valor", tipoCambio.valor);

                    con.Open();
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

        public int Delete(TipoCambio tipoCambio)
        {
            string delete = "UPDATE tipo_cambio SET estatus = @estatus WHERE id =@id ";
            try
            {
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
                    cmd.Parameters.AddWithValue("@estatus", tipoCambio.estatus);
                    cmd.Parameters.AddWithValue("@id", tipoCambio.id);

                    con.Open();
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
