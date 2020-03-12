using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGia.Models;
using Npgsql;

namespace AppGia.Dao
{
    public class MontosConsolidadosDataAccessLayer
    {
        NpgsqlConnection con;
        Conexion.Conexion conex = new Conexion.Conexion();

        public MontosConsolidadosDataAccessLayer()
        {
            con = conex.ConnexionDB();
        }

        public IEnumerable<MontosConsolidados> GetMontosConsolidados(int montConsAnio, int montConsMes, int montConsEmpresa, int montConsModeloNeg, int montConsProyecto, int montConsRubro)
        {
            string consulta = "";
            consulta += " select ";
            consulta += "	 id, anio, mes, empresa_id, modelo_negocio_id, proyecto_id, rubro_id, ";
            consulta += "	 coalesce(enero_abono_financiero, 0) as enero_abono_financiero, coalesce(enero_abono_resultado, 0) as enero_abono_resultado, ";
            consulta += "	 coalesce(enero_cargo_financiero, 0) as enero_cargo_financiero, coalesce(enero_cargo_resultado, 0) as enero_cargo_resultado, ";
            consulta += "	 coalesce(febrero_abono_financiero, 0) as febrero_abono_financiero, coalesce(febrero_abono_resultado, 0) as febrero_abono_resultado, ";
            consulta += "	 coalesce(febrero_cargo_financiero, 0) as febrero_cargo_financiero, coalesce(febrero_cargo_resultado, 0) as febrero_cargo_resultado, ";
            consulta += "	 coalesce(marzo_abono_financiero, 0) as marzo_abono_financiero, coalesce(marzo_abono_resultado, 0) as marzo_abono_resultado, ";
            consulta += "	 coalesce(marzo_cargo_financiero, 0) as marzo_cargo_financiero, coalesce(marzo_cargo_resultado, 0) as marzo_cargo_resultado, ";
            consulta += "	 coalesce(abril_abono_financiero, 0) as abril_abono_financiero, coalesce(abril_abono_resultado, 0) as abril_abono_resultado, ";
            consulta += "	 coalesce(abril_cargo_financiero, 0) as abril_cargo_financiero, coalesce(abril_cargo_resultado, 0) as abril_cargo_resultado, ";
            consulta += "	 coalesce(mayo_abono_financiero, 0) as mayo_abono_financiero, coalesce(mayo_abono_resultado, 0) as mayo_abono_resultado, ";
            consulta += "	 coalesce(mayo_cargo_financiero, 0) as mayo_cargo_financiero, coalesce(mayo_cargo_resultado, 0) as mayo_cargo_resultado, ";
            consulta += "	 coalesce(junio_abono_financiero, 0) as junio_abono_financiero, coalesce(junio_abono_resultado, 0) as junio_abono_resultado, ";
            consulta += "	 coalesce(junio_cargo_financiero, 0) as junio_cargo_financiero, coalesce(junio_cargo_resultado, 0) as junio_cargo_resultado, ";
            consulta += "	 coalesce(julio_abono_financiero, 0) as julio_abono_financiero, coalesce(julio_abono_resultado, 0) as julio_abono_resultado, ";
            consulta += "	 coalesce(julio_cargo_financiero, 0) as julio_cargo_financiero, coalesce(julio_cargo_resultado, 0) as julio_cargo_resultado, ";
            consulta += "	 coalesce(agosto_abono_financiero, 0) as agosto_abono_financiero, coalesce(agosto_abono_resultado, 0) as agosto_abono_resultado, ";
            consulta += "	 coalesce(agosto_cargo_financiero, 0) as agosto_cargo_financiero, coalesce(agosto_cargo_resultado, 0) as agosto_cargo_resultado, ";
            consulta += "	 coalesce(septiembre_abono_financiero, 0) as septiembre_abono_financiero, coalesce(septiembre_abono_resultado, 0) as septiembre_abono_resultado, ";
            consulta += "	 coalesce(septiembre_cargo_financiero, 0) as septiembre_cargo_financiero, coalesce(septiembre_cargo_resultado, 0) as septiembre_cargo_resultado, ";
            consulta += "	 coalesce(octubre_abono_financiero, 0) as octubre_abono_financiero, coalesce(octubre_abono_resultado, 0) as octubre_abono_resultado, ";
            consulta += "	 coalesce(octubre_cargo_financiero, 0) as octubre_cargo_financiero, coalesce(octubre_cargo_resultado, 0) as octubre_cargo_resultado, ";
            consulta += "	 coalesce(noviembre_abono_financiero, 0) as noviembre_abono_financiero, coalesce(noviembre_abono_resultado, 0) as noviembre_abono_resultado, ";
            consulta += "	 coalesce(noviembre_cargo_financiero, 0) as noviembre_cargo_financiero, coalesce(noviembre_cargo_resultado, 0) as noviembre_cargo_resultado, ";
            consulta += "	 coalesce(diciembre_abono_financiero, 0) as diciembre_abono_financiero, coalesce(diciembre_abono_resultado, 0) as diciembre_abono_resultado, ";
            consulta += "	 coalesce(diciembre_cargo_financiero, 0) as diciembre_cargo_financiero, coalesce(diciembre_cargo_resultado, 0) as diciembre_cargo_resultado, ";
            consulta += "	 coalesce(valor_tipo_cambio_financiero, 0) as valor_tipo_cambio_financiero, ";
            consulta += "	 coalesce(valor_tipo_cambio_resultado, 0) as valor_tipo_cambio_resultado, ";
            consulta += "	 activo, fecha ";
            consulta += " from montos_consolidados ";
            consulta += " where activo = 'true' ";
            consulta += " and anio = " + montConsAnio.ToString();
            consulta += " and mes = " + montConsMes.ToString();
            consulta += " and empresa_id = " + montConsEmpresa.ToString();
            consulta += " and modelo_negocio_id = " + montConsModeloNeg.ToString();
            consulta += " and proyecto_id = " + montConsProyecto.ToString();
            consulta += " and rubro_id = " + montConsRubro.ToString();

            try
            {
                List<MontosConsolidados> lstMontosConsolidados = new List<MontosConsolidados>();

                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                NpgsqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    MontosConsolidados montos_consolidados = new MontosConsolidados();
                    montos_consolidados.id = Convert.ToInt32(rdr["id"]);
                    montos_consolidados.anio = Convert.ToInt32(rdr["anio"]);
                    montos_consolidados.mes = Convert.ToInt32(rdr["mes"]);
                    montos_consolidados.empresa_id = Convert.ToInt32(rdr["empresa_id"]);
                    montos_consolidados.modelo_negocio_id = Convert.ToInt32(rdr["modelo_negocio_id"]);
                    montos_consolidados.proyecto_id = Convert.ToInt32(rdr["proyecto_id"]);
                    montos_consolidados.rubro_id = Convert.ToInt32(rdr["rubro_id"]);
                    montos_consolidados.enero_abono_financiero = Convert.ToDouble(rdr["enero_abono_financiero"]);
                    montos_consolidados.enero_abono_resultado = Convert.ToDouble(rdr["enero_abono_resultado"]);
                    montos_consolidados.enero_cargo_financiero = Convert.ToDouble(rdr["enero_cargo_financiero"]);
                    montos_consolidados.enero_cargo_resultado = Convert.ToDouble(rdr["enero_cargo_resultado"]);
                    montos_consolidados.febrero_abono_financiero = Convert.ToDouble(rdr["febrero_abono_financiero"]);
                    montos_consolidados.febrero_abono_resultado = Convert.ToDouble(rdr["febrero_abono_resultado"]);
                    montos_consolidados.febrero_cargo_financiero = Convert.ToDouble(rdr["febrero_cargo_financiero"]);
                    montos_consolidados.febrero_cargo_resultado = Convert.ToDouble(rdr["febrero_cargo_resultado"]);
                    montos_consolidados.marzo_abono_financiero = Convert.ToDouble(rdr["marzo_abono_financiero"]);
                    montos_consolidados.marzo_abono_resultado = Convert.ToDouble(rdr["marzo_abono_resultado"]);
                    montos_consolidados.marzo_cargo_financiero = Convert.ToDouble(rdr["marzo_cargo_financiero"]);
                    montos_consolidados.marzo_cargo_resultado = Convert.ToDouble(rdr["marzo_cargo_resultado"]);
                    montos_consolidados.abril_abono_financiero = Convert.ToDouble(rdr["abril_abono_financiero"]);
                    montos_consolidados.abril_abono_resultado = Convert.ToDouble(rdr["abril_abono_resultado"]);
                    montos_consolidados.abril_cargo_financiero = Convert.ToDouble(rdr["abril_cargo_financiero"]);
                    montos_consolidados.abril_cargo_resultado = Convert.ToDouble(rdr["abril_cargo_resultado"]);
                    montos_consolidados.mayo_abono_financiero = Convert.ToDouble(rdr["mayo_abono_financiero"]);
                    montos_consolidados.mayo_abono_resultado = Convert.ToDouble(rdr["mayo_abono_resultado"]);
                    montos_consolidados.mayo_cargo_financiero = Convert.ToDouble(rdr["mayo_cargo_financiero"]);
                    montos_consolidados.mayo_cargo_resultado = Convert.ToDouble(rdr["mayo_cargo_resultado"]);
                    montos_consolidados.junio_abono_financiero = Convert.ToDouble(rdr["junio_abono_financiero"]);
                    montos_consolidados.junio_abono_resultado = Convert.ToDouble(rdr["junio_abono_resultado"]);
                    montos_consolidados.junio_cargo_financiero = Convert.ToDouble(rdr["junio_cargo_financiero"]);
                    montos_consolidados.junio_cargo_resultado = Convert.ToDouble(rdr["junio_cargo_resultado"]);
                    montos_consolidados.julio_abono_financiero = Convert.ToDouble(rdr["julio_abono_financiero"]);
                    montos_consolidados.julio_abono_resultado = Convert.ToDouble(rdr["julio_abono_resultado"]);
                    montos_consolidados.julio_cargo_financiero = Convert.ToDouble(rdr["julio_cargo_financiero"]);
                    montos_consolidados.julio_cargo_resultado = Convert.ToDouble(rdr["julio_cargo_resultado"]);
                    montos_consolidados.agosto_abono_financiero = Convert.ToDouble(rdr["agosto_abono_financiero"]);
                    montos_consolidados.agosto_abono_resultado = Convert.ToDouble(rdr["agosto_abono_resultado"]);
                    montos_consolidados.agosto_cargo_financiero = Convert.ToDouble(rdr["agosto_cargo_financiero"]);
                    montos_consolidados.agosto_cargo_resultado = Convert.ToDouble(rdr["agosto_cargo_resultado"]);
                    montos_consolidados.septiembre_abono_financiero = Convert.ToDouble(rdr["septiembre_abono_financiero"]);
                    montos_consolidados.septiembre_abono_resultado = Convert.ToDouble(rdr["septiembre_abono_resultado"]);
                    montos_consolidados.septiembre_cargo_financiero = Convert.ToDouble(rdr["septiembre_cargo_financiero"]);
                    montos_consolidados.septiembre_cargo_resultado = Convert.ToDouble(rdr["septiembre_cargo_resultado"]);
                    montos_consolidados.octubre_abono_financiero = Convert.ToDouble(rdr["octubre_abono_financiero"]);
                    montos_consolidados.octubre_abono_resultado = Convert.ToDouble(rdr["octubre_abono_resultado"]);
                    montos_consolidados.octubre_cargo_financiero = Convert.ToDouble(rdr["octubre_cargo_financiero"]);
                    montos_consolidados.octubre_cargo_resultado = Convert.ToDouble(rdr["octubre_cargo_resultado"]);
                    montos_consolidados.noviembre_abono_financiero = Convert.ToDouble(rdr["noviembre_abono_financiero"]);
                    montos_consolidados.noviembre_abono_resultado = Convert.ToDouble(rdr["noviembre_abono_resultado"]);
                    montos_consolidados.noviembre_cargo_financiero = Convert.ToDouble(rdr["noviembre_cargo_financiero"]);
                    montos_consolidados.noviembre_cargo_resultado = Convert.ToDouble(rdr["noviembre_cargo_resultado"]);
                    montos_consolidados.diciembre_abono_financiero = Convert.ToDouble(rdr["diciembre_abono_financiero"]);
                    montos_consolidados.diciembre_abono_resultado = Convert.ToDouble(rdr["diciembre_abono_resultado"]);
                    montos_consolidados.diciembre_cargo_financiero = Convert.ToDouble(rdr["diciembre_cargo_financiero"]);
                    montos_consolidados.diciembre_cargo_resultado = Convert.ToDouble(rdr["diciembre_cargo_resultado"]);
                    montos_consolidados.valor_tipo_cambio_financiero = Convert.ToDouble(rdr["valor_tipo_cambio_financiero"]);
                    montos_consolidados.valor_tipo_cambio_resultado = Convert.ToDouble(rdr["valor_tipo_cambio_resultado"]);
                    montos_consolidados.activo = Convert.ToBoolean(rdr["activo"]);
                    montos_consolidados.fecha = Convert.ToDateTime(rdr["fecha"]);
                    lstMontosConsolidados.Add(montos_consolidados);
                }
                return lstMontosConsolidados;
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

        public int AddMontosConsolidados(MontosConsolidados montos_consolidados)
        {
            string consulta = "";
            consulta += " insert into montos_consolidados ( ";
            consulta += "	 id, anio, mes, empresa_id, modelo_negocio_id, proyecto_id, rubro_id, ";
            consulta += "	 enero_abono_financiero, enero_abono_resultado, enero_cargo_financiero, enero_cargo_resultado, ";
            consulta += "	 febrero_abono_financiero, febrero_abono_resultado, febrero_cargo_financiero, febrero_cargo_resultado, ";
            consulta += "	 marzo_abono_financiero, marzo_abono_resultado, marzo_cargo_financiero, marzo_cargo_resultado, ";
            consulta += "	 abril_abono_financiero, abril_abono_resultado, abril_cargo_financiero, abril_cargo_resultado, ";
            consulta += "	 mayo_abono_financiero, mayo_abono_resultado, mayo_cargo_financiero, mayo_cargo_resultado, ";
            consulta += "	 junio_abono_financiero, junio_abono_resultado, junio_cargo_financiero, junio_cargo_resultado, ";
            consulta += "	 julio_abono_financiero, julio_abono_resultado, julio_cargo_financiero, julio_cargo_resultado, ";
            consulta += "	 agosto_abono_financiero, agosto_abono_resultado, agosto_cargo_financiero, agosto_cargo_resultado, ";
            consulta += "	 septiembre_abono_financiero, septiembre_abono_resultado, septiembre_cargo_financiero, septiembre_cargo_resultado, ";
            consulta += "	 octubre_abono_financiero, octubre_abono_resultado, octubre_cargo_financiero, octubre_cargo_resultado, ";
            consulta += "	 noviembre_abono_financiero, noviembre_abono_resultado, noviembre_cargo_financiero, noviembre_cargo_resultado, ";
            consulta += "	 diciembre_abono_financiero, diciembre_abono_resultado, diciembre_cargo_financiero, diciembre_cargo_resultado, ";
            consulta += "	 valor_tipo_cambio_financiero, valor_tipo_cambio_resultado, activo, fecha ";
            consulta += "	 ) values ( ";
            consulta += "	 nextval('seq_montos_consol'), @anio, @mes, @empresa_id, @modelo_negocio_id, @proyecto_id, @rubro_id, ";
            consulta += "	 @enero_abono_financiero, @enero_abono_resultado, @enero_cargo_financiero, @enero_cargo_resultado, ";
            consulta += "	 @febrero_abono_financiero, @febrero_abono_resultado, @febrero_cargo_financiero, @febrero_cargo_resultado, ";
            consulta += "	 @marzo_abono_financiero, @marzo_abono_resultado, @marzo_cargo_financiero, @marzo_cargo_resultado, ";
            consulta += "	 @abril_abono_financiero, @abril_abono_resultado, @abril_cargo_financiero, @abril_cargo_resultado, ";
            consulta += "	 @mayo_abono_financiero, @mayo_abono_resultado, @mayo_cargo_financiero, @mayo_cargo_resultado, ";
            consulta += "	 @junio_abono_financiero, @junio_abono_resultado, @junio_cargo_financiero, @junio_cargo_resultado, ";
            consulta += "	 @julio_abono_financiero, @julio_abono_resultado, @julio_cargo_financiero, @julio_cargo_resultado, ";
            consulta += "	 @agosto_abono_financiero, @agosto_abono_resultado, @agosto_cargo_financiero, @agosto_cargo_resultado, ";
            consulta += "	 @septiembre_abono_financiero, @septiembre_abono_resultado, @septiembre_cargo_financiero, @septiembre_cargo_resultado, ";
            consulta += "	 @octubre_abono_financiero, @octubre_abono_resultado, @octubre_cargo_financiero, @octubre_cargo_resultado, ";
            consulta += "	 @noviembre_abono_financiero, @noviembre_abono_resultado, @noviembre_cargo_financiero, @noviembre_cargo_resultado, ";
            consulta += "	 @diciembre_abono_financiero, @diciembre_abono_resultado, @diciembre_cargo_financiero, @diciembre_cargo_resultado, ";
            consulta += "	 @valor_tipo_cambio_financiero, @valor_tipo_cambio_resultado, @activo, @fecha ";
            consulta += "	 ) ";

            try
            {
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);
                cmd.Parameters.AddWithValue("@anio", montos_consolidados.anio);
                cmd.Parameters.AddWithValue("@mes", montos_consolidados.mes);
                cmd.Parameters.AddWithValue("@empresa_id", montos_consolidados.empresa_id);
                cmd.Parameters.AddWithValue("@modelo_negocio_id", montos_consolidados.modelo_negocio_id);
                cmd.Parameters.AddWithValue("@proyecto_id", montos_consolidados.proyecto_id);
                cmd.Parameters.AddWithValue("@rubro_id", montos_consolidados.rubro_id);
                cmd.Parameters.AddWithValue("@enero_abono_financiero", montos_consolidados.enero_abono_financiero);
                cmd.Parameters.AddWithValue("@enero_abono_resultado", montos_consolidados.enero_abono_resultado);
                cmd.Parameters.AddWithValue("@enero_cargo_financiero", montos_consolidados.enero_cargo_financiero);
                cmd.Parameters.AddWithValue("@enero_cargo_resultado", montos_consolidados.enero_cargo_resultado);
                cmd.Parameters.AddWithValue("@febrero_abono_financiero", montos_consolidados.febrero_abono_financiero);
                cmd.Parameters.AddWithValue("@febrero_abono_resultado", montos_consolidados.febrero_abono_resultado);
                cmd.Parameters.AddWithValue("@febrero_cargo_financiero", montos_consolidados.febrero_cargo_financiero);
                cmd.Parameters.AddWithValue("@febrero_cargo_resultado", montos_consolidados.febrero_cargo_resultado);
                cmd.Parameters.AddWithValue("@marzo_abono_financiero", montos_consolidados.marzo_abono_financiero);
                cmd.Parameters.AddWithValue("@marzo_abono_resultado", montos_consolidados.marzo_abono_resultado);
                cmd.Parameters.AddWithValue("@marzo_cargo_financiero", montos_consolidados.marzo_cargo_financiero);
                cmd.Parameters.AddWithValue("@marzo_cargo_resultado", montos_consolidados.marzo_cargo_resultado);
                cmd.Parameters.AddWithValue("@abril_abono_financiero", montos_consolidados.abril_abono_financiero);
                cmd.Parameters.AddWithValue("@abril_abono_resultado", montos_consolidados.abril_abono_resultado);
                cmd.Parameters.AddWithValue("@abril_cargo_financiero", montos_consolidados.abril_cargo_financiero);
                cmd.Parameters.AddWithValue("@abril_cargo_resultado", montos_consolidados.abril_cargo_resultado);
                cmd.Parameters.AddWithValue("@mayo_abono_financiero", montos_consolidados.mayo_abono_financiero);
                cmd.Parameters.AddWithValue("@mayo_abono_resultado", montos_consolidados.mayo_abono_resultado);
                cmd.Parameters.AddWithValue("@mayo_cargo_financiero", montos_consolidados.mayo_cargo_financiero);
                cmd.Parameters.AddWithValue("@mayo_cargo_resultado", montos_consolidados.mayo_cargo_resultado);
                cmd.Parameters.AddWithValue("@junio_abono_financiero", montos_consolidados.junio_abono_financiero);
                cmd.Parameters.AddWithValue("@junio_abono_resultado", montos_consolidados.junio_abono_resultado);
                cmd.Parameters.AddWithValue("@junio_cargo_financiero", montos_consolidados.junio_cargo_financiero);
                cmd.Parameters.AddWithValue("@junio_cargo_resultado", montos_consolidados.junio_cargo_resultado);
                cmd.Parameters.AddWithValue("@julio_abono_financiero", montos_consolidados.julio_abono_financiero);
                cmd.Parameters.AddWithValue("@julio_abono_resultado", montos_consolidados.julio_abono_resultado);
                cmd.Parameters.AddWithValue("@julio_cargo_financiero", montos_consolidados.julio_cargo_financiero);
                cmd.Parameters.AddWithValue("@julio_cargo_resultado", montos_consolidados.julio_cargo_resultado);
                cmd.Parameters.AddWithValue("@agosto_abono_financiero", montos_consolidados.agosto_abono_financiero);
                cmd.Parameters.AddWithValue("@agosto_abono_resultado", montos_consolidados.agosto_abono_resultado);
                cmd.Parameters.AddWithValue("@agosto_cargo_financiero", montos_consolidados.agosto_cargo_financiero);
                cmd.Parameters.AddWithValue("@agosto_cargo_resultado", montos_consolidados.agosto_cargo_resultado);
                cmd.Parameters.AddWithValue("@septiembre_abono_financiero", montos_consolidados.septiembre_abono_financiero);
                cmd.Parameters.AddWithValue("@septiembre_abono_resultado", montos_consolidados.septiembre_abono_resultado);
                cmd.Parameters.AddWithValue("@septiembre_cargo_financiero", montos_consolidados.septiembre_cargo_financiero);
                cmd.Parameters.AddWithValue("@septiembre_cargo_resultado", montos_consolidados.septiembre_cargo_resultado);
                cmd.Parameters.AddWithValue("@octubre_abono_financiero", montos_consolidados.octubre_abono_financiero);
                cmd.Parameters.AddWithValue("@octubre_abono_resultado", montos_consolidados.octubre_abono_resultado);
                cmd.Parameters.AddWithValue("@octubre_cargo_financiero", montos_consolidados.octubre_cargo_financiero);
                cmd.Parameters.AddWithValue("@octubre_cargo_resultado", montos_consolidados.octubre_cargo_resultado);
                cmd.Parameters.AddWithValue("@noviembre_abono_financiero", montos_consolidados.noviembre_abono_financiero);
                cmd.Parameters.AddWithValue("@noviembre_abono_resultado", montos_consolidados.noviembre_abono_resultado);
                cmd.Parameters.AddWithValue("@noviembre_cargo_financiero", montos_consolidados.noviembre_cargo_financiero);
                cmd.Parameters.AddWithValue("@noviembre_cargo_resultado", montos_consolidados.noviembre_cargo_resultado);
                cmd.Parameters.AddWithValue("@diciembre_abono_financiero", montos_consolidados.diciembre_abono_financiero);
                cmd.Parameters.AddWithValue("@diciembre_abono_resultado", montos_consolidados.diciembre_abono_resultado);
                cmd.Parameters.AddWithValue("@diciembre_cargo_financiero", montos_consolidados.diciembre_cargo_financiero);
                cmd.Parameters.AddWithValue("@diciembre_cargo_resultado", montos_consolidados.diciembre_cargo_resultado);
                cmd.Parameters.AddWithValue("@valor_tipo_cambio_financiero", montos_consolidados.valor_tipo_cambio_financiero);
                cmd.Parameters.AddWithValue("@valor_tipo_cambio_resultado", montos_consolidados.valor_tipo_cambio_resultado);
                cmd.Parameters.AddWithValue("@activo", montos_consolidados.activo);
                cmd.Parameters.AddWithValue("@fecha", montos_consolidados.fecha);
                int regInsert = cmd.ExecuteNonQuery();

                return regInsert;
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

        public int UpdateMontosConsolidados(int idMontosConsolidados, bool bandActivo)
        {
            string consulta = "";
            consulta += " update montos_consolidados set activo = '" + bandActivo.ToString() +"' ";
            consulta += " where id = " + idMontosConsolidados.ToString();

            try
            {
                {
                    con.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(consulta.Trim(), con);

                    int regActual = cmd.ExecuteNonQuery();
                    return regActual;
                }
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

    }
}
