using AppGia.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AppGia.Controllers
{
    public class ajusteDataAccessLayer
    {

        SqlConnection conSQL;
        Conexion.Conexion conex = new Conexion.Conexion();
        SqlCommand com = new SqlCommand();

        public ajusteDataAccessLayer()
        {
            //Constructor
            conSQL = conex.ConexionSQL();
        }

        /// <summary>
        /// select * from 
        /// </summary>
        /// <param name="ajusteBalanza"></param>
        /// <returns></returns>
        public DataTable selectAjuste(AjusteBalanza ajusteBalanza)
        {
            string add = "SELECT * FROM ajuste ";

            try
            {
                conSQL.Open();
                com = new SqlCommand(add, conSQL);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                conSQL.Close();
                throw;
            }
            finally
            {
                conSQL.Close();
            }
        }

        /// <summary>
        /// where and
        /// </summary>
        /// <param name="ajusteBalanza"></param>
        /// <returns></returns>
        public DataTable selectAjusteEmpresaCC(AjusteBalanza ajusteBalanza)
        {
            string add = "SELECT * FROM ajuste WHERE empresa = " + ajusteBalanza.empresa +
                " AND centrocosto = " + ajusteBalanza.centrocosto;            

            try
            {
                conSQL.Open();
                com = new SqlCommand(add, conSQL);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                conSQL.Close();
                throw;
            }
            finally
            {
                conSQL.Close();
            }
        }

        /// <summary>
        /// where empresa
        /// </summary>
        /// <param name="ajusteBalanza"></param>
        /// <returns></returns>
        public DataTable selectAjusteEmpresa(AjusteBalanza ajusteBalanza)
        {
            string add = "SELECT * FROM ajuste WHERE empresa = " + ajusteBalanza.empresa;

            try
            {
                conSQL.Open();
                com = new SqlCommand(add, conSQL);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                conSQL.Close();
                throw;
            }
            finally
            {
                conSQL.Close();
            }
        }

        /// <summary>
        /// where centrocostos
        /// </summary>
        /// <param name="ajusteBalanza"></param>
        /// <returns></returns>
        public DataTable selectAjusteCC(AjusteBalanza ajusteBalanza)
        {
            string add = "SELECT * FROM ajuste WHERE centrocosto = " + ajusteBalanza.centrocosto;

            try
            {
                conSQL.Open();
                com = new SqlCommand(add, conSQL);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conSQL.Close();
                return dt;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                conSQL.Close();
                throw;
            }
            finally
            {
                conSQL.Close();
            }
        }

    }
}
