using System;
using System.Collections.Generic;
using System.Data;
using AppGia.Models.Etl;
using Npgsql;

namespace AppGia.Dao.Etl
{
    public class ValidaExtraccionDataAccessLayer
    {
        NpgsqlConnection con = new NpgsqlConnection();
        Conexion.Conexion conex = new Conexion.Conexion();
        NpgsqlCommand com = new NpgsqlCommand();

        public ValidaExtraccionDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        /// <summary>
        /// Busca en la tabla y recupera todos lo registros de la tabla de extracción programada
        /// </summary>
        /// <returns>Data Table TAB_ETL_PROG</returns> 
        private DataTable FechaExtra()
        {
            string consulta = " select  id ,"
                              + " fecha_extraccion ,"
                              + " hora_extraccion ,"
                              + " id_empresa "
                              + " from  etl_prog ";

            try
            {
                con.Open();
                com = new NpgsqlCommand(consulta, con);
                NpgsqlDataAdapter daP = new NpgsqlDataAdapter(com);

                DataTable dt = new DataTable();
                daP.Fill(dt);
                return dt;
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

        public List<ETLProg> lstParametros()
        {
            List<ETLProg> lstEtlP = new List<ETLProg>();
            DataTable dt = new DataTable();
            dt = FechaExtra();
            foreach (DataRow r in dt.Rows)
            {
                ETLProg etlProg = new ETLProg();
                etlProg.id = Convert.ToInt32(r["id"]);
                etlProg.fecha_extraccion = r["fecha_extraccion"].ToString();
                etlProg.hora_extraccion = r["hora_extraccion"].ToString();
                etlProg.id_empresa = Convert.ToInt32(r["id_empresa"]);

                lstEtlP.Add(etlProg);
            }

            return lstEtlP;
        }
    }
}